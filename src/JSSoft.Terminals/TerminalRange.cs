// <copyright file="TerminalRange.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public readonly struct TerminalRange(int begin, int length) : IEquatable<TerminalRange>
{
    public TerminalRange(int begin)
        : this(begin, 0)
    {
    }

    public bool Contains(int value) => value >= Begin && value < End;

    public int Begin { get; } = begin;

    public int Length { get; } = length;

    public int End { get; } = checked((int)(begin + length));

    public readonly int Min => Math.Min(Begin, End);

    public readonly int Max => Math.Max(Begin, End);

    public readonly int AbsoluteLength => Math.Abs(Length);

    public static readonly TerminalRange Empty = new();

    public static TerminalRange FromBeginEnd(int begin, int end)
        => new(begin, end - begin);

    public override readonly bool Equals(object? obj)
    {
        if (obj is TerminalRange rangeInt)
        {
            return Begin == rangeInt.Begin && Length == rangeInt.Length;
        }
        return base.Equals(obj);
    }

    public override readonly int GetHashCode()
        => Begin.GetHashCode() ^ Length.GetHashCode();

    public override readonly string ToString()
        => $"{Begin}, {Length}";

    public static bool operator ==(TerminalRange rangeInt1, TerminalRange rangeInt2)
        => rangeInt1.Begin == rangeInt2.Begin && rangeInt1.Length == rangeInt2.Length;

    public static bool operator !=(TerminalRange rangeInt1, TerminalRange rangeInt2)
        => rangeInt1.Begin != rangeInt2.Begin || rangeInt1.Length != rangeInt2.Length;

    #region IEquatable

    readonly bool IEquatable<TerminalRange>.Equals(TerminalRange other)
        => Begin == other.Begin && Length == other.Length;

    #endregion
}
