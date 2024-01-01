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

namespace JSSoft.Terminals;

public static class TerminalStyleUtility
{
    public static readonly TerminalColor DefaultForegroundColor = TerminalColor.FromArgb(255, 255, 255, 255);
    public static readonly TerminalColor DefaultBackgroundColor = TerminalColor.FromArgb(240, 23, 23, 23);
    public static readonly TerminalColor DefaultSelectionForegroundColor = TerminalColor.FromArgb(255, 255, 255, 255);
    public static readonly TerminalColor DefaultSelectionBackgroundColor = TerminalColor.FromArgb(255, 49, 79, 129);
    public static readonly TerminalColorSource DefaultSelectionForegroundColorSource = TerminalColorSource.NotUsed;
    public static readonly TerminalColorSource DefaultSelectionBackgroundColorSource = TerminalColorSource.Origin;
    public static readonly TerminalColor DefaultCursorForegroundColor = TerminalColor.FromArgb(255, 255, 255, 255);
    public static readonly TerminalColor DefaultCursorBackgroundColor = TerminalColor.FromArgb(255, 139, 139, 139);
    public static readonly TerminalColorSource DefaultCursorForegroundColorSource = TerminalColorSource.NotUsed;
    public static readonly TerminalColorSource DefaultCursorBackgroundColorSource = TerminalColorSource.Origin;

    public static readonly TerminalCursorShape DefaultCursorShape = TerminalCursorShape.Block;
    public static readonly TerminalCursorVisibility DefaultCursorVisibility = TerminalCursorVisibility.Always;
    public static readonly int DefaultCursorThickness = 1;
    public static readonly bool DefaultIsCursorBlinkable = false;
    public static readonly double DefaultCursorBlinkDelay = 0.5;
    public static readonly bool DefaultIsScrollForwardEnabled = false;

    public static readonly TerminalColor DefaultBlack = TerminalColor.FromArgb(255, 0, 0, 0);
    public static readonly TerminalColor DefaultRed = TerminalColor.FromArgb(255, 128, 0, 0);
    public static readonly TerminalColor DefaultGreen = TerminalColor.FromArgb(255, 0, 128, 0);
    public static readonly TerminalColor DefaultYellow = TerminalColor.FromArgb(255, 128, 128, 0);
    public static readonly TerminalColor DefaultBlue = TerminalColor.FromArgb(255, 0, 0, 128);
    public static readonly TerminalColor DefaultMagenta = TerminalColor.FromArgb(255, 128, 0, 128);
    public static readonly TerminalColor DefaultCyan = TerminalColor.FromArgb(255, 0, 128, 128);
    public static readonly TerminalColor DefaultWhite = TerminalColor.FromArgb(255, 128, 128, 128);
    public static readonly TerminalColor DefaultBrightBlack = TerminalColor.FromArgb(255, 128, 128, 128);
    public static readonly TerminalColor DefaultBrightRed = TerminalColor.FromArgb(255, 255, 0, 0);
    public static readonly TerminalColor DefaultBrightGreen = TerminalColor.FromArgb(255, 0, 255, 0);
    public static readonly TerminalColor DefaultBrightYellow = TerminalColor.FromArgb(255, 255, 255, 0);
    public static readonly TerminalColor DefaultBrightBlue = TerminalColor.FromArgb(255, 0, 0, 255);
    public static readonly TerminalColor DefaultBrightMagenta = TerminalColor.FromArgb(255, 255, 0, 255);
    public static readonly TerminalColor DefaultBrightCyan = TerminalColor.FromArgb(255, 0, 255, 255);
    public static readonly TerminalColor DefaultBrightWhite = TerminalColor.FromArgb(255, 255, 255, 255);

    public static TerminalColor? GetColor(ITerminalStyle style, TerminalColorType? color)
    {
        return color is { } value ? GetColor(style, value) : null;
    }

    public static TerminalColor GetColor(ITerminalStyle style, TerminalColorType color)
    {
        return color switch
        {
            TerminalColorType.Black => style.Black,
            TerminalColorType.Blue => style.Blue,
            TerminalColorType.Green => style.Green,
            TerminalColorType.Cyan => style.Cyan,
            TerminalColorType.Red => style.Red,
            TerminalColorType.Magenta => style.Magenta,
            TerminalColorType.Yellow => style.Yellow,
            TerminalColorType.BrightBlack => style.BrightBlack,
            TerminalColorType.White => style.White,
            TerminalColorType.BrightBlue => style.BrightBlue,
            TerminalColorType.BrightGreen => style.BrightGreen,
            TerminalColorType.BrightCyan => style.BrightCyan,
            TerminalColorType.BrightRed => style.BrightRed,
            TerminalColorType.BrightMagenta => style.BrightMagenta,
            TerminalColorType.BrightYellow => style.BrightYellow,
            TerminalColorType.BrightWhite => style.BrightWhite,
            _ => throw new NotImplementedException(),
        };
    }
}
