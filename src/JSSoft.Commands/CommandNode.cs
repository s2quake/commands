// <copyright file="CommandNode.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

sealed class CommandNode : ICommandNode
{
    public CommandNode(CommandContextBase commandContext)
    {
        CommandContext = commandContext;
        Command = new EmptyCommand();
    }

    public CommandNode(CommandContextBase commandContext, ICommand command)
    {
        CommandContext = commandContext;
        Command = command;
        Category = AttributeUtility.GetCategory(command.GetType());
    }

    public override string ToString() => Name;

    public CommandNode? Parent { get; set; }

    public CommandNodeCollection Children { get; } = [];

    public CommandAliasNodeCollection ChildByAlias { get; } = [];

    public ICommand Command { get; }

    public ICommandUsage? Usage => Command as ICommandUsage;

    public CommandContextBase CommandContext { get; }

    public List<ICommand> CommandList { get; } = [];

    public string Name => Command.Name;

    public string Category { get; } = string.Empty;

    public string[] Aliases => Command != null ? Command.Aliases : [];

    public bool IsEnabled => CommandList.Any(item => item.IsEnabled);

    #region ICommandNode

    IEnumerable<ICommand> ICommandNode.Commands => CommandList;

    ICommandNode? ICommandNode.Parent => Parent;

    IReadOnlyDictionary<string, ICommandNode> ICommandNode.Children => Children;

    IReadOnlyDictionary<string, ICommandNode> ICommandNode.ChildByAlias => ChildByAlias;

    ICommandContext ICommandNode.CommandContext => CommandContext;

    #endregion

    #region EmptyCommand

    sealed class EmptyCommand : ICommand
    {
        public string Name => throw new System.NotImplementedException();

        public string[] Aliases => throw new System.NotImplementedException();

        public bool IsEnabled => throw new System.NotImplementedException();
    }

    #endregion
}
