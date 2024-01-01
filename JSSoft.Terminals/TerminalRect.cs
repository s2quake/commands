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

public struct TerminalRect(TerminalPoint location, TerminalSize size) : IEquatable<TerminalRect>
{
    private TerminalPoint _location = location;
    private TerminalSize _size = size;

    public TerminalRect(int x, int y, int width, int height)
        : this(new TerminalPoint(x, y), new TerminalSize(width, height))
    {
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj is TerminalRect rect)
        {
            return _location == rect._location && _size == rect._size;
        }
        return base.Equals(obj);
    }

    public override readonly int GetHashCode()
    {
        return _location.GetHashCode() ^ _size.GetHashCode();
    }

    public override readonly string ToString()
    {
        return $"{_location}, {_size}";
    }

    public static TerminalRect FromLTRB(int left, int top, int right, int bottom)
    {
        return new TerminalRect(left, top, right - left, bottom - top);
    }

    public static bool Contains(TerminalRect outerRect, TerminalRect innerRect, TerminalPoint point)
    {
        if (outerRect.Left >= innerRect.Left)
            throw new ArgumentOutOfRangeException(nameof(outerRect));
        if (outerRect.Top >= innerRect.Top)
            throw new ArgumentOutOfRangeException(nameof(outerRect));
        if (outerRect.Right < innerRect.Right)
            throw new ArgumentOutOfRangeException(nameof(outerRect));
        if (outerRect.Bottom < innerRect.Bottom)
            throw new ArgumentOutOfRangeException(nameof(outerRect));
        return outerRect.Contains(point) == true && innerRect.Contains(point) == false;
    }

    public static bool Contains(TerminalRect rect, TerminalThickness margin, TerminalThickness padding, TerminalPoint point)
    {
        return Contains(rect + margin, rect - padding, point);
    }

    public readonly bool Contains(TerminalPoint point)
    {
        return point.X >= Left && point.Y >= Top && point.X < Right && point.Y < Bottom;
    }

    public readonly bool Contains(TerminalRect rect)
    {
        return rect.Left >= Left && rect.Top >= Top && rect.Right <= Right && rect.Bottom <= Bottom;
    }

    public readonly bool IntersectsWith(TerminalRect rect)
    {
        return rect.Left < Right && rect.Right > Left && rect.Top < Bottom && rect.Bottom > Top;
    }

    public readonly TerminalRect Intersect(TerminalRect rect)
    {
        var left = Math.Max(Left, rect.Left);
        var top = Math.Max(Top, rect.Top);
        var right = Math.Min(Right, rect.Right);
        var bottom = Math.Min(Bottom, rect.Bottom);
        return FromLTRB(left, top, right, bottom);
    }

    public readonly TerminalRect Union(TerminalRect rect)
    {
        var left = Math.Min(Left, rect.Left);
        var top = Math.Min(Top, rect.Top);
        var right = Math.Max(Right, rect.Right);
        var bottom = Math.Max(Bottom, rect.Bottom);
        return FromLTRB(left, top, right, bottom);
    }

    public readonly TerminalPoint GetCenter()
    {
        return new TerminalPoint(X + Width / 2, Y + Height / 2);
    }

    public readonly TerminalRect PutIn(TerminalRect targetRect)
    {
        var rect = this;
        if (rect.Left < targetRect.Left)
        {
            rect.X = targetRect.Left;
        }
        if (rect.Top < targetRect.Top)
        {
            rect.Y = targetRect.Top;
        }
        if (rect.Right >= targetRect.Right)
        {
            rect.X = targetRect.Right - rect.Width;
        }
        if (rect.Bottom >= targetRect.Bottom)
        {
            rect.Y = targetRect.Bottom - rect.Height;
        }
        return rect;
    }

    public readonly TerminalRect Transform(TerminalPoint point)
    {
        return new TerminalRect(Location + point, Size);
    }

    public TerminalPoint Location
    {
        readonly get => _location;
        set => _location = value;
    }

    public TerminalSize Size
    {
        readonly get => _size;
        set => _size = value;
    }

    public readonly int Left => _location.X;

    public readonly int Right => _location.X + _size.Width;

    public readonly int Top => _location.Y;

    public readonly int Bottom => _location.Y + _size.Height;

    public int X
    {
        readonly get => _location.X;
        set => _location.X = value;
    }

    public int Y
    {
        readonly get => _location.Y;
        set => _location.Y = value;
    }

    public int Width
    {
        readonly get => _size.Width;
        set => _size.Width = value;
    }

    public int Height
    {
        readonly get => _size.Height;
        set => _size.Height = value;
    }

    public static TerminalRect Empty { get; } = new TerminalRect();

    public static bool operator ==(TerminalRect rect1, TerminalRect rect2)
    {
        return rect1.Location == rect2.Location && rect1.Size == rect2.Size;
    }

    public static bool operator !=(TerminalRect rect1, TerminalRect rect2)
    {
        return rect1.Location != rect2.Location || rect1.Size != rect2.Size;
    }

    public static TerminalRect operator -(TerminalRect rect, TerminalSize size)
    {
        return new TerminalRect(rect.Location, rect.Size - size);
    }

    public static TerminalRect operator +(TerminalRect rect, TerminalSize size)
    {
        return new TerminalRect(rect.Location, rect.Size + size);
    }

    public static TerminalRect operator -(TerminalRect rect, TerminalPoint point)
    {
        return new TerminalRect(rect.Location - point, rect.Size);
    }

    public static TerminalRect operator +(TerminalRect rect, TerminalPoint point)
    {
        return new TerminalRect(rect.Location + point, rect.Size);
    }

    public static TerminalRect operator *(TerminalRect rect, int value)
    {
        return new TerminalRect(rect.Location.X * value, rect.Location.Y * value, rect.Size.Width * value, rect.Size.Height * value);
    }

    #region IEquatable

    readonly bool IEquatable<TerminalRect>.Equals(TerminalRect other)
    {
        return Location == other.Location && Size == other.Size;
    }

    #endregion
}
