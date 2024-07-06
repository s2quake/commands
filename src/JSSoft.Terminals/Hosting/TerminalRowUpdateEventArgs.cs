// <copyright file="TerminalRowUpdateEventArgs.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting;

sealed class TerminalRowUpdateEventArgs(TerminalRow[] changedRows) : EventArgs
{
    public TerminalRow[] ChangedRows { get; } = changedRows;
}
