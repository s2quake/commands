// <copyright file="SubCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

internal sealed class SubCommand(CommandMethodBase method, CommandMethodDescriptor methodDescriptor)
    : ICommand, ICommandCompleter, IExecutable, ICommandUsage, ICommandUsagePrinter,
    ICustomCommandDescriptor
{
    public string Name => methodDescriptor.Name;

    public string[] Aliases => methodDescriptor.Aliases;

    public CommandSettings Settings => method.CommandContext.Settings;

    bool ICommand.IsEnabled => methodDescriptor.CanExecute(method);

    string ICommandUsage.ExecutionName
        => $"{method.ExecutionName} {CommandUtility.GetExecutionName(Name, Aliases)}";

    string ICommandUsage.Summary => methodDescriptor.UsageDescriptor.Summary;

    string ICommandUsage.Description => methodDescriptor.UsageDescriptor.Description;

    string ICommandUsage.Example => methodDescriptor.UsageDescriptor.Example;

    public CommandMemberDescriptorCollection GetMembers() => methodDescriptor.Members;

    public object GetMemberOwner(CommandMemberDescriptor memberDescriptor) => method;

    public void Execute() => methodDescriptor.Invoke(method, methodDescriptor.Members);

    public string[] GetCompletions(CommandCompletionContext completionContext)
        => method.GetCompletions(
            methodDescriptor, completionContext.MemberDescriptor, completionContext.Find);

    void ICommandUsagePrinter.Print(bool isDetail)
    {
        var settings = Settings;
        var usagePrinter = new CommandInvocationUsagePrinter(this, settings)
        {
            IsDetail = isDetail,
        };
        usagePrinter.Print(method.Out, methodDescriptor);
    }
}
