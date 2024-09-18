// <copyright file="AsciiCodeContext.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi;

internal sealed class AsciiCodeContext(TerminalLineCollection lines, string text, ITerminal terminal)
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
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

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
            {
                return GetCoordinate(lines, index + characterInfo.Span);
            }

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
