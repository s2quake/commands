// <copyright file="CommandCompletionContext.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public sealed class CommandCompletionContext(
    ICommand command,
    CommandMemberDescriptor memberDescriptor,
    IReadOnlyDictionary<string, object?> properties)
{
    public ICommand Command { get; } = command;

    public CommandMemberDescriptor MemberDescriptor { get; } = memberDescriptor;

    public IReadOnlyDictionary<string, object?> Properties { get; } = properties;

    public string MemberName => MemberDescriptor.MemberName;
}
