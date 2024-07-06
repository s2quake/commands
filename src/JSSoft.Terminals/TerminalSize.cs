// <copyright file="TerminalSize.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public struct TerminalSize(int width, int height) : IEquatable<TerminalSize>
{
    public override readonly bool Equals(object? obj)
    {
        if (obj is TerminalSize size)
        {
            return Width == size.Width && Height == size.Height;
        }
        return base.Equals(obj);
    }

    public int Width { get; set; } = width;

    public int Height { get; set; } = height;

    public static TerminalSize Empty { get; } = new TerminalSize();

    public override readonly int GetHashCode()
        => Width.GetHashCode() ^ Height.GetHashCode();

    public override readonly string ToString()
        => $"{Width}, {Height}";

    public static bool operator ==(TerminalSize size1, TerminalSize size2)
        => size1.Width == size2.Width && size1.Height == size2.Height;

    public static bool operator !=(TerminalSize size1, TerminalSize size2)
        => size1.Width != size2.Width || size1.Height != size2.Height;

    public static TerminalSize operator -(TerminalSize size, TerminalThickness thickness)
        => new(size.Width - thickness.Horizontal, size.Height - thickness.Vertical);

    public static TerminalSize operator +(TerminalSize size, TerminalThickness thickness)
        => new(size.Width + thickness.Horizontal, size.Height + thickness.Vertical);

    public static TerminalSize operator -(TerminalSize size1, TerminalSize size2)
        => new(size1.Width - size2.Width, size1.Height - size2.Height);

    public static TerminalSize operator +(TerminalSize size1, TerminalSize size2)
        => new(size1.Width + size2.Width, size1.Height + size2.Height);

    public static TerminalSize operator *(TerminalSize size, int value)
        => new(size.Width * value, size.Height * value);

    public static TerminalSize operator /(TerminalSize size, int value)
        => new(size.Width / value, size.Height / value);

    #region IEquatable

    readonly bool IEquatable<TerminalSize>.Equals(TerminalSize other)
        => Width == other.Width && Height == other.Height;

    #endregion
}
