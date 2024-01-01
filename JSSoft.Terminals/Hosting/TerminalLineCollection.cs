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

sealed class TerminalLineCollection(Terminal terminal) : IEnumerable<TerminalLine>
{
    private readonly TerminalArray<TerminalCharacterInfo?> _items = new();
    private readonly List<TerminalLine> _lineList = [];
    private readonly Terminal _terminal = terminal;

    public int Count => _lineList.Count;

    public TerminalLine this[int index] => _lineList[index];

    public TerminalLine this[TerminalIndex index] => _lineList[index.Y];

    public TerminalCharacterInfo? GetCharacterInfo(TerminalIndex index)
        => index.Value < _items.Count ? _items[index.Value] : null;

    public TerminalLine Prepare(TerminalIndex beginIndex, TerminalIndex index)
    {
        var width = _terminal.BufferSize.Width;
        while (index.Y >= _lineList.Count)
        {
            var y = _lineList.Count;
            var line = new TerminalLine(_items, y, width);
            _lineList.Add(line);
            line.Parent = beginIndex.Y != index.Y ? _lineList[beginIndex.Y] : null;
        }
        _items.Expand(_lineList.Count * width);
        return _lineList[index.Y];
    }

    public void Update()
    {
        var query = from item in _lineList
                    where item.IsModified == true
                    select item;
        foreach (var item in query)
        {
            item.Update();
        }
    }

    public void Take(TerminalIndex index)
    {
        while (index.Y < _lineList.Count)
        {
            _lineList.RemoveAt(_lineList.Count - 1);
        }
        _items.Take(index.Value);
    }

    public void Clear()
    {
        for (var i = _lineList.Count - 1; i >= 0; i--)
        {
            var line = _lineList[i];
            line.Parent = null;
            line.Dispose();
        }
        _lineList.Clear();
        _items.Reset();
    }

    public void Erase(TerminalIndex index, int length)
    {
        // if (index.Width != _terminal.BufferSize.Width)
        //     throw new ArgumentException($"'{nameof(index)}' and '{nameof(index2)}' do not have the same width value.", nameof(index));
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));

        var index1 = index;
        var index2 = index + length;

        for (var i = index2.Y - 1; i >= index1.Y; i--)
        {
            if (i < 0 || i >= _lineList.Count)
                continue;

            var line = _lineList[i];
            var x1 = i == index1.Y ? index1.X : 0;
            var x2 = i == index2.Y ? index2.X : line.Length;
            line.Erase(x1, x2 - x1);
        }
    }

    #region IEnumerable

    IEnumerator<TerminalLine> IEnumerable<TerminalLine>.GetEnumerator() => _lineList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _lineList.GetEnumerator();

    #endregion
}
