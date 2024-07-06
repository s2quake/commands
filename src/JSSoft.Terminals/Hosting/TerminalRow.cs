// <copyright file="TerminalRow.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting;

sealed class TerminalRow(Terminal terminal) : ITerminalRow
{
    private readonly TerminalArray<TerminalCharacterInfo?> _items = new();

    public Terminal Terminal { get; } = terminal;

    public int Index { get; set; }

    public bool IsSelected { get; private set; }

    public int Length => _items.Count;

    public void Sync(TerminalLine? line)
    {
        if (line == null)
        {
            _items.Reset();
        }
        else
        {
            _items.Take(line.Length);
            for (var i = 0; i < line.Length; i++)
            {
                _items[i] = line[i];
            }
        }
    }

    public TerminalCharacterInfo this[int index]
    {
        get
        {
            if (index < _items.Count && _items[index] is { } characterInfo)
                return characterInfo;
            return TerminalCharacterInfo.Empty;
        }
    }

    #region ITerminalRow

    ITerminal ITerminalRow.Terminal => Terminal;

    #endregion
}
