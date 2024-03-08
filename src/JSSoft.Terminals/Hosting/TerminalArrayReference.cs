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
