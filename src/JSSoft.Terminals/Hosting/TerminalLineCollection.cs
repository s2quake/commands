// <copyright file="TerminalLineCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using JSSoft.Terminals.Hosting.Ansi;

namespace JSSoft.Terminals.Hosting;

internal sealed class TerminalLineCollection(Terminal terminal) : IReadOnlyList<TerminalLine>
{
    private static readonly Dictionary<char, IAsciiCode> AsciiCodeByCharacter = new()
    {
        { '\a', new TerminalBell() },
        { '\b', new Backspace() },
        { '\t', new HorizontalTAB() },
        { '\n', new Linefeed() },
        { '\v', new VerticalTAB() },
        { '\f', new Formfeed() },
        { '\r', new CarriageReturn() },
        { '\x1b', new EscapeCharacter() },
        { (char)127, new TerminalBell() },
    };

    private readonly TerminalArray<TerminalCharacterInfo?> _items = new();
    private readonly List<TerminalLine> _lineList = [];
    private readonly Terminal _terminal = terminal;
    private TerminalIndex _index = new(terminal, TerminalCoord.Empty);
    private TerminalIndex _beginIndex = new(terminal, TerminalCoord.Empty);
    private string _rest = string.Empty;

    public int Count => _lineList.Count;

    public TerminalIndex Index => _index;

    public TerminalLine this[int index] => _lineList[index];

    public TerminalLine this[TerminalIndex index] => _lineList[index.Y];

    public int IndexOf(TerminalLine item) => _lineList.IndexOf(item);

    public string GetString(params TerminalSelection[] selections)
    {
        var sb = new StringBuilder();
        var lines = new List<string>(selections.Length);
        foreach (var item in selections)
        {
            lines.Add(GetString(item));
        }

        return string.Join(Environment.NewLine, lines);
    }

    public bool TryGetLine(int index, [MaybeNullWhen(false)] out TerminalLine line)
    {
        if (index < _lineList.Count)
        {
            line = _lineList[index];
            return true;
        }

        line = null!;
        return false;
    }

    public bool TryGetLine(TerminalIndex index, [MaybeNullWhen(false)] out TerminalLine line)
    {
        if (index.Y < _lineList.Count)
        {
            line = _lineList[index.Y];
            return true;
        }

        line = null!;
        return false;
    }

    public TerminalCharacterInfo? GetCharacterInfo(TerminalIndex index)
        => index.Value < _items.Count ? _items[index.Value] : null;

    public TerminalLine Prepare(TerminalIndex beginIndex, TerminalIndex index)
    {
        var width = _terminal.BufferSize.Width;
        while (index.Y >= _lineList.Count)
        {
            var y = _lineList.Count;
            var line = new TerminalLine(_items, y, width);
            _lineList.Add(line);
            line.Parent = beginIndex.Y != index.Y ? _lineList[beginIndex.Y] : null;
        }

        _items.Expand(_lineList.Count * width);
        return _lineList[index.Y];
    }

    public void Take(TerminalIndex index)
    {
        while (index.Y + 1 < _lineList.Count)
        {
            _lineList[_lineList.Count - 1].Parent = null;
            _lineList.RemoveAt(_lineList.Count - 1);
        }

        if (index.Y < _lineList.Count)
        {
            _lineList[index.Y].Take(index.X);
        }

        _items.Take(index.Value);
    }

    public void Clear()
    {
        _index = new TerminalIndex(_terminal, TerminalCoord.Empty);
        _beginIndex = _index;
        for (var i = _lineList.Count - 1; i >= 0; i--)
        {
            var line = _lineList[i];
            line.Parent = null;
            line.Dispose();
        }

        _lineList.Clear();
        _items.Reset();
        AppendText(string.Empty, TerminalDisplayInfo.Empty);
    }

    public void Erase(TerminalIndex index, int length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        var index1 = index;
        var index2 = index + length;

        for (var i = index2.Y - 1; i >= index1.Y; i--)
        {
            if (i < 0 || i >= _lineList.Count)
            {
                continue;
            }

            var line = _lineList[i];
            var x1 = i == index1.Y ? index1.X : 0;
            var x2 = i == index2.Y ? index2.X : line.Length;
            line.Erase(x1, x2 - x1);
        }
    }

    public void Append(string text, TerminalDisplayInfo displayInfo)
    {
        AppendText(text, displayInfo);
    }

    public void Update()
    {
        Take(_index);
        var lines = this;
        var query = from item in lines
                    where item.Parent is null
                    select item.GetCharacterInfos();
        var strings = query.ToArray();
        var index = new TerminalIndex(_terminal, TerminalCoord.Empty);
        var beginIndex = index;

        lines.Clear();
        for (var y = 0; y < strings.Length; y++)
        {
            var @string = strings[y];
            index = y > 0 ? index.Linefeed() : index;
            beginIndex = index;
            index = UpdateString(index, beginIndex, lines, @string);
        }

        _index = index;
        _beginIndex = beginIndex;
    }

    public void ReverseLineFeed(int index)
    {
        for (var i = _lineList.Count - 1; i > index; i--)
        {
            var line1 = _lineList[i - 1];
            var line2 = _lineList[i];
            line1.CopyTo(line2);
        }
    }

    public TerminalCoord GetCoordinate(TerminalIndex index)
    {
        if (GetCharacterInfo(index) is { } characterInfo)
        {
            if (characterInfo.Span < 0)
            {
                return GetCoordinate(index + characterInfo.Span);
            }

            var bufferWidth = _terminal.BufferSize.Width;
            var x = index.Value % bufferWidth;
            var y = index.Value / bufferWidth;
            return new TerminalCoord(x, y);
        }
        else
        {
            var bufferWidth = _terminal.BufferSize.Width;
            var x = index.Value % bufferWidth;
            var y = index.Value / bufferWidth;
            return new TerminalCoord(x, y);
        }
    }

    public TerminalCharacterInfo? GetInfo(TerminalCoord coord)
        => GetInfo(coord.X, coord.Y);

    public TerminalCharacterInfo? GetInfo(int x, int y)
    {
        if (y < 0 || y >= Count)
        {
            return null;
        }

        var block = this[y];
        var x1 = x;
        var y1 = y;
        if (y1 < Count && this[y1] is { } line && line.Length > 0)
        {
            return line[x1];
        }

        return null;
    }

    public event EventHandler? Updated;

    private void AppendText(string text, TerminalDisplayInfo displayInfo)
    {
        var lines = this;
        var contextText = _rest + text;
        var context = new AsciiCodeContext(lines, contextText, _terminal)
        {
            Index = _index,
            BeginIndex = _beginIndex,
            DisplayInfo = displayInfo,
        };
        try
        {
            _rest = string.Empty;
            while (context.TextIndex < contextText.Length)
            {
                var character = contextText[context.TextIndex];
                if (AsciiCodeByCharacter.ContainsKey(character) is true)
                {
                    AsciiCodeByCharacter[character].Process(context);
                }
                else
                {
                    Process(lines, context);
                }
            }
        }
        catch (NotSupportedException)
        {
            _rest = contextText.Substring(context.TextIndex);
#if DEBUG && NET8_0
            Console.WriteLine($"rest: {SequenceUtility.ToLiteral(_rest)}");
#endif
        }

        _index = context.Index;
        _beginIndex = context.BeginIndex;
        lines.Prepare(_beginIndex, _index);
        lines.UpdateLines();
        InvokeTextChangedEvent();
    }

    private static void Process(TerminalLineCollection lines, AsciiCodeContext context)
    {
        var character = context.Text[context.TextIndex];
        var displayInfo = context.DisplayInfo;
        var font = context.Font;
        var span = TerminalFontUtility.GetGlyphSpan(font, character);
        var index1 = context.Index;
        var beginIndex = context.BeginIndex;
        var characterInfo = new TerminalCharacterInfo
        {
            Character = character,
            DisplayInfo = displayInfo,
            Span = span,
        };
        var index2 = index1.Expect(span);
        var line = lines.Prepare(beginIndex, index2);
        line.SetCharacterInfo(index2, characterInfo);
        context.Index = index2 + span;
        context.TextIndex++;
    }

    private void InvokeTextChangedEvent()
    {
        Updated?.Invoke(this, EventArgs.Empty);
    }

    private TerminalIndex UpdateString(TerminalIndex index, TerminalIndex beginIndex, TerminalLineCollection lines, TerminalCharacterInfo[] @string)
    {
        var font = _terminal.ActualStyle.Font;
        for (var x = 0; x < @string.Length; x++)
        {
            var item = @string[x];
            var span = TerminalFontUtility.GetGlyphSpan(font, item.Character);
            var index1 = index;
            var characterInfo = new TerminalCharacterInfo
            {
                Character = item.Character,
                DisplayInfo = item.DisplayInfo,
                Span = span,
            };
            var index2 = index1.Expect(span);
            var line = lines.Prepare(beginIndex, index2);
            line.SetCharacterInfo(index2, characterInfo);
            index = index2 + characterInfo.Span;
        }

        lines.Prepare(beginIndex, index);
        return index;
    }

    private void UpdateLines()
    {
        var query = from item in _lineList
                    where item.IsModified is true
                    select item;
        foreach (var item in query)
        {
            item.Update();
        }
    }

    private string GetString(TerminalSelection selection)
    {
        var bufferWidth = _terminal.BufferSize.Width;
        var capacity = selection.GetLength(bufferWidth);
        var sb = new StringBuilder(capacity);
        var g = 0;
        foreach (var item in selection.GetEnumerator(bufferWidth))
        {
            if (GetInfo(item) is { } characterInfo)
            {
                if (g != 0 && characterInfo.Group != g)
                {
                    sb.AppendLine();
                }

                sb.Append(characterInfo.Character);
                g = characterInfo.Group;
            }
        }

        return sb.ToString();
    }

    IEnumerator<TerminalLine> IEnumerable<TerminalLine>.GetEnumerator()
        => _lineList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _lineList.GetEnumerator();
}
