// <copyright file="SubCommandAsync.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands;

internal sealed class SubCommandAsync(
    CommandMethodBase method, CommandMethodDescriptor methodDescriptor)
    : CommandMethodInstance(methodDescriptor),
    ICommand,
    ICommandCompleter,
    IAsyncExecutable,
    ICommandUsage,
    ICommandUsagePrinter
{
    private readonly CommandMethodDescriptor _methodDescriptor = methodDescriptor;

    public string Name => _methodDescriptor.Name;

    public string[] Aliases => _methodDescriptor.Aliases;

    public CommandSettings Settings => method.CommandContext.Settings;

    bool ICommand.IsEnabled => _methodDescriptor.CanExecute(method);

    string ICommandUsage.ExecutionName
        => $"{method.ExecutionName} {CommandUtility.GetExecutionName(Name, Aliases)}";

    string ICommandUsage.Summary => _methodDescriptor.UsageDescriptor.Summary;

    string ICommandUsage.Description => _methodDescriptor.UsageDescriptor.Description;

    string ICommandUsage.Example => _methodDescriptor.UsageDescriptor.Example;

    public CommandMemberDescriptorCollection Members => _methodDescriptor.Members;

    public object GetMemberOwner(CommandMemberDescriptor memberDescriptor) => method;

    public Task ExecuteAsync(CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
        => _methodDescriptor.InvokeAsync(
            method, this, cancellationToken, progress);

    public string[] GetCompletions(CommandCompletionContext completionContext)
        => method.GetCompletions(
            _methodDescriptor, completionContext.MemberDescriptor, completionContext.Find);

    void ICommandUsagePrinter.Print(bool isDetail)
    {
        var settings = Settings;
        var usagePrinter = new CommandInvocationUsagePrinter(this, settings)
        {
            IsDetail = isDetail,
        };
        usagePrinter.Print(method.Out, _methodDescriptor);
    }
}
