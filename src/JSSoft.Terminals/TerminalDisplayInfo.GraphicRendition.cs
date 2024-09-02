// <copyright file="TerminalDisplayInfo.GraphicRendition.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

// https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences
public partial struct TerminalDisplayInfo
{
    private delegate void GraphicRenditionSetter(ref TerminalDisplayInfo o, ref Span<int> codes);

    private static readonly Dictionary<int, GraphicRenditionSetter> actionByCode = new()
    {
        { 0, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Reset() },
        { 1, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.IsBold = true },
        { 3, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.IsItalic = true}, // italic
        { 22, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.IsBold = false },
        { 23, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.IsItalic = false },
        { 4, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.IsUnderline = true },
        { 24, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.IsUnderline = false },
        { 7, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.IsInverse = true },
        { 27, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.IsInverse = false },
        { 30, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.Black },
        { 31, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.Red },
        { 32, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.Green },
        { 33, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.Yellow },
        { 34, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.Blue },
        { 35, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.Magenta },
        { 36, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.Cyan },
        { 37, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.White },
        { 38, SetForegroundUsingIndexedColor },
        { 39, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = null },
        { 40, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.Black },
        { 41, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.Red },
        { 42, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.Green },
        { 43, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.Yellow },
        { 44, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.Blue },
        { 45, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.Magenta },
        { 46, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.Cyan },
        { 47, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.White },
        { 48, SetBackgroundUsingIndexedColor },
        { 49, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = null },
        { 90, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.BrightBlack },
        { 91, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.BrightRed },
        { 92, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.BrightGreen },
        { 93, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.BrightYellow },
        { 94, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.BrightBlue },
        { 95, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.BrightMagenta },
        { 96, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.BrightCyan },
        { 97, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Foreground = TerminalColorType.BrightWhite },
        { 100, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.BrightBlack },
        { 101, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.BrightRed },
        { 102, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.BrightGreen },
        { 103, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.BrightYellow },
        { 104, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.BrightBlue },
        { 105, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.BrightMagenta },
        { 106, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.BrightCyan },
        { 107, (ref TerminalDisplayInfo o, ref Span<int> codes) => o.Background = TerminalColorType.BrightWhite },
    };

    public void Reset()
    {
        Foreground = null;
        Background = null;
        IsBold = false;
        IsItalic = false;
        IsUnderline = false;
        IsInverse = false;
    }

    public void SetGraphicRendition(int[] codes)
    {
        var span = new Span<int>(codes);
        while (span.Length > 0)
        {
            var code = span[0];
            span = span[1..];
            if (actionByCode.TryGetValue(code, out var action) is true)
            {
                action.Invoke(ref this, ref span);
            }
            else
            {
                Console.WriteLine($"'CSI Pm m' not supported code: {code}");
            }
        }
    }

    // Ps = 3 8 : 2 : Pi : Pr : Pg : Pb
    // Ps = 3 8 : 5 : Ps
    // Ps = 3 8 ; 2 ; Pr ; Pg ; Pb
    private static void SetForegroundUsingIndexedColor(ref TerminalDisplayInfo o, ref Span<int> codes)
    {
        // Ps = 3 8 : 5 : Ps
        if (codes[0] == 5)
        {
            if (codes.Length == 2)
            {
                o.Foreground = TerminalColors.IndexedColors[codes[1]];
                codes = codes[2..];
            }
            else
            {
                Console.WriteLine("Invalid color code");
            }
        }
        else if (codes[0] == 2)
        {
            if (codes.Length == 4)
            {
                o.Foreground = TerminalColor.FromArgb((byte)codes[1], (byte)codes[2], (byte)codes[3], (byte)codes[4]);
                codes = codes[4..];
            }
            else
            {
                Console.WriteLine("Invalid color code");
            }
        }
        else
        {
            Console.WriteLine("Invalid color code");
        }
    }

    private static void SetBackgroundUsingIndexedColor(ref TerminalDisplayInfo o, ref Span<int> codes)
    {
        // Ps = 4 8 : 5 : Ps
        if (codes[0] == 5)
        {
            if (codes.Length == 2)
            {
                o.Background = TerminalColors.IndexedColors[codes[1]];
                codes = codes[2..];
            }
            else
            {
                Console.WriteLine("Invalid color code");
            }
        }
        else if (codes[0] == 2)
        {
            if (codes.Length == 4)
            {
                o.Background = TerminalColor.FromArgb((byte)codes[1], (byte)codes[2], (byte)codes[3], (byte)codes[4]);
                codes = codes[4..];
            }
            else
            {
                Console.WriteLine("Invalid color code");
            }
        }
        else
        {
            Console.WriteLine("Invalid color code");
        }
    }
}
