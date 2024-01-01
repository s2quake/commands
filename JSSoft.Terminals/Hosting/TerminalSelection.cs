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

using System.Collections.ObjectModel;

namespace JSSoft.Terminals.Hosting;

sealed class TerminalSelection(Terminal terminal, Action<ITerminalRow[]> updator)
    : ObservableCollection<Terminals.TerminalSelection>, ITerminalSelection
{
    private readonly Terminal _terminal = terminal;
    private readonly Action<ITerminalRow[]> _updator = updator;

    public void SelectAll()
    {
        var (bufferWidth, bufferHeight) = (_terminal.BufferSize.Width, _terminal.BufferSize.Height);
        var scroll = _terminal.Scroll;
        var scrollMaximum = scroll.Maximum;
        var c1 = new TerminalCoord(0, 0);
        var c2 = new TerminalCoord(bufferWidth, scrollMaximum + bufferHeight - 1);
        var range = new Terminals.TerminalSelection(c1, c2);
        Clear();
        Add(range);
    }

    protected override void InsertItem(int index, Terminals.TerminalSelection item)
    {
        if (item == Terminals.TerminalSelection.Empty)
            throw new ArgumentException("Invalid selection", nameof(item));
        if (Contains(item) == true)
            throw new ArgumentException("Already exists.", nameof(item));

        base.InsertItem(index, item);
        _updator.Invoke([.. _terminal.View]);
    }

    protected override void MoveItem(int oldIndex, int newIndex)
    {
        base.MoveItem(oldIndex, newIndex);
    }

    protected override void RemoveItem(int index)
    {
        base.RemoveItem(index);
    }

    protected override void ClearItems()
    {
        base.ClearItems();
        _updator.Invoke([.. _terminal.View]);
    }
}
