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
