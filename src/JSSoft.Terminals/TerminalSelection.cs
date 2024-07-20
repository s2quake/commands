// <copyright file="TerminalSelection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public struct TerminalSelection(TerminalCoord c1, TerminalCoord c2)
{
    private TerminalCoord _beginCoord = c1 < c2 ? c1 : c2;
    private TerminalCoord _endCoord = c1 > c2 ? c1 : c2;

    public TerminalSelection(TerminalIndex i1, TerminalIndex i2)
        : this((TerminalCoord)i1, (TerminalCoord)i2)
    {
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj is TerminalSelection s)
        {
            return _beginCoord == s._beginCoord && _endCoord == s._endCoord;
        }

        return base.Equals(obj);
    }

    public override readonly int GetHashCode()
        => _beginCoord.GetHashCode() ^ _endCoord.GetHashCode();

    public override readonly string ToString()
        => $"{{{_beginCoord}}} - {{{_endCoord}}}";

    public readonly bool Intersect(TerminalCoord coord)
        => coord >= _beginCoord && coord < _endCoord;

    public readonly IEnumerable<TerminalCoord> GetEnumerator(int width)
    {
        var s1 = BeginCoord;
        var s2 = EndCoord;

        while (s1 != s2)
        {
            yield return s1;
            s1.X++;
            if (s1.X >= width)
            {
                s1.X = 0;
                s1.Y++;
            }
        }
    }

    public readonly int GetLength(int width)
    {
        var s1 = BeginCoord;
        var s2 = EndCoord;
        var length = 0;

        while (s1 != s2)
        {
            length++;
            s1.X++;
            if (s1.X >= width)
            {
                s1.X = 0;
                s1.Y++;
            }
        }

        return length;
    }

    public readonly TerminalCoord BeginCoord => _beginCoord;

    public readonly TerminalCoord EndCoord => _endCoord;

    public static bool operator ==(TerminalSelection s1, TerminalSelection s2)
        => s1.BeginCoord == s2.BeginCoord && s1.EndCoord == s2.EndCoord;

    public static bool operator !=(TerminalSelection s1, TerminalSelection s2)
        => s1.BeginCoord != s2.BeginCoord || s1.EndCoord != s2.EndCoord;

    public static TerminalSelection Empty { get; } = new TerminalSelection(TerminalCoord.Invalid, TerminalCoord.Invalid);
}
