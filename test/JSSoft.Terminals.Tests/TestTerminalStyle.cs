// <copyright file="TestTerminalStyle.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Terminals.Tests;

public sealed class TestTerminalStyle : ITerminalStyle
{
    public ITerminalFont Font => TestTerminalFont.Default;

    public TerminalColor ForegroundColor => TerminalStyleUtility.DefaultForegroundColor;

    public TerminalColor BackgroundColor => TerminalStyleUtility.DefaultBackgroundColor;

    public TerminalColor SelectionForegroundColor => TerminalStyleUtility.DefaultSelectionForegroundColor;

    public TerminalColor SelectionBackgroundColor => TerminalStyleUtility.DefaultSelectionBackgroundColor;

    public TerminalColorSource SelectionForegroundColorSource => default;

    public TerminalColorSource SelectionBackgroundColorSource => default;

    public TerminalColor CursorForegroundColor => TerminalStyleUtility.DefaultCursorForegroundColor;

    public TerminalColor CursorBackgroundColor => TerminalStyleUtility.DefaultCursorBackgroundColor;

    public TerminalColorSource CursorForegroundColorSource => default;

    public TerminalColorSource CursorBackgroundColorSource => default;

    public TerminalCursorShape CursorShape => default;

    public TerminalCursorVisibility CursorVisibility => default;

    public int CursorThickness => default;

    public bool IsCursorBlinkable => default;

    public double CursorBlinkDelay => default;

    public bool IsScrollForwardEnabled => default;

    public TerminalColor Black => TerminalStyleUtility.DefaultBlack;

    public TerminalColor Red => TerminalStyleUtility.DefaultRed;

    public TerminalColor Green => TerminalStyleUtility.DefaultGreen;

    public TerminalColor Yellow => TerminalStyleUtility.DefaultYellow;

    public TerminalColor Blue => TerminalStyleUtility.DefaultBlue;

    public TerminalColor Magenta => TerminalStyleUtility.DefaultMagenta;

    public TerminalColor Cyan => TerminalStyleUtility.DefaultCyan;

    public TerminalColor White => TerminalStyleUtility.DefaultWhite;

    public TerminalColor BrightBlack => TerminalStyleUtility.DefaultBrightBlack;

    public TerminalColor BrightRed => TerminalStyleUtility.DefaultBrightRed;

    public TerminalColor BrightGreen => TerminalStyleUtility.DefaultBrightGreen;

    public TerminalColor BrightYellow => TerminalStyleUtility.DefaultBrightYellow;

    public TerminalColor BrightBlue => TerminalStyleUtility.DefaultBrightBlue;

    public TerminalColor BrightMagenta => TerminalStyleUtility.DefaultBrightMagenta;

    public TerminalColor BrightCyan => TerminalStyleUtility.DefaultBrightCyan;

    public TerminalColor BrightWhite => TerminalStyleUtility.DefaultBrightWhite;

    public static TestTerminalStyle Default { get; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    public void InvokePropertyChangedEvent(PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }
}
