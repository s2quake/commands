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

public readonly struct TerminalRange(int begin, int length) : IEquatable<TerminalRange>
{
    public TerminalRange(int begin)
        : this(begin, 0)
    {
    }

    public static TerminalRange FromBeginEnd(int begin, int end)
    {
        return new TerminalRange(begin, end - begin);
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj is TerminalRange rangeInt)
        {
            return this.Begin == rangeInt.Begin && this.Length == rangeInt.Length;
        }
        return base.Equals(obj);
    }

    public override readonly int GetHashCode()
    {
        return this.Begin.GetHashCode() ^ this.Length.GetHashCode();
    }

    public override readonly string ToString()
    {
        return $"{this.Begin}, {this.Length}";
    }

    public bool Contains(int value) => value >= Begin && value < End;

    public int Begin { get; } = begin;

    public int Length { get; } = length;

    public int End { get; } = checked((int)(begin + length));

    public readonly int Min => Math.Min(this.Begin, this.End);

    public readonly int Max => Math.Max(this.Begin, this.End);

    public readonly int AbsoluteLength => Math.Abs(this.Length);

    public static readonly TerminalRange Empty = new();

    public static bool operator ==(TerminalRange rangeInt1, TerminalRange rangeInt2)
    {
        return rangeInt1.Begin == rangeInt2.Begin && rangeInt1.Length == rangeInt2.Length;
    }

    public static bool operator !=(TerminalRange rangeInt1, TerminalRange rangeInt2)
    {
        return rangeInt1.Begin != rangeInt2.Begin || rangeInt1.Length != rangeInt2.Length;
    }

    #region IEquatable

    readonly bool IEquatable<TerminalRange>.Equals(TerminalRange other)
    {
        return this.Begin == other.Begin && this.Length == other.Length;
    }

    #endregion
}
