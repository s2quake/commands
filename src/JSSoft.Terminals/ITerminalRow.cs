// <copyright file="ITerminalRow.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public interface ITerminalRow
{
    ITerminal Terminal { get; }

    int Index { get; }

    bool IsSelected { get; }

    int Length { get; }

    TerminalCharacterInfo this[int index] { get; }
}
