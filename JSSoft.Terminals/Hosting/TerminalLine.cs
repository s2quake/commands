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

using System.Text;

namespace JSSoft.Terminals.Hosting;

sealed class TerminalLine : IDisposable
{
    private readonly TerminalArray<TerminalCharacterInfo?> _items;
    private readonly List<TerminalLine> _children = [];
    private readonly int _y;
    private readonly int _width;
    private readonly int _offset;
    private int _group;
    private TerminalLine? _parent;
    private bool _isDisposed;

    public TerminalLine(TerminalArray<TerminalCharacterInfo?> items, int y, int width)
    {
        _items = items;
        _y = y;
        _width = width;
        _offset = y * width;
        _group = GetHashCode();
    }

    public TerminalCharacterInfo? this[int index] => _items[index + _offset];

    public int Length { get; private set; }

    public bool IsModified { get; private set; }

    public IReadOnlyList<TerminalLine> Children => _children;

    public TerminalLine? Parent
    {
        get => _parent;
        set
        {
            if (_parent == this)
                throw new InvalidOperationException();

            if (_parent != value)
            {
                if (_parent != null)
                {
                    _parent._children.Remove(this);
                    _group = GetHashCode();
                }
                _parent = value;
                if (_parent != null)
                {
                    _parent._children.Add(this);
                    _group = _parent.Group;
                }
            }
        }
    }

    public int Group { get; private set; }

#if DEBUG
    public override string ToString()
    {
        var sb = new StringBuilder(Length);
        for (var i = 0; i < Length; i++)
        {
            if (this[i] is not { } item || item.Span <= 0)
                continue;
            sb.Append(item.Character);
        }
        return sb.ToString();
    }
#endif

    public void Update()
    {
        if (IsModified == false)
            throw new InvalidOperationException($"'{this}' is not modified.");

        for (var i = Length - 1; i >= 0; i--)
        {
            if (_items[i + _offset] is not null)
                continue;

            if (i + 1 == Length)
            {
                Length = i;
            }
            else
            {
                _items[i + _offset] = new TerminalCharacterInfo
                {
                    Character = ' ',
                    Span = 1,
                    Group = Group,
                };
            }
        }
        IsModified = false;
    }

    public void SetCharacterInfo(TerminalIndex index, TerminalCharacterInfo characterInfo)
    {
        if (index.Y != _y)
            throw new ArgumentException("The Y of index is different from the Y of this.", nameof(index));

        SetCharacterInfo(index.X, characterInfo);
    }

    public void SetEmpty(int index)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (index >= _width)
            throw new ArgumentOutOfRangeException(nameof(index));

        _items[index + _offset] = new TerminalCharacterInfo
        {
            Character = ' ',
            Span = 1,
            Group = _group,
        };
        Length = Math.Max(Length, index + 1);
        IsModified = true;
    }

    public void SetEmpty(TerminalIndex index, int length)
    {
        if (index.Y != _y)
            throw new ArgumentException("The Y of index is different from the Y of this.", nameof(index));

        SetEmpty(index.X, length);
    }

    public void SetEmpty(int index, int length)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (index + length > _width)
            throw new ArgumentOutOfRangeException(nameof(length));

        var begin = index;
        var end = index + length;
        for (var i = begin; i < end; i++)
        {
            _items[i + _offset] = new TerminalCharacterInfo
            {
                Character = ' ',
                Span = 1,
                Group = _group,
            };
        }
        Length = Math.Max(Length, end);
        IsModified = true;
    }

    public void SetCharacterInfo(int index, TerminalCharacterInfo characterInfo)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (index >= _width)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (characterInfo.Group != 0)
            throw new ArgumentException($"The value of the property '{nameof(TerminalCharacterInfo.Group)}' of '{nameof(TerminalCharacterInfo)}' must be 0.", nameof(characterInfo));

        var i1 = index;
        var i2 = index + characterInfo.Span;
        _items[i1++ + _offset] = characterInfo;
        for (; i1 < i2; i1++)
        {
            _items[i1 + _offset] = new TerminalCharacterInfo
            {
                Span = -i1,
                Group = _group,
            };
        }
        IsModified = true;
        Length = Math.Max(Length, i1);
    }

    public TerminalCharacterInfo[] GetCharacterInfos()
    {
        if (_parent != null)
            throw new InvalidOperationException("This is not the first line.");

        TerminalLine[] lines = [this, .. _children];
        var capacity = lines.Sum(item => item.Length);
        var itemList = new List<TerminalCharacterInfo>(capacity);
        foreach (var line in lines)
        {
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] is not { } item || item.Span <= 0)
                    continue;
                item.Group = 0;
                itemList.Add(item);
            }
        }
        return [.. itemList];
    }

    public void Erase(TerminalIndex index, int length)
    {
        if (index.Y != _y)
            throw new ArgumentException("The Y of index is different from the Y of this.", nameof(index));

        Erase(index.X, length);
    }

    public void Erase(int index, int length)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (index + length > Length)
            throw new ArgumentOutOfRangeException(nameof(length));

        var begin = index;
        var end = index + length;
        for (var i = end - 1; i >= begin; i--)
        {
            _items[i + _offset] = null;
            if (i + 1 == Length)
            {
                Length = i;
            }
        }
        IsModified = true;
    }

    public void Take(int index)
    {
        for (var i = index; i < _width; i++)
        {
            _items[i + _offset] = null;
        }
        Length = index;
        IsModified = true;
    }

    public void Dispose()
    {
        if (_parent != null)
            throw new InvalidOperationException();
        if (_isDisposed == true)
            throw new ObjectDisposedException($"{this}");

        _isDisposed = true;
    }
}
