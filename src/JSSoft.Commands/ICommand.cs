// <copyright file="ICommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public interface ICommand
{
    string Name { get; }

    string[] Aliases { get; }

    bool IsEnabled { get; }

    bool AllowsSubCommands { get; }

    string Summary { get; }

    string Description { get; }

    string Example { get; }

    string Category { get; }

    ICommand? Parent { get; set; }

    CommandCollection Commands { get; }

    ICommandContext? Context { get; set; }

    string[] GetCompletions(CommandCompletionContext completionContext);

    string GetUsage(bool isDetail);
}
