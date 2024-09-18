// <copyright file="SubCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;

namespace JSSoft.Commands;

internal sealed class SubCommand(CommandMethodBase method, CommandMethodDescriptor methodDescriptor)
    : CommandMethodInstance(methodDescriptor),
    ICommand,
    IExecutable
{
    private readonly CommandMethodDescriptor _methodDescriptor = methodDescriptor;

    public string Name => _methodDescriptor.Name;

    public string[] Aliases => _methodDescriptor.Aliases;

    public CommandSettings Settings => method.Context.Settings;

    ICommandContext? ICommand.Context { get; set; }

    bool ICommand.IsEnabled => _methodDescriptor.CanExecute(method);

    bool ICommand.AllowsSubCommands => false;

    CommandUsage ICommand.Usage => _methodDescriptor.Usage;

    string ICommand.Category => _methodDescriptor.Category;

    ICommand? ICommand.Parent
    {
        get => method;
        set => throw new NotSupportedException();
    }

    CommandCollection ICommand.Commands { get; } = CommandCollection.Empty;

    public void Execute() => _methodDescriptor.Invoke(method, this);

    public string[] GetCompletions(CommandCompletionContext completionContext)
        => method.GetCompletions(
            _methodDescriptor, completionContext.MemberDescriptor, completionContext.Find);

    string ICommand.GetUsage(bool isDetail)
    {
        var settings = Settings;
        var usagePrinter = new CommandUsagePrinter(this, settings)
        {
            IsDetail = isDetail,
        };
        using var sw = new StringWriter();
        usagePrinter.Print(sw);
        return sw.ToString();
    }
}
