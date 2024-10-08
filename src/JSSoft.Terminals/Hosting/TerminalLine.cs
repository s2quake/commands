// <copyright file="TerminalLine.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Text;

namespace JSSoft.Terminals.Hosting;

internal sealed class TerminalLine : IDisposable
{
    private readonly TerminalArrayReference<TerminalCharacterInfo?> _items;
    private readonly List<TerminalLine> _children = [];
    private readonly int _y;
    private readonly int _width;
    private int _group;
    private TerminalLine? _parent;
    private TerminalLine? _prev;
    private TerminalLine? _next;
    private bool _isDisposed;

    public TerminalLine(TerminalArray<TerminalCharacterInfo?> items, int y, int width)
    {
        _items = new(items, y * width, width);
        _y = y;
        _width = width;
        _group = GetHashCode();
    }

    public TerminalCharacterInfo? this[int index] => _items[index];

    public int Length { get; private set; }

    public bool IsModified { get; private set; }

    public IReadOnlyList<TerminalLine> Children => _children;

    public TerminalLine? Parent
    {
        get => _parent;
        set
        {
            if (_parent == this)
            {
                throw new InvalidOperationException();
            }

            if (_parent != value)
            {
                if (_parent is not null)
                {
                    _parent._next = _next;
                    if (_next is not null)
                    {
                        _next._prev = _prev;
                    }

                    _parent._children.Remove(this);
                    _group = GetHashCode();
                }

                _parent = value;
                if (_parent is not null)
                {
                    if (_parent.Children.Count is 0)
                    {
                        _parent._next = this;
                        _prev = _parent;
                    }
                    else
                    {
                        _prev = _parent.Children[^1];
                        _prev._next = this;
                    }

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
            {
                continue;
            }

            sb.Append(item.Character);
        }

        return sb.ToString();
    }
#endif

    public void Update()
    {
        if (IsModified is false)
        {
            throw new InvalidOperationException($"'{this}' is not modified.");
        }

        for (var i = Length - 1; i >= 0; i--)
        {
            if (_items[i] is not null)
            {
                continue;
            }

            if (i + 1 == Length)
            {
                Length = i;
            }
            else
            {
                _items[i] = new TerminalCharacterInfo
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
        {
            throw new ArgumentException("The Y of index is different from the Y of this.", nameof(index));
        }

        SetCharacterInfo(index.X, characterInfo);
    }

    public void SetEmpty(int index)
    {
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (index >= _width)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        _items[index] = new TerminalCharacterInfo
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
        {
            throw new ArgumentException("The Y of index is different from the Y of this.", nameof(index));
        }

        SetEmpty(index.X, length);
    }

    public void SetEmpty(int index, int length)
    {
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (index + length > _width)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        var begin = index;
        var end = index + length;
        for (var i = begin; i < end; i++)
        {
            _items[i] = new TerminalCharacterInfo
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
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (index >= _width)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (characterInfo.Group is not 0)
        {
            throw new ArgumentException($"The value of the property '{nameof(TerminalCharacterInfo.Group)}' of '{nameof(TerminalCharacterInfo)}' must be 0.", nameof(characterInfo));
        }

        var i1 = index;
        var i2 = index + characterInfo.Span;
        characterInfo.Group = _group;
        _items[i1++] = characterInfo;
        for (; i1 < i2; i1++)
        {
            _items[i1] = new TerminalCharacterInfo
            {
                Span = -i1,
                Group = _group,
            };
        }

        IsModified = true;
        Length = Math.Max(Length, i1);
    }

    public void InsertCharacter(int index, TerminalCharacterInfo characterInfo)
    {
        if (index < 0 || index >= _width)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        ShiftRight(index, characterInfo.Span);
        _items[index] = characterInfo;
        Length++;
    }

    public void Delete(int index, int length)
    {
        if (index < 0 || index + length >= _width)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        for (var i = index + length; i < Length; i++)
        {
            _items[i - length] = _items[i];
        }

        for (var i = Length - 1; i < Length; i++)
        {
            _items[i] = null;
        }

        Length -= length;
    }

    public TerminalIndex Backspace(TerminalIndex index)
    {
        if (index.Y != _y)
        {
            throw new ArgumentException("The Y of index is different from the Y of this.", nameof(index));
        }

        if (_parent is null && index.X <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return index.Backspace();
    }

    public TerminalCharacterInfo[] GetCharacterInfos()
    {
        if (_parent is not null)
        {
            throw new InvalidOperationException("This is not the first line.");
        }

        TerminalLine[] lines = [this, .. _children];
        var capacity = lines.Sum(item => item.Length);
        var itemList = new List<TerminalCharacterInfo>(capacity);
        foreach (var line in lines)
        {
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] is not { } item || item.Span <= 0)
                {
                    continue;
                }

                item.Group = 0;
                itemList.Add(item);
            }
        }

        return [.. itemList];
    }

    public void Erase(TerminalIndex index, int length)
    {
        if (index.Y != _y)
        {
            throw new ArgumentException("The Y of index is different from the Y of this.", nameof(index));
        }

        Erase(index.X, length);
    }

    public void Erase(int index, int length)
    {
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (index + length > Length)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        var begin = index;
        var end = index + length;
        for (var i = end - 1; i >= begin; i--)
        {
            _items[i] = null;
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
            _items[i] = null;
        }

        Length = index;
        IsModified = true;
    }

    public void CopyTo(TerminalLine line)
    {
        if (line == this)
        {
            throw new ArgumentException("The line is same as this.", nameof(line));
        }

        if (line._width != _width)
        {
            throw new ArgumentException("The width of line is different from the width of this.", nameof(line));
        }

        for (var i = 0; i < _width; i++)
        {
            line._items[i] = _items[i];
        }

        line.Length = Length;
        line.IsModified = true;
    }

    public void Dispose()
    {
        if (_parent is not null)
        {
            throw new InvalidOperationException();
        }

        if (_isDisposed is true)
        {
            throw new ObjectDisposedException($"{this}");
        }

        _isDisposed = true;
    }

    private void ShiftRight(int index, int length)
    {
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        for (var i = Length - length; i >= index; i--)
        {
            _items[i + length] = _items[i];
        }
    }
}
