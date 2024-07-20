// <copyright file="TerminalArray.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections;

namespace JSSoft.Terminals.Hosting;

internal sealed class TerminalArray<T> : IEnumerable<T>
{
    private readonly int _growSize = 128;

    private readonly List<T[]> _lineList = [];
    private int _count;

    public TerminalArray()
    {
    }

    public TerminalArray(int growSize)
    {
        _growSize = growSize;
    }

    public int Count
    {
        get => _count;
        private set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (_count != value)
            {
                while (value >= _lineList.Count * _growSize)
                {
                    _lineList.Add(new T[_growSize]);
                }

                _count = value;
            }
        }
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var y = index / _growSize;
            var x = index % _growSize;
            return _lineList[y][x];
        }
        set
        {
            if (index < 0 || index >= _count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var y = index / _growSize;
            var x = index % _growSize;
            _lineList[y][x] = value;
        }
    }

    public void Reset()
    {
        for (var x = 0; x < _lineList.Count; x++)
        {
            Array.Clear(_lineList[x], 0, _growSize);
        }

        _count = 0;
    }

    public void Take(int count)
    {
        for (var i = count; i < Count; i++)
        {
            this[i] = default!;
        }

        Count = count;
    }

    public void SetRange(int index, int length, T value)
    {
        for (var i = 0; i < length; i++)
        {
            this[i + index] = value;
        }
    }

    public void Expand(int count)
    {
        if (count > Count)
        {
            Count = count;
        }
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        for (var i = 0; i < _count; i++)
        {
            var y = i / _growSize;
            var x = i % _growSize;
            yield return _lineList[y][x];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        for (var i = 0; i < _count; i++)
        {
            var y = i / _growSize;
            var x = i % _growSize;
            yield return _lineList[y][x];
        }
    }
}
