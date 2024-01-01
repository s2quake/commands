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

sealed class TerminalCellCollection(TerminalRow row) : IReadOnlyList<TerminalCharacterInfo>
{
    private readonly List<TerminalCharacterInfo?> _cellList = [];
    private readonly TerminalRow _row = row;
    private int _count;

    public int Capacity => _cellList.Count;

    public int Count
    {
        get => _count;
        set
        {
            if (_count != value)
            {
                _count = value;
                _cellList.Capacity = _cellList.Capacity < _count ? _count : _cellList.Capacity;
                while (_cellList.Count < _count)
                {
                    _cellList.Add(null);
                }
            }
        }
    }

    public TerminalCharacterInfo this[int index]
    {
        get
        {
            if (index >= _count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _cellList[index] ?? TerminalCharacterInfo.Empty;
        }
    }

    public void Set(int index, TerminalCharacterInfo characterInfo)
    {
        if (_cellList[index] is null)
        {
            _cellList[index] = TerminalCharacterInfo.Empty;
            _row.ReferenceCount++;
        }
        _cellList[index] = characterInfo;
    }

    public void Reset(int index)
    {
        if (_cellList[index] is { } cell)
        {
            _cellList[index] = null;
            _row.ReferenceCount--;
        }
    }

    #region IEnumerable

    public IEnumerator<TerminalCharacterInfo> GetEnumerator()
    {
        foreach (var item in _cellList)
        {
            yield return item ?? TerminalCharacterInfo.Empty;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var item in _cellList)
        {
            yield return item ?? TerminalCharacterInfo.Empty;
        }
    }

    #endregion
}
