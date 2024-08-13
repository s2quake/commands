// <copyright file="CommandContext.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.Composition;
using JSSoft.Commands.Applications.Commands;

namespace JSSoft.Commands.Repl;

[Export(typeof(CommandContext))]
[CommandSummary("Provides a prompt for input and execution of commands.")]
[CommandDescription(
    "Provides a REPL environment to execute certain commands with user input and output the " +
    "results of the execution.\n" +
    "This project provides examples of various commands that can be utilized with a command " +
    "string, so be sure to compare the functionality of each command with your own code to see " +
    "how it can be utilized.")]
[method: ImportingConstructor]
internal sealed class CommandContext(
    [ImportMany] IEnumerable<ICommand> commands,
    HelpCommand helpCommand,
    VersionCommand versionCommand)
    : CommandContextBase(commands)
{
    protected override ICommand HelpCommand { get; } = helpCommand;

    protected override ICommand VersionCommand { get; } = versionCommand;
}
