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

sealed class TerminalBlockCollection : IEnumerable<TerminalBlockBase>
{
    private readonly Queue<TerminalBlock> _poolQueue = [];
    private readonly List<TerminalBlock> _itemList;
    private readonly Terminal _terminal;
    private readonly TerminalArray<TerminalBlockBase> _indexes = [];

    public TerminalBlockCollection(Terminal terminal)
    {
        OutputBlock = new TerminalBlock(terminal);
        PromptBlock = new TerminalPrompt(terminal);
        _itemList = [OutputBlock];
        _terminal = terminal;
        OutputBlock.Index = _itemList.IndexOf(OutputBlock);
        PromptBlock.Index = _itemList.Count;
        UpdateTransform(index: 0, y: 0);
        OutputBlock.TextChanged += Block_TextChanged;
        PromptBlock.TextChanged += Block_TextChanged;
    }

    public TerminalPrompt PromptBlock { get; }

    public TerminalBlock OutputBlock { get; private set; }

    public int Count => _itemList.Count + 1;

    public int Height { get; private set; }

    public TerminalBlockBase this[int index] => index == _itemList.Count ? PromptBlock : _itemList[index];

    public void Clear()
    {
        foreach (var item in _itemList)
        {
            _poolQueue.Enqueue(item);
        }
        for (var i = _itemList.Count - 1; i >= 0; i--)
        {
            var item = _itemList[i];
            item.TextChanged -= Block_TextChanged;
            item.Index = -1;
            item.Clear();
        }
        _itemList.Clear();
        OutputBlock = _poolQueue.TryDequeue(out var outputBlock) == true ? outputBlock : new TerminalBlock(_terminal);
        _itemList.Add(OutputBlock);
        OutputBlock.Index = _itemList.IndexOf(OutputBlock);
        PromptBlock.Index = _itemList.Count;
        UpdateTransform(index: 0, y: 0);
        OutputBlock.TextChanged += Block_TextChanged;
        Updated?.Invoke(this, EventArgs.Empty);
    }

    public int IndexOf(TerminalBlockBase item)
    {
        if (item == PromptBlock)
            return _itemList.Count;
        if (item is TerminalBlock block)
            return _itemList.IndexOf(block);
        return -1;
    }

    public void Update()
    {
        var y = 0;
        for (var i = 0; i < Count; i++)
        {
            var item = this[i];
            item.Update();
            item.TryTransform(y);
            y += item.Height;
            _indexes.Expand(y);
            _indexes.SetRange(index: item.Top, length: item.Height, value: item);
        }
        Height = y;
        Updated?.Invoke(this, EventArgs.Empty);
    }

    public TerminalCharacterInfo? GetInfo(int x, int y)
    {
        var bufferWidth = _terminal.BufferSize.Width;
        if (y < 0 || y >= _indexes.Count)
            return null;
        var block = _indexes[y];
        var x1 = x;
        var y1 = y - block.Top;
        if (y1 < block.Lines.Count)
            return block.Lines[y1][x1];
        // var index = x + (y - block.Top) * bufferWidth;
        // if (index >= 0 && index < block.Items.Count)
        //     return block.Items[index];
        return null;
    }

    public event EventHandler? Updated;

    private void UpdateTransform(int index, int y)
    {
        for (var i = index; i < Count; i++)
        {
            var item = this[i];
            item.TryTransform(y);
            y += item.Height;
            _indexes.Expand(y);
            _indexes.SetRange(index: item.Top, length: item.Height, value: item);
        }
        Height = y;
    }

    private void Block_TextChanged(object? sender, TerminalTextChangedEventArgs e)
    {
        if (sender is TerminalBlockBase block)
        {
            var index = block.Index;
            var y = block.Top;
            UpdateTransform(index, y);
            Updated?.Invoke(this, EventArgs.Empty);
        }
    }

    #region IEnumerable

    IEnumerator<TerminalBlockBase> IEnumerable<TerminalBlockBase>.GetEnumerator()
    {
        foreach (var item in _itemList)
        {
            yield return item;
        }
        yield return PromptBlock;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var item in _itemList)
        {
            yield return item;
        }
        yield return PromptBlock;
    }

    #endregion
}
