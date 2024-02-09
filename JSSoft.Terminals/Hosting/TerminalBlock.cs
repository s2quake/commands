// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using JSSoft.Terminals.Hosting.Ansi;

namespace JSSoft.Terminals.Hosting;

sealed class TerminalBlock(Terminal terminal)
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
    private readonly Terminal _terminal = terminal;
    public TerminalIndex _index = new(terminal, TerminalCoord.Empty);
    private TerminalIndex _beginIndex = new(terminal, TerminalCoord.Empty);
    private string _command = string.Empty;

    public int Height
    {
        get
        {
            if (Lines.Count > 0)
            {
                if (Lines.Last().Length == 0)
                    return Lines.Count - 1;
                return Lines.Count;
            }
            return 0;
        }
    }

    public int Top { get; private set; }

    public int Bottom => Top + Height;

    public int Index { get; set; } = -1;

    public TerminalLineCollection Lines { get; } = new(terminal);

    public string Command
    {
        get => _command;
        set
        {
            _command = value;
            UpdateCommand();
            InvokeTextChangedEvent();
        }
    }

    public void Append(string text, TerminalDisplayInfo displayInfo)
    {
        AppendText(text, displayInfo);
    }

    private void UpdateCommand()
    {
        // if (_command == string.Empty)
        //     return;

        var command = _command + char.MinValue;
        var index = _index;
        var beginIndex = _beginIndex;
        var lines = Lines;
        // lines.Take(_index);
        for (var i = 0; i < command.Length; i++)
        {
            var character = command[i];
            var font = _terminal.ActualStyle.Font;
            var span = TerminalFontUtility.GetGlyphSpan(font, character);
            var index1 = index;
            var characterInfo = new TerminalCharacterInfo
            {
                Character = character,
                DisplayInfo = TerminalDisplayInfo.Empty,
                Span = span,
            };
            var index2 = index1.Expect(span);
            var line = lines.Prepare(beginIndex, index2);
            line.SetCharacterInfo(index2, characterInfo);
            index = index2 + span;
        }
        lines.Update();
    }

    public TerminalCoord GetCoordinate(TerminalIndex index)
    {
        if (Lines.GetCharacterInfo(index) is { } characterInfo)
        {
            if (characterInfo.Span < 0)
                return GetCoordinate(index + characterInfo.Span);
            var bufferWidth = _terminal.BufferSize.Width;
            var x = index.Value % bufferWidth;
            var y = index.Value / bufferWidth;
            return new TerminalCoord(x, Top + y);
        }
        return TerminalCoord.Empty;
    }

    public void Clear()
    {
        _index = new TerminalIndex(_terminal, TerminalCoord.Empty);
        _beginIndex = _index;
        Lines.Clear();
        AppendText(string.Empty, TerminalDisplayInfo.Empty);
    }

    public void Take(int count)
    {
        var index = new TerminalIndex(_terminal, count);
        _index = index;
        _beginIndex = index.MoveToFirstOfString(_terminal);
        Lines.Take(index);
    }

    public bool TryTransform(int y)
    {
        if (y != Top)
        {
            Transform(y);
            return true;
        }
        return false;
    }

    public void Transform(int y)
    {
        Top = y;
    }

    public void Update()
    {
        Lines.Take(_index);
        var lines = Lines;
        var query = from item in Lines
                    where item.Parent == null
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
        UpdateCommand();
        // lines.Take(_index.Linefeed());
    }

    public void Take()
    {
        Lines.Take(_index.Linefeed());
    }

    public event EventHandler? TextChanged;

    private void AppendText(string text, TerminalDisplayInfo displayInfo)
    {
        var lines = Lines;
        var context = new AsciiCodeContext(text, _terminal)
        {
            Index = _index,
            BeginIndex = _beginIndex,
            DisplayInfo = displayInfo,
        };
        while (context.TextIndex < text.Length)
        {
            var character = text[context.TextIndex];
            if (AsciiCodeByCharacter.ContainsKey(character) == true)
            {
                AsciiCodeByCharacter[character].Process(lines, context);
            }
            else
            {
                Process(lines, context);
            }
        }
        _index = context.Index;
        _beginIndex = context.BeginIndex;

        UpdateCommand();

        lines.Update();
        InvokeTextChangedEvent();
    }

    // private void SetText(string text, TerminalDisplayInfo displayInfo)
    // {
    //     var lines = Lines;
    //     var index = new TerminalIndex(_terminal, TerminalCoord.Empty);
    //     var context = new AsciiCodeContext(text, _terminal)
    //     {
    //         Index = new TerminalIndex(0, lines.Count, _terminal.BufferSize.Width),
    //         BeginIndex = new TerminalIndex(0, lines.Count, _terminal.BufferSize.Width),
    //         DisplayInfo = displayInfo,
    //     };
    //     lines.Clear();
    //     while (context.TextIndex < text.Length)
    //     {
    //         var character = text[context.TextIndex];
    //         if (AsciiCodeByCharacter.ContainsKey(character) == true)
    //         {
    //             AsciiCodeByCharacter[character].Process(lines, context);
    //         }
    //         else
    //         {
    //             Process(lines, context);
    //         }
    //     }
    //     _index = context.Index;
    //     _beginIndex = context.BeginIndex;
    //     InvokeTextChangedEvent();
    // }

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
        TextChanged?.Invoke(this, EventArgs.Empty);
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
}
