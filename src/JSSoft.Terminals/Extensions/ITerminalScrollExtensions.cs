// <copyright file="ITerminalScrollExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Extensions;

public static class ITerminalScrollExtensions
{
    public static void ScrollToTop(this ITerminalScroll @this)
    {
        @this.Value = @this.Minimum;
    }

    public static void ScrollToBottom(this ITerminalScroll @this)
    {
        @this.Value = @this.Maximum;
    }

    public static void PageUp(this ITerminalScroll @this)
    {
        ScrollValue(@this, -@this.LargeChange);
    }

    public static void PageDown(this ITerminalScroll @this)
    {
        ScrollValue(@this, @this.LargeChange);
    }

    public static void LineUp(this ITerminalScroll @this)
    {
        ScrollValue(@this, -1);
    }

    public static void LineDown(this ITerminalScroll @this)
    {
        ScrollValue(@this, 1);
    }

    public static int CoerceValue(this ITerminalScroll @this, int value)
    {
        return TerminalMathUtility.Clamp(value, @this.Minimum, @this.Maximum);
    }

    private static void ScrollValue(ITerminalScroll @this, int offset)
    {
        @this.Value = CoerceValue(@this, @this.Value + offset);
    }
}
