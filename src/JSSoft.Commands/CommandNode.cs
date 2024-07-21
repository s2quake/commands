// <copyright file="CommandNode.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

internal sealed class CommandNode : ICommandNode
{
    private readonly ICommand? _command;

    public CommandNode(CommandContextBase commandContext)
    {
        CommandContext = commandContext;
    }

    public CommandNode(CommandContextBase commandContext, ICommand command)
    {
        CommandContext = commandContext;
        Category = AttributeUtility.GetCategory(command.GetType());
        _command = command;
    }

    public CommandNode? Parent { get; set; }

    public CommandNodeCollection Children { get; } = [];

    public CommandAliasNodeCollection ChildByAlias { get; } = [];

    public ICommandUsage? Usage => _command as ICommandUsage;

    public CommandContextBase CommandContext { get; }

    public List<ICommand> CommandList { get; } = [];

    public string Name => _command?.Name ?? string.Empty;

    public string Category { get; } = string.Empty;

    public string[] Aliases => _command is not null ? _command.Aliases : [];

    public bool IsEnabled => CommandList.Any(item => item.IsEnabled);

    IEnumerable<ICommand> ICommandNode.Commands => CommandList;

    ICommandNode? ICommandNode.Parent => Parent;

    ICommandNodeCollection ICommandNode.Children => Children;

    ICommandNodeCollection ICommandNode.ChildByAlias => ChildByAlias;

    ICommandContext ICommandNode.CommandContext => CommandContext;

    public override string ToString() => Name;
}
