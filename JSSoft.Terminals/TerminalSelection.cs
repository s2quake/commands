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
    {
        return _beginCoord.GetHashCode() ^ _endCoord.GetHashCode();
    }

    public override readonly string ToString()
    {
        return $"{{{_beginCoord}}} - {{{_endCoord}}}";
    }

    public readonly bool Intersect(TerminalCoord coord)
    {
        return coord >= _beginCoord && coord < _endCoord;
    }

    public readonly IEnumerable<TerminalCoord> GetEnumerator(int width)
    {
        var s1 = BeginCoord;
        var s2 = EndCoord;
        // var x = s1.X;
        // var y = s1.Y;

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

    public readonly TerminalCoord BeginCoord => _beginCoord;

    public readonly TerminalCoord EndCoord => _endCoord;

    public static bool operator ==(TerminalSelection s1, TerminalSelection s2)
    {
        return s1.BeginCoord == s2.BeginCoord && s1.EndCoord == s2.EndCoord;
    }

    public static bool operator !=(TerminalSelection s1, TerminalSelection s2)
    {
        return s1.BeginCoord != s2.BeginCoord || s1.EndCoord != s2.EndCoord;
    }

    public static TerminalSelection Empty { get; } = new TerminalSelection(TerminalCoord.Invalid, TerminalCoord.Invalid);
}
