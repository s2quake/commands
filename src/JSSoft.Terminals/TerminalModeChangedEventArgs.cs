// <copyright file="TerminalModeChangedEventArgs.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public sealed class TerminalModeChangedEventArgs(TerminalMode mode, bool value) : EventArgs
{
    public TerminalMode Mode { get; } = mode;

    public bool Value { get; } = value;
}
