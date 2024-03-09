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

public struct TerminalCoord(int x, int y) : IEquatable<TerminalCoord>, IComparable
{
    public override readonly bool Equals(object? obj)
    {
        if (obj is TerminalCoord coord)
        {
            return X == coord.X && Y == coord.Y;
        }
        return base.Equals(obj);
    }

    public override readonly int GetHashCode() => X ^ Y;

    public override readonly string ToString() => $"{X}, {Y}";

    public int X { readonly get; set; } = x;

    public int Y { readonly get; set; } = y;

    public static bool operator >(TerminalCoord coord1, TerminalCoord coord2)
        => coord1.Y > coord2.Y || (coord1.Y == coord2.Y && coord1.X > coord2.X);

    public static bool operator >=(TerminalCoord coord1, TerminalCoord coord2)
        => coord1 > coord2 || coord1 == coord2;

    public static bool operator <(TerminalCoord coord1, TerminalCoord coord2)
        => coord1.Y < coord2.Y || (coord1.Y == coord2.Y && coord1.X < coord2.X);

    public static bool operator <=(TerminalCoord coord1, TerminalCoord coord2)
        => coord1 < coord2 || coord1 == coord2;

    public static bool operator ==(TerminalCoord coord1, TerminalCoord coord2)
        => coord1.Y == coord2.Y && coord1.X == coord2.X;

    public static bool operator !=(TerminalCoord coord1, TerminalCoord coord2)
        => coord1.Y != coord2.Y || coord1.X != coord2.X;

    public static TerminalCoord operator +(TerminalCoord coord1, TerminalCoord coord2)
        => new(coord1.X + coord2.X, coord1.Y + coord2.Y);

    public static TerminalCoord operator -(TerminalCoord coord1, TerminalCoord coord2)
        => new(coord1.X - coord2.X, coord1.Y - coord2.Y);

    public static readonly TerminalCoord Empty = new(0, 0);

    public static readonly TerminalCoord Invalid = new(-1, -1);

    internal readonly string CursorString => $"\u001b[{Y + 1};{X + 1}f";

    #region IEquatable

    readonly bool IEquatable<TerminalCoord>.Equals(TerminalCoord other) => X == other.X && Y == other.Y;

    #endregion

    #region IComparable

    readonly int IComparable.CompareTo(object? obj)
    {
        if (obj is TerminalCoord coord)
        {
            if (this < coord)
                return -1;
            else if (this > coord)
                return 1;
            return 0;
        }
        else if (obj is null)
        {
            return 1;
        }
        throw new ArgumentException($"Object is not a {nameof(TerminalCoord)}", nameof(obj));
    }

    #endregion
}
