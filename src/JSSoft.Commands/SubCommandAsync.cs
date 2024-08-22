// <copyright file="SubCommandAsync.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands;

internal sealed class SubCommandAsync(
    CommandMethodBase method, CommandMethodDescriptor methodDescriptor)
    : CommandMethodInstance(methodDescriptor),
    ICommand,
    IAsyncExecutable
{
    private readonly CommandMethodDescriptor _methodDescriptor = methodDescriptor;

    public string Name => _methodDescriptor.Name;

    public string[] Aliases => _methodDescriptor.Aliases;

    public CommandSettings Settings => method.Context.Settings;

    public ICommandContext? Context { get; set; }

    bool ICommand.IsEnabled => _methodDescriptor.CanExecute(method);

    bool ICommand.AllowsSubCommands => false;

    string ICommand.Summary => _methodDescriptor.UsageDescriptor.Summary;

    string ICommand.Description => _methodDescriptor.UsageDescriptor.Description;

    string ICommand.Example => _methodDescriptor.UsageDescriptor.Example;

    string ICommand.Category => _methodDescriptor.Category;

    public CommandMemberDescriptorCollection Members => _methodDescriptor.Members;

    ICommand? ICommand.Parent
    {
        get => method;
        set => throw new NotSupportedException();
    }

    CommandCollection ICommand.Commands { get; } = CommandCollection.Empty;

    public object GetMemberOwner(CommandMemberDescriptor memberDescriptor) => method;

    public Task ExecuteAsync(CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
        => _methodDescriptor.InvokeAsync(
            method, this, cancellationToken, progress);

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
