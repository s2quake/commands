// <copyright file="ITerminalStyle.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Terminals;

public interface ITerminalStyle : INotifyPropertyChanged
{
    ITerminalFont Font { get; }

    TerminalColor ForegroundColor { get; }

    TerminalColor BackgroundColor { get; }

    TerminalColor SelectionForegroundColor { get; }

    TerminalColor SelectionBackgroundColor { get; }

    TerminalColorSource SelectionForegroundColorSource { get; }

    TerminalColorSource SelectionBackgroundColorSource { get; }

    TerminalColor CursorForegroundColor { get; }

    TerminalColor CursorBackgroundColor { get; }

    TerminalColorSource CursorForegroundColorSource { get; }

    TerminalColorSource CursorBackgroundColorSource { get; }

    TerminalCursorShape CursorShape { get; }

    TerminalCursorVisibility CursorVisibility { get; }

    int CursorThickness { get; }

    bool IsCursorBlinkable { get; }

    double CursorBlinkDelay { get; }

    bool IsScrollForwardEnabled { get; }

    public TerminalColor Black { get; }

    public TerminalColor Red { get; }

    public TerminalColor Green { get; }

    public TerminalColor Yellow { get; }

    public TerminalColor Blue { get; }

    public TerminalColor Magenta { get; }

    public TerminalColor Cyan { get; }

    public TerminalColor White { get; }

    public TerminalColor BrightBlack { get; }

    public TerminalColor BrightRed { get; }

    public TerminalColor BrightGreen { get; }

    public TerminalColor BrightYellow { get; }

    public TerminalColor BrightBlue { get; }

    public TerminalColor BrightMagenta { get; }

    public TerminalColor BrightCyan { get; }

    public TerminalColor BrightWhite { get; }
}
