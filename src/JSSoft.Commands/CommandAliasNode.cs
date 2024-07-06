// <copyright file="CommandAliasNode.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

sealed class CommandAliasNode(ICommandNode commandNode, string alias) : ICommandNode
{
    private readonly ICommandNode commandNode = commandNode;

    public ICommandNode? Parent => commandNode.Parent;

    public IReadOnlyDictionary<string, ICommandNode> Children => commandNode.Children;

    public IReadOnlyDictionary<string, ICommandNode> ChildByAlias => commandNode.ChildByAlias;

    public string Name { get; } = alias;

    public string[] Aliases => commandNode.Aliases;

    public string Category => commandNode.Category;

    public ICommand Command => commandNode.Command;

    public ICommandUsage? Usage => commandNode.Usage;

    public ICommandContext CommandContext => commandNode.CommandContext;

    public bool IsEnabled => commandNode.IsEnabled;

    public IEnumerable<ICommand> Commands => commandNode.Commands;
}
