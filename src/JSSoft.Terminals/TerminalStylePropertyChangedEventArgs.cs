// <copyright file="TerminalStylePropertyChangedEventArgs.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public sealed class TerminalStylePropertyChangedEventArgs<T>(T value) : EventArgs
{
    public T Value { get; } = value;
}
