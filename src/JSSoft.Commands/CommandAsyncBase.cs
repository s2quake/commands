// <copyright file="CommandAsyncBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands;

public abstract class CommandAsyncBase
    : ICommand, IAsyncExecutable, ICommandHost, ICommandUsage, ICommandUsagePrinter
{
    private readonly CommandUsageDescriptorBase _usageDescriptor;
    private ICommandNode? _node;
    private IProgress<ProgressInfo>? _progress;

    protected CommandAsyncBase()
        : this(aliases: [])
    {
    }

    protected CommandAsyncBase(string[] aliases)
    {
        Name = CommandUtility.ToSpinalCase(GetType());
        Aliases = aliases;
        _usageDescriptor = CommandDescriptor.GetUsageDescriptor(GetType());
    }

    protected CommandAsyncBase(string name)
        : this(name, [])
    {
    }

    protected CommandAsyncBase(string name, string[] aliases)
    {
        Name = name;
        Aliases = aliases;
        _usageDescriptor = CommandDescriptor.GetUsageDescriptor(GetType());
    }

    public string Name { get; }

    public string[] Aliases { get; }

    public virtual bool IsEnabled => true;

    public TextWriter Out => CommandContext.Out;

    public TextWriter Error => CommandContext.Error;

    public ICommandContext CommandContext
        => _node is not null
            ? _node.CommandContext
            : throw new InvalidOperationException("The command node is not available.");

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Minor Code Smell",
        "S2292:Trivial properties should be auto-implemented",
        Justification = "This property does not need to be public.")]
    ICommandNode? ICommandHost.Node
    {
        get => _node;
        set => _node = value;
    }

    string ICommandUsage.ExecutionName => ExecutionName;

    string ICommandUsage.Summary => _usageDescriptor.Summary;

    string ICommandUsage.Description => _usageDescriptor.Description;

    string ICommandUsage.Example => _usageDescriptor.Example;

    internal string ExecutionName
    {
        get
        {
            var executionName = CommandUtility.GetExecutionName(Name, Aliases);
            if (CommandContext.ExecutionName != string.Empty)
            {
                return $"{CommandContext.ExecutionName} {executionName}";
            }

            return executionName;
        }
    }

    protected IProgress<ProgressInfo> Progress
        => _progress ?? throw new InvalidOperationException("The progress is not available.");

    public virtual string[] GetCompletions(CommandCompletionContext completionContext) => [];

    Task IAsyncExecutable.ExecuteAsync(
        CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        _progress = progress;
        try
        {
            return OnExecuteAsync(cancellationToken);
        }
        finally
        {
            _progress = null;
        }
    }

    void ICommandUsagePrinter.Print(bool isDetail) => OnUsagePrint(isDetail);

    protected abstract Task OnExecuteAsync(CancellationToken cancellationToken);

    protected CommandMemberDescriptor GetDescriptor(string propertyName)
        => CommandDescriptor.GetMemberDescriptors(GetType())[propertyName];

    protected virtual void OnUsagePrint(bool isDetail)
    {
        var settings = CommandContext.Settings;
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(GetType());
        var usagePrinter = new CommandParsingUsagePrinter(this, settings)
        {
            IsDetail = isDetail,
        };
        usagePrinter.Print(Out, memberDescriptors);
    }
}
