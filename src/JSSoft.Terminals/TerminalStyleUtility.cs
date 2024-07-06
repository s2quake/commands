// <copyright file="TerminalStyleUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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
    public static readonly TerminalColor DefaultRed = TerminalColor.FromArgb(255, 153, 0, 0);
    public static readonly TerminalColor DefaultGreen = TerminalColor.FromArgb(255, 0, 166, 0);
    public static readonly TerminalColor DefaultYellow = TerminalColor.FromArgb(255, 153, 153, 0);
    public static readonly TerminalColor DefaultBlue = TerminalColor.FromArgb(255, 0, 0, 178);
    public static readonly TerminalColor DefaultMagenta = TerminalColor.FromArgb(255, 178, 0, 178);
    public static readonly TerminalColor DefaultCyan = TerminalColor.FromArgb(255, 0, 166, 178);
    public static readonly TerminalColor DefaultWhite = TerminalColor.FromArgb(255, 191, 191, 191);
    public static readonly TerminalColor DefaultBrightBlack = TerminalColor.FromArgb(255, 102, 102, 102);
    public static readonly TerminalColor DefaultBrightRed = TerminalColor.FromArgb(255, 229, 0, 0);
    public static readonly TerminalColor DefaultBrightGreen = TerminalColor.FromArgb(255, 0, 217, 0);
    public static readonly TerminalColor DefaultBrightYellow = TerminalColor.FromArgb(255, 229, 229, 0);
    public static readonly TerminalColor DefaultBrightBlue = TerminalColor.FromArgb(255, 0, 0, 255);
    public static readonly TerminalColor DefaultBrightMagenta = TerminalColor.FromArgb(255, 229, 0, 229);
    public static readonly TerminalColor DefaultBrightCyan = TerminalColor.FromArgb(255, 0, 229, 229);
    public static readonly TerminalColor DefaultBrightWhite = TerminalColor.FromArgb(255, 229, 229, 229);

    public static TerminalColor? GetColor(ITerminalStyle style, object? color)
    {
        if (color is TerminalColor terminalColor)
        {
            return terminalColor;
        }
        else if (color is TerminalColorType terminalColorType)
        {
            return GetColor(style, terminalColorType);
        }
        else if (color is null)
        {
            return null;
        }
        else
        {
            throw new NotSupportedException($"Not supported color type: {color}");
        }
    }

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
