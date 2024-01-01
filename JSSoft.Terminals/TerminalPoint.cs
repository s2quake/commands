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

public struct TerminalPoint(int x, int y) : IEquatable<TerminalPoint>
{
    public override readonly bool Equals(object? obj)
    {
        if (obj is TerminalPoint point)
        {
            return X == point.X && Y == point.Y;
        }
        return base.Equals(obj);
    }

    public override readonly int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }

    public override readonly string ToString()
    {
        return $"{X}, {Y}";
    }

    public readonly int Length()
    {
        return (int)Math.Sqrt(X * X + Y * Y);
    }

    public int X { get; set; } = x;

    public int Y { get; set; } = y;

    public readonly int Magnitude => (int)Math.Sqrt(X * X + Y * Y);

    public static TerminalPoint Empty { get; } = new TerminalPoint();

    public static bool operator ==(TerminalPoint point1, TerminalPoint point2)
    {
        return point1.X == point2.X && point1.Y == point2.Y;
    }

    public static bool operator !=(TerminalPoint point1, TerminalPoint point2)
    {
        return point1.X != point2.X || point1.Y != point2.Y;
    }

    public static TerminalPoint operator -(TerminalPoint point, TerminalSize size)
    {
        return new TerminalPoint(point.X - size.Width, point.Y - size.Height);
    }

    public static TerminalPoint operator -(TerminalPoint point)
    {
        return new TerminalPoint(-point.X, -point.Y);
    }

    public static TerminalPoint operator +(TerminalPoint point)
    {
        return new TerminalPoint(+point.X, +point.Y);
    }

    public static TerminalPoint operator +(TerminalPoint point, TerminalSize size)
    {
        return new TerminalPoint(point.X + size.Width, point.Y + size.Height);
    }

    public static TerminalPoint operator -(TerminalPoint point1, TerminalPoint point2)
    {
        return new TerminalPoint(point1.X - point2.X, point1.Y - point2.Y);
    }

    public static TerminalPoint operator +(TerminalPoint point1, TerminalPoint point2)
    {
        return new TerminalPoint(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static TerminalPoint operator *(TerminalPoint point, int value)
    {
        return new TerminalPoint(point.X * value, point.Y * value);
    }

    public static TerminalPoint operator /(TerminalPoint point, int value)
    {
        return new TerminalPoint(point.X / value, point.Y / value);
    }

    #region IEquatable

    readonly bool IEquatable<TerminalPoint>.Equals(TerminalPoint other)
    {
        return X == other.X && Y == other.Y;
    }

    #endregion
}
