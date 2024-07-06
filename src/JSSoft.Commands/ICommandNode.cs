// <copyright file="ICommandNode.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public interface ICommandNode
{
    ICommandNode? Parent { get; }

    IReadOnlyDictionary<string, ICommandNode> Children { get; }

    IReadOnlyDictionary<string, ICommandNode> ChildByAlias { get; }

    string Name { get; }

    string[] Aliases { get; }

    ICommand Command { get; }

    ICommandUsage? Usage { get; }

    ICommandContext CommandContext { get; }

    bool IsEnabled { get; }

    string Category { get; }

    IEnumerable<ICommand> Commands { get; }
}
