// <copyright file="TerminalPoint.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public struct TerminalPoint(int x, int y) : IEquatable<TerminalPoint>
{
    public int X { get; set; } = x;

    public int Y { get; set; } = y;

    public readonly int Magnitude => (int)Math.Sqrt(X * X + Y * Y);

    public static TerminalPoint Empty { get; } = new TerminalPoint();

    public override readonly bool Equals(object? obj)
    {
        if (obj is TerminalPoint point)
        {
            return X == point.X && Y == point.Y;
        }
        return base.Equals(obj);
    }

    public override readonly int GetHashCode()
        => X.GetHashCode() ^ Y.GetHashCode();

    public override readonly string ToString()
        => $"{X}, {Y}";

    public readonly int Length()
        => (int)Math.Sqrt(X * X + Y * Y);

    public static bool operator ==(TerminalPoint point1, TerminalPoint point2)
        => point1.X == point2.X && point1.Y == point2.Y;

    public static bool operator !=(TerminalPoint point1, TerminalPoint point2)
        => point1.X != point2.X || point1.Y != point2.Y;

    public static TerminalPoint operator -(TerminalPoint point, TerminalSize size)
        => new(point.X - size.Width, point.Y - size.Height);

    public static TerminalPoint operator -(TerminalPoint point)
        => new(-point.X, -point.Y);

    public static TerminalPoint operator +(TerminalPoint point)
        => new(+point.X, +point.Y);

    public static TerminalPoint operator +(TerminalPoint point, TerminalSize size)
        => new(point.X + size.Width, point.Y + size.Height);

    public static TerminalPoint operator -(TerminalPoint point1, TerminalPoint point2)
        => new(point1.X - point2.X, point1.Y - point2.Y);

    public static TerminalPoint operator +(TerminalPoint point1, TerminalPoint point2)
        => new(point1.X + point2.X, point1.Y + point2.Y);

    public static TerminalPoint operator *(TerminalPoint point, int value)
        => new(point.X * value, point.Y * value);

    public static TerminalPoint operator /(TerminalPoint point, int value)
        => new(point.X / value, point.Y / value);

    #region IEquatable

    readonly bool IEquatable<TerminalPoint>.Equals(TerminalPoint other)
        => X == other.X && Y == other.Y;

    #endregion
}
