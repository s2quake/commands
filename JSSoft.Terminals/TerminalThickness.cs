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
    {
        return Left.GetHashCode() ^ Top.GetHashCode() ^ Right.GetHashCode() ^ Bottom.GetHashCode();
    }

    public override string ToString()
    {
        return $"{Left}, {Top}, {Right}, {Bottom}";
    }

    public int Left { readonly get; set; }

    public int Top { readonly get; set; }

    public int Right { readonly get; set; }

    public int Bottom { readonly get; set; }

    public readonly int Vertical => Top + Bottom;

    public readonly int Horizontal => Left + Right;

    public static bool operator ==(TerminalThickness pt1, TerminalThickness pt2)
    {
        return pt1.Left == pt2.Left && pt1.Top == pt2.Top && pt1.Right == pt2.Right && pt1.Bottom == pt2.Bottom;
    }

    public static bool operator !=(TerminalThickness pt1, TerminalThickness pt2)
    {
        return pt1.Left != pt2.Left || pt1.Top != pt2.Top || pt1.Right != pt2.Right || pt1.Bottom != pt2.Bottom;
    }

    public static readonly TerminalThickness Empty = new(0);

    public static implicit operator TerminalSize(TerminalThickness thickness)
    {
        return new TerminalSize(thickness.Horizontal, thickness.Vertical);
    }

    public static TerminalRect operator -(TerminalRect rect, TerminalThickness thickness)
    {
        return new TerminalRect(rect.X + thickness.Left, rect.Y + thickness.Top, rect.Width - thickness.Horizontal, rect.Height - thickness.Vertical);
    }

    public static TerminalRect operator +(TerminalRect rect, TerminalThickness thickness)
    {
        return new TerminalRect(rect.X - thickness.Left, rect.Y - thickness.Top, rect.Width + thickness.Horizontal, rect.Height + thickness.Vertical);
    }

    public static TerminalThickness operator +(TerminalThickness thickness1, TerminalThickness thickness2)
    {
        return new TerminalThickness(thickness1.Left + thickness2.Left, thickness1.Top + thickness2.Top, thickness1.Right + thickness2.Right, thickness1.Bottom + thickness2.Bottom);
    }

    public static TerminalThickness operator -(TerminalThickness thickness1, TerminalThickness thickness2)
    {
        return new TerminalThickness(thickness1.Left - thickness2.Left, thickness1.Top - thickness2.Top, thickness1.Right - thickness2.Right, thickness1.Bottom - thickness2.Bottom);
    }

    public static TerminalThickness operator *(TerminalThickness thickness, int value)
    {
        return new TerminalThickness(thickness.Left * value, thickness.Top * value, thickness.Right * value, thickness.Bottom * value);
    }

    public static TerminalThickness operator /(TerminalThickness thickness, int value)
    {
        return new TerminalThickness(thickness.Left / value, thickness.Top / value, thickness.Right / value, thickness.Bottom / value);
    }

    public static explicit operator int[](TerminalThickness thickness)
    {
        return [thickness.Left, thickness.Top, thickness.Right, thickness.Bottom];
    }

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
    {
        return Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
    }

    #endregion
}
