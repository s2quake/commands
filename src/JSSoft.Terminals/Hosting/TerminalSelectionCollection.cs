// <copyright file="TerminalSelectionCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections.ObjectModel;

namespace JSSoft.Terminals.Hosting;

internal sealed class TerminalSelectionCollection(Terminal terminal, Action<ITerminalRow[]> updator)
    : ObservableCollection<TerminalSelection>, ITerminalSelectionCollection
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
        var range = new TerminalSelection(c1, c2);
        Clear();
        Add(range);
    }

    protected override void InsertItem(int index, TerminalSelection item)
    {
        if (item == TerminalSelection.Empty)
        {
            throw new ArgumentException("Invalid selection", nameof(item));
        }

        if (Contains(item) == true)
        {
            throw new ArgumentException("Already exists.", nameof(item));
        }

        base.InsertItem(index, item);
        _updator.Invoke([.. _terminal.View]);
    }

    protected override void ClearItems()
    {
        base.ClearItems();
        _updator.Invoke([.. _terminal.View]);
    }
}
