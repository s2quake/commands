// <copyright file="CommandNode.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

internal sealed class CommandNode(CommandContextBase commandContext) : ICommand
{
    private readonly CommandContextBase _commandContext = commandContext;
    private readonly CommandCollection _commands = [];

    public string Name => _commandContext.ExecutionName;

    public string[] Aliases { get; } = [];

    public bool IsEnabled => true;

    public bool AllowsSubCommands => true;

    public string Summary { get; } = string.Empty;

    public string Description { get; } = string.Empty;

    public string Example { get; } = string.Empty;

    public string Category { get; } = string.Empty;

    public ICommand? Parent { get; }

    public CommandCollection Commands => _commands;

    public ICommandContext? Node => _commandContext;

    ICommandContext? ICommand.Context
    {
        get => _commandContext;
        set => throw new NotSupportedException();
    }

    ICommand? ICommand.Parent
    {
        get => null;
        set => throw new NotSupportedException();
    }

    public string[] GetCompletions(CommandCompletionContext completionContext) => [];

    public string GetUsage(bool isDetail) => string.Empty;
}
