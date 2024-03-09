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
