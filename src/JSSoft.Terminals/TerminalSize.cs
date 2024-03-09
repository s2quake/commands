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
