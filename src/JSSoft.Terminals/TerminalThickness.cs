// <copyright file="TerminalThickness.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public struct TerminalThickness : IEquatable<TerminalThickness>
{
    public TerminalThickness(int length)
    {
        Left = length;
        Top = length;
        Right = length;
        Bottom = length;
    }

    public TerminalThickness(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj is TerminalThickness thickness)
        {
            return Left == thickness.Left && Top == thickness.Top && Right == thickness.Right && Bottom == thickness.Bottom;
        }
        return base.Equals(obj);
    }

    public override readonly int GetHashCode()
        => Left.GetHashCode() ^ Top.GetHashCode() ^ Right.GetHashCode() ^ Bottom.GetHashCode();

    public override readonly string ToString()
        => $"{Left}, {Top}, {Right}, {Bottom}";

    public int Left { readonly get; set; }

    public int Top { readonly get; set; }

    public int Right { readonly get; set; }

    public int Bottom { readonly get; set; }

    public readonly int Vertical => Top + Bottom;

    public readonly int Horizontal => Left + Right;

    public static bool operator ==(TerminalThickness pt1, TerminalThickness pt2)
        => pt1.Left == pt2.Left && pt1.Top == pt2.Top && pt1.Right == pt2.Right && pt1.Bottom == pt2.Bottom;

    public static bool operator !=(TerminalThickness pt1, TerminalThickness pt2)
        => pt1.Left != pt2.Left || pt1.Top != pt2.Top || pt1.Right != pt2.Right || pt1.Bottom != pt2.Bottom;

    public static readonly TerminalThickness Empty = new(0);

    public static implicit operator TerminalSize(TerminalThickness thickness)
        => new(thickness.Horizontal, thickness.Vertical);

    public static TerminalRect operator -(TerminalRect rect, TerminalThickness thickness)
        => new(rect.X + thickness.Left, rect.Y + thickness.Top, rect.Width - thickness.Horizontal, rect.Height - thickness.Vertical);

    public static TerminalRect operator +(TerminalRect rect, TerminalThickness thickness)
        => new(rect.X - thickness.Left, rect.Y - thickness.Top, rect.Width + thickness.Horizontal, rect.Height + thickness.Vertical);

    public static TerminalThickness operator +(TerminalThickness thickness1, TerminalThickness thickness2)
        => new(thickness1.Left + thickness2.Left, thickness1.Top + thickness2.Top, thickness1.Right + thickness2.Right, thickness1.Bottom + thickness2.Bottom);

    public static TerminalThickness operator -(TerminalThickness thickness1, TerminalThickness thickness2)
        => new(thickness1.Left - thickness2.Left, thickness1.Top - thickness2.Top, thickness1.Right - thickness2.Right, thickness1.Bottom - thickness2.Bottom);

    public static TerminalThickness operator *(TerminalThickness thickness, int value)
        => new(thickness.Left * value, thickness.Top * value, thickness.Right * value, thickness.Bottom * value);

    public static TerminalThickness operator /(TerminalThickness thickness, int value)
        => new(thickness.Left / value, thickness.Top / value, thickness.Right / value, thickness.Bottom / value);

    public static explicit operator int[](TerminalThickness thickness)
        => [thickness.Left, thickness.Top, thickness.Right, thickness.Bottom];

    public static explicit operator TerminalThickness(int[] values)
    {
        if (values.Length != 4)
            throw new ArgumentException("The array must be 4 in length.", nameof(values));
        return new TerminalThickness(values[0], values[1], values[2], values[3]);
    }

    public TerminalThickness(int[] values)
    {
        Left = values[0];
        Top = values[1];
        Right = values[2];
        Bottom = values[3];
    }

    #region IEquatable

    readonly bool IEquatable<TerminalThickness>.Equals(TerminalThickness other)
        => Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;

    #endregion
}
