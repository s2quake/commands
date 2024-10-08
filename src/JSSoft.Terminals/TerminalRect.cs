// <copyright file="TerminalRect.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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
        => _location.GetHashCode() ^ _size.GetHashCode();

    public override readonly string ToString()
        => $"{_location}, {_size}";

    public static TerminalRect FromLTRB(int left, int top, int right, int bottom)
        => new(left, top, right - left, bottom - top);

    public static bool Contains(TerminalRect outerRect, TerminalRect innerRect, TerminalPoint point)
    {
        if (outerRect.Left >= innerRect.Left)
        {
            throw new ArgumentOutOfRangeException(nameof(outerRect));
        }

        if (outerRect.Top >= innerRect.Top)
        {
            throw new ArgumentOutOfRangeException(nameof(outerRect));
        }

        if (outerRect.Right < innerRect.Right)
        {
            throw new ArgumentOutOfRangeException(nameof(outerRect));
        }

        if (outerRect.Bottom < innerRect.Bottom)
        {
            throw new ArgumentOutOfRangeException(nameof(outerRect));
        }

        return outerRect.Contains(point) is true && innerRect.Contains(point) is false;
    }

    public static bool Contains(TerminalRect rect, TerminalThickness margin, TerminalThickness padding, TerminalPoint point)
        => Contains(rect + margin, rect - padding, point);

    public readonly bool Contains(TerminalPoint point)
        => point.X >= Left && point.Y >= Top && point.X < Right && point.Y < Bottom;

    public readonly bool Contains(TerminalRect rect)
        => rect.Left >= Left && rect.Top >= Top && rect.Right <= Right && rect.Bottom <= Bottom;

    public readonly bool IntersectsWith(TerminalRect rect)
        => rect.Left < Right && rect.Right > Left && rect.Top < Bottom && rect.Bottom > Top;

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
        => new(X + Width / 2, Y + Height / 2);

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
        => new(Location + point, Size);

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
        => rect1.Location == rect2.Location && rect1.Size == rect2.Size;

    public static bool operator !=(TerminalRect rect1, TerminalRect rect2)
        => rect1.Location != rect2.Location || rect1.Size != rect2.Size;

    public static TerminalRect operator -(TerminalRect rect, TerminalSize size)
        => new(rect.Location, rect.Size - size);

    public static TerminalRect operator +(TerminalRect rect, TerminalSize size)
        => new(rect.Location, rect.Size + size);

    public static TerminalRect operator -(TerminalRect rect, TerminalPoint point)
        => new(rect.Location - point, rect.Size);

    public static TerminalRect operator +(TerminalRect rect, TerminalPoint point)
        => new(rect.Location + point, rect.Size);

    public static TerminalRect operator *(TerminalRect rect, int value)
        => new(rect.Location.X * value, rect.Location.Y * value, rect.Size.Width * value, rect.Size.Height * value);

    readonly bool IEquatable<TerminalRect>.Equals(TerminalRect other)
        => Location == other.Location && Size == other.Size;
}
