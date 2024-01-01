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

using JSSoft.Terminals.Extensions;
using JSSoft.Terminals.Hosting;

namespace JSSoft.Terminals;

public struct TerminalIndex : IEquatable<TerminalIndex>, IComparable
{
    public TerminalIndex(ITerminal terminal, TerminalCoord coord)
        : this(coord, terminal.BufferSize.Width)
    {
    }

    public TerminalIndex(ITerminal terminal, int index)
        : this(index, terminal.BufferSize.Width)
    {
    }

    public TerminalIndex(TerminalCoord coord, int width)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width));

        Width = width;
        Value = coord.X + coord.Y * width;
    }

    public TerminalIndex(int x, int y, int width)
        : this(new TerminalCoord(x, y), width)
    {
    }

    public TerminalIndex(int index, int width)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width));

        Width = width;
        Value = index;
    }

    public int Value { readonly get; set; }

    public int Width { readonly get; set; }

    public readonly int X => Value % Width;

    public readonly int Y => Value / Width;

    public override readonly int GetHashCode()
    {
        return Value ^ Width;
    }

    public override readonly string ToString()
    {
        return $"{Value} ({X}, {Y})";
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj is TerminalIndex index)
        {
            return index.Value == Value;
        }
        return base.Equals(obj);
    }

    public readonly TerminalIndex MoveToFirstOfLine()
    {
        return new TerminalIndex(Value - (Value % Width), Width);
    }

    public readonly TerminalIndex MoveToEndOfLine()
    {
        return new TerminalIndex(Value + Width - (Value % Width), Width);
    }

    public readonly TerminalIndex MoveToFirstOfString(ITerminal terminal)
    {
        if (terminal.GetInfo(this) is not { } characterInfo)
            return this.MoveToFirstOfLine();

        var group = characterInfo.Group;
        var index = this - 1;
        return index.MoveBackward(terminal, (i, v) =>
        {
            if (v is { } c2)
                return c2.Group != group;
            if (v is not null)
                return true;
            return false;
        }) + 1;
    }

    public readonly TerminalIndex MoveForwardToEndOfString(ITerminal terminal)
    {
        if (terminal.GetInfo(this) is not { } ci)
            throw new InvalidOperationException();

        var group = ci.Group;
        var index = this + 1;
        return index.MoveForward(terminal, (i, v) =>
        {
            if (v is { } c2)
                return c2.Group != group;
            if (v is not null)
                return true;
            return false;
        }) - 1;
    }

    public readonly TerminalIndex MoveBackwardToEndOfString(ITerminal terminal)
    {
        var index = this;
        return index.MoveBackward(terminal, (index, characterInfo) =>
        {
            if (characterInfo is { } info)
                return info.Character == char.MinValue;
            if (characterInfo is null)
                return true;
            return index.X != 0;
        }) + 1;
    }

    internal readonly TerminalIndex MoveForward(TerminalBlockBase block, int characterCount)
    {
        var index = this;
        for (var i = 0; i < characterCount; i++)
        {
            index = index.MoveForward(block);
        }
        return index;
    }

    internal readonly TerminalIndex MoveForward(TerminalBlockBase block)
    {
        var index = this;
        if (index.Y >= 0 && index.Y < block.Lines.Count)
        {
            while (block.Lines.GetCharacterInfo(index) is { } c1 && c1.Span <= 0)
            {
                index++;
            }
            if (block.Lines.GetCharacterInfo(index) is { } characterInfo)
            {
                index += characterInfo.Span;
            }
            while (block.Lines.GetCharacterInfo(index) is { } c2 && c2.Span <= 0)
            {
                index++;
            }
        }
        return index;
    }

    public readonly TerminalIndex MoveForward(ITerminal terminal, Func<TerminalIndex, TerminalCharacterInfo?, bool> predicate)
    {
        var index = this;
        var c = (TerminalCoord)index;
        var i = terminal.GetInfo(c);
        while (predicate(index, i) == true)
        {
            index++;
            c = (TerminalCoord)index;
            i = terminal.GetInfo(c);
        }
        return index;
    }

    public readonly TerminalIndex MoveBackward(ITerminal terminal, Func<TerminalIndex, TerminalCharacterInfo?, bool> predicate)
    {
        var index = this;
        var c = (TerminalCoord)index;
        var i = terminal.GetInfo(c);
        while (predicate(index, i) == true)
        {
            index--;
            c = (TerminalCoord)index;
            i = terminal.GetInfo(c);
        }
        return index;
    }

    public readonly TerminalIndex Linefeed()
    {
        var index = this;
        var value = index.Width - (index.Value % index.Width);
        index += value;
        return index;
    }

    public readonly TerminalIndex CarriageReturn()
    {
        var index = this;
        var value = index.Value % index.Width;
        index -= value;
        return index;
    }

    public readonly TerminalIndex Backspace()
    {
        var coord = (TerminalCoord)this;
        coord.X = Math.Min(coord.X, Value - 1);
        coord.X--;
        return new TerminalIndex(coord, Width);
    }

    public readonly TerminalIndex CursorDown(int count, int bottom)
    {
        var index = this;
        var max = bottom - index.Y - 1;
        var count2 = Math.Min(count, max);
        index += count2 * Width;
        return index;
    }

    public readonly TerminalIndex CursorUp(int count, int top)
    {
        var index = this;
        var max = index.Y - top;
        var count2 = Math.Min(count, max);
        index -= count2 * Width;
        return index;
    }

    public readonly TerminalIndex CursorLeft(int count)
    {
        var coord = (TerminalCoord)this;
        coord.X = Math.Max(coord.X - count, 0);
        return new TerminalIndex(coord, Width);
    }

    public readonly TerminalIndex CursorRight(int count)
    {
        var coord = (TerminalCoord)this;
        coord.X = Math.Min(Width - 1, coord.X + count);
        return new TerminalIndex(coord, Width);
    }

    public readonly TerminalIndex VerticalTAB()
    {
        return this + Width;
    }

    // public readonly TerminalIndex CursorToBeginningOfNextLine()
    // {
    //     var coord = (TerminalCoord)this;
    //     coord.X = 0;
    //     coord.Y++;
    //     return new TerminalIndex(coord, Width);
    // }

    public readonly TerminalIndex CursorToColumn(int column)
    {
        if (column < 0 || column >= Width)
            throw new ArgumentOutOfRangeException(nameof(column));
        var index = this.MoveToFirstOfLine();
        index += column;
        return index;
    }

    public readonly TerminalIndex Reset()
    {
        return new TerminalIndex(0, Width);
    }

    public readonly TerminalIndex Expect(int span)
    {
        var x = Value % Width;
        if (x + span > Width)
        {
            return this.Linefeed();
        }
        return this;
    }

    public static int DistanceOf(TerminalIndex index1, TerminalIndex index2)
    {
        if (index1.Width != index2.Width)
            throw new ArgumentException($"'{nameof(index1)}' and '{nameof(index2)}' do not have the same width value.", nameof(index2));
        return index2.Value - index1.Value;
    }

    public static TerminalIndex operator +(TerminalIndex index, int value)
    {
        return new TerminalIndex(index.Value + value, index.Width);
    }

    public static TerminalIndex operator -(TerminalIndex index, int value)
    {
        return new TerminalIndex(index.Value - value, index.Width);
    }

    public static TerminalIndex operator +(TerminalIndex index1, TerminalIndex index2)
    {
        if (index1.Width != index2.Width)
            throw new ArgumentException($"'{nameof(index1)}' and '{nameof(index2)}' do not have the same width value.", nameof(index2));

        return new TerminalIndex(index1.Value + index2.Value, index1.Width);
    }

    public static TerminalIndex operator -(TerminalIndex index1, TerminalIndex index2)
    {
        if (index1.Width != index2.Width)
            throw new ArgumentException($"'{nameof(index1)}' and '{nameof(index2)}' do not have the same width value.", nameof(index2));

        return new TerminalIndex(index1.Value - index2.Value, index1.Width);
    }

    public static TerminalIndex operator ++(TerminalIndex index)
    {
        return new TerminalIndex(index.Value + 1, index.Width);
    }

    public static TerminalIndex operator --(TerminalIndex index)
    {
        return new TerminalIndex(index.Value - 1, index.Width);
    }

    public static bool operator ==(TerminalIndex index1, TerminalIndex index2)
    {
        return index1.Value == index2.Value && index1.Width == index2.Width;
    }

    public static bool operator !=(TerminalIndex index1, TerminalIndex index2)
    {
        return index1.Value != index2.Value || index1.Width != index2.Width;
    }

    public static bool operator <(TerminalIndex index1, TerminalIndex index2)
    {
        return index1.Value < index2.Value;
    }

    public static bool operator <=(TerminalIndex index1, TerminalIndex index2)
    {
        return index1.Value <= index2.Value;
    }

    public static bool operator >(TerminalIndex index1, TerminalIndex index2)
    {
        return index1.Value > index2.Value;
    }

    public static bool operator >=(TerminalIndex index1, TerminalIndex index2)
    {
        return index1.Value >= index2.Value;
    }

    public static explicit operator TerminalCoord(TerminalIndex index)
    {
        return new TerminalCoord(index.Value % index.Width, index.Value / index.Width);
    }

    public static implicit operator int(TerminalIndex index) => index.Value;

    #region IEquatable

    readonly bool IEquatable<TerminalIndex>.Equals(TerminalIndex other) => Value == other.Value;

    #endregion

    #region IComparable

    readonly int IComparable.CompareTo(object? obj)
    {
        if (obj is TerminalIndex index)
        {
            return Value.CompareTo(index);
        }
        else if (obj is null)
        {
            return 1;
        }
        throw new ArgumentException($"Object is not a {nameof(TerminalIndex)}", nameof(obj));
    }

    #endregion
}
