// <copyright file="TerminalCoord.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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

    readonly bool IEquatable<TerminalCoord>.Equals(TerminalCoord other) => X == other.X && Y == other.Y;

    readonly int IComparable.CompareTo(object? obj)
    {
        if (obj is TerminalCoord coord)
        {
            if (this < coord)
            {
                return -1;
            }
            else if (this > coord)
            {
                return 1;
            }

            return 0;
        }
        else if (obj is null)
        {
            return 1;
        }

        throw new ArgumentException($"Object is not a {nameof(TerminalCoord)}", nameof(obj));
    }
}
