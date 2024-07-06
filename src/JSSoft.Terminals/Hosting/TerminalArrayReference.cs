// <copyright file="TerminalArrayReference.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections;

namespace JSSoft.Terminals.Hosting;

sealed class TerminalArrayReference<T>(TerminalArray<T> @array, int offset, int length)
    : IEnumerable<T>
{
    private readonly TerminalArray<T> _array = @array;
    private readonly int _offset = offset;

    public int Length { get; } = length;

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            return _array[index + _offset];
        }
        set
        {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            _array[index + _offset] = value;
        }
    }

    #region IEnumerable

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        for (var i = 0; i < Length; i++)
        {
            yield return _array[i + _offset];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        for (var i = 0; i < Length; i++)
        {
            yield return _array[i + _offset];
        }
    }

    #endregion
}
