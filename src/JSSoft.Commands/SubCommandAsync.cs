// <copyright file="SubCommandAsync.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands;

internal sealed class SubCommandAsync(
    CommandMethodBase method, CommandMethodDescriptor methodDescriptor)
    : ICommand, ICommandCompleter, IAsyncExecutable, ICommandUsage, ICommandUsagePrinter,
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

    public Task ExecuteAsync(CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
        => methodDescriptor.InvokeAsync(
            method, methodDescriptor.Members, cancellationToken, progress);

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
