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

namespace JSSoft.Terminals.Hosting.Ansi;

sealed class AsciiCodeContext(TerminalLineCollection lines, string text, ITerminal terminal)
{
    private readonly ITerminal _terminal = terminal;
    private TerminalIndex _index;
    private TerminalRect _view = new(0, terminal.Scroll.Value, terminal.BufferSize.Width, terminal.BufferSize.Height);

    public string Title
    {
        get => _terminal.Title;
        set => _terminal.Title = value;
    }

    public string Text { get; } = text;

    public int TextIndex { get; set; }

    public TerminalLineCollection Lines { get; } = lines;

    public ITerminalFont Font => _terminal.ActualStyle.Font;

    public TerminalIndex BeginIndex { get; set; }

    public TerminalIndex Index
    {
        get => _index;
        set
        {
            if (value.X < 0 || value.Y < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            _index = value;
        }
    }

    public TerminalDisplayInfo DisplayInfo { get; set; }

    public ITerminalModes Modes => _terminal.Modes;

    public TerminalCoord OriginCoordinate
    {
        get => _terminal.OriginCoordinate;
        set => _terminal.OriginCoordinate = value;
    }

    public TerminalCoord ViewCoordinate
    {
        get => _terminal.ViewCoordinate;
        set => _terminal.ViewCoordinate = value;
    }

    public TerminalRect View => _view;

    public TerminalCoord GetCoordinate(TerminalLineCollection lines, TerminalIndex index)
    {
        if (lines.GetCharacterInfo(index) is { } characterInfo)
        {
            if (characterInfo.Span < 0)
                return GetCoordinate(lines, index + characterInfo.Span);
            var bufferWidth = _terminal.BufferSize.Width;
            var x = index.Value % bufferWidth;
            var y = index.Value / bufferWidth;
            return new TerminalCoord(x, 0 + y);
        }
        else
        {
            var bufferWidth = _terminal.BufferSize.Width;
            var x = index.Value % bufferWidth;
            var y = index.Value / bufferWidth;
            return new TerminalCoord(x, 0 + y);
        }
    }

    public void SendSequence(string sequence)
        => _terminal.WriteInput(sequence);
}
