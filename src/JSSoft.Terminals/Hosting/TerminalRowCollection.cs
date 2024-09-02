// <copyright file="TerminalRowCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Terminals.Hosting;

internal sealed class TerminalRowCollection : List<TerminalRow>
{
    private readonly Terminal _terminal;
    private readonly Stack<TerminalRow> _poolStack = new();

    public TerminalRowCollection(Terminal terminal)
    {
        _terminal = terminal;
        _terminal.PropertyChanged += Terminal_PropertyChanged;
        Resize(_terminal.BufferSize.Height);
    }

    public void Update(TerminalLineCollection lines)
    {
        var scroll = _terminal.Scroll;
        for (var i = 0; i < Count; i++)
        {
            var y = scroll.Value + i;
            var row = this[i];
            var line = lines.TryGetLine(y, out var l) is true ? l : null;
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
