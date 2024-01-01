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

using System.ComponentModel;

namespace JSSoft.Terminals.Hosting;

sealed class TerminalRowCollection : List<TerminalRow>
{
    private readonly Terminal _terminal;
    private readonly Stack<TerminalRow> _poolStack = new();

    public TerminalRowCollection(Terminal terminal)
    {
        _terminal = terminal;
        _terminal.PropertyChanged += Terminal_PropertyChanged;
        Resize(_terminal.BufferSize.Height);
    }

    public void Update(TerminalBlockCollection blocks)
    {
        var scroll = _terminal.Scroll;
        for (var i = 0; i < Count; i++)
        {
            var y = scroll.Value + i;
            var row = this[i];
            var line = blocks.GetLine(y);
            row.Sync(line);
        }
        Updated?.Invoke(this, new([.. this]));
    }

    public TerminalRow Prepare()
    {
        return _poolStack.Count != 0 ? _poolStack.Pop() : new TerminalRow(_terminal);
    }

    public event EventHandler<TerminalRowUpdateEventArgs>? Updated;

    private void Terminal_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Terminal.BufferSize))
        {
            Resize(_terminal.BufferSize.Height);
        }
    }

    private void Resize(int bufferHeight)
    {
        for (var i = Count - 1; i >= bufferHeight; i--)
        {
            _poolStack.Push(this[i]);
            RemoveAt(i);
        }
        for (var i = Count; i < bufferHeight; i++)
        {
            var item = _poolStack.Count != 0 ? _poolStack.Pop() : new TerminalRow(_terminal);
            Add(item);
            item.Index = i;
        }
    }
}
