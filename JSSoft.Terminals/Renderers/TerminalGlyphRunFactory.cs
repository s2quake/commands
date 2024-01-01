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

using JSSoft.Terminals.Extensions;

namespace JSSoft.Terminals.Renderers;

sealed class TerminalGlyphRunFactory<T>(ITerminalFont font, int capacity, Func<TerminalGlyphRunInfo, T> creator) where T : TerminalGlyphRun
{
    private readonly ITerminalFont _font = font;
    private readonly Func<TerminalGlyphRunInfo, T> _creator = creator;
    private readonly List<TerminalGlyphInfo> _glyphInfoList = new(capacity);
    private readonly List<int> _spanList = new(capacity);
    private readonly List<char> _characterList = new(capacity);
    private readonly List<T> _itemList = [];

    private TerminalColor _foregroundColor;
    private bool _isBold;
    private int _group;
    private int _x = int.MaxValue;

    private static bool Predicate(char character)
    {
        if (character == 0)
            return false;
        return true;
    }

    public static T[] Create(ITerminalRow row, Func<TerminalGlyphRunInfo, T> creator)
    {
        var terminal = row.Terminal;
        var bufferWidth = terminal.BufferSize.Width;
        var font = row.Terminal.ActualStyle.Font;
        var glyphRunFactory = new TerminalGlyphRunFactory<T>(font, bufferWidth, creator);
        for (var i = 0; i < bufferWidth; i++)
        {
            var viewCoord = new TerminalCoord(i, row.Index);
            var coord = terminal.ViewToWorld(viewCoord);
            if (terminal.GetInfo(coord) is not { } characterInfo)
                break;
            if (Predicate(characterInfo.Character) == false)
                continue;
            var glyphInfo = GetGlyphInfo(font, characterInfo.Character);
            var foregroundColor = terminal.GetForegroundColor(coord);
            var isBold = characterInfo.DisplayInfo.IsBold;
            var span = characterInfo.Span;
            var backgroundRect = terminal.GetBackgroundRect(coord, span);
            glyphRunFactory.Add(glyphInfo, backgroundRect, foregroundColor, isBold, span);
        }
        return glyphRunFactory.ToArray();
    }

    public static T Create(ITerminalRow row, int x, Func<TerminalGlyphRunInfo, T> creator)
    {
        var terminal = row.Terminal;
        var font = terminal.ActualStyle.Font;
        var viewCoord = new TerminalCoord(x, row.Index);
        var coord = terminal.ViewToWorld(viewCoord);
        var characterInfo = (TerminalCharacterInfo)terminal.GetInfo(coord)!;
        var color = terminal.GetForegroundColor(coord);
        var isBold = characterInfo.DisplayInfo.IsBold;
        var glyphInfo = GetGlyphInfo(font, characterInfo.Character);
        var span = characterInfo.Span;
        var backgroundRect = terminal.GetBackgroundRect(coord, span);
        var info = new TerminalGlyphRunInfo(font)
        {
            Color = color,
            IsBold = isBold,
            Group = glyphInfo.Group,
            GlyphInfos = [glyphInfo],
            Spans = [span],
            Position = backgroundRect.Location,
        };
        return creator(info);
    }

    public void Add(TerminalGlyphInfo glyphInfo, TerminalRect backgroundRect, TerminalColor foregroundColor, bool isBold, int span)
    {
        if (_foregroundColor != foregroundColor || _isBold != isBold || _group != glyphInfo.Group)
        {
            Flush();
        }
        _glyphInfoList.Add(glyphInfo);
        _spanList.Add(span);
        _characterList.Add(glyphInfo.Character);
        _foregroundColor = foregroundColor;
        _isBold = isBold;
        _group = glyphInfo.Group;
        _x = Math.Min(_x, backgroundRect.X);
    }

    public T[] ToArray()
    {
        Flush();
        return [.. _itemList];
    }

    private void Flush()
    {
        if (_glyphInfoList.Count > 0)
        {
            var info = new TerminalGlyphRunInfo(_font)
            {
                Color = _foregroundColor,
                IsBold = _isBold,
                Group = _group,
                GlyphInfos = [.. _glyphInfoList],
                Spans = [.. _spanList],
                Position = new TerminalPoint(_x, 0)
            };
            _itemList.Add(_creator(info));
            _glyphInfoList.Clear();
            _spanList.Clear();
            _characterList.Clear();
            _x = int.MaxValue;
        }
    }

    private static TerminalGlyphInfo GetGlyphInfo(ITerminalFont font, char character)
    {
        if (font.Contains(character) == true)
        {
            return font[character];
        }
        return new()
        {
            Character = character,
        };
    }
}
