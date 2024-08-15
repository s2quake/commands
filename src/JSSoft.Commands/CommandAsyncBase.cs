// <copyright file="CommandAsyncBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

public abstract class CommandAsyncBase : ICommand, IAsyncExecutable
{
    private readonly CommandUsageDescriptorBase _usageDescriptor;
    private readonly CommandCollection _commands = [];
    private ICommandContext? _context;
    private IProgress<ProgressInfo>? _progress;

    protected CommandAsyncBase()
        : this(parent: (object?)null, name: null, aliases: [])
    {
    }

    protected CommandAsyncBase(string[] aliases)
        : this(parent: (object?)null, name: null, aliases)
    {
    }

    protected CommandAsyncBase(string name)
        : this(parent: (object?)null, name, aliases: [])
    {
    }

    protected CommandAsyncBase(string name, string[] aliases)
        : this(parent: (object?)null, name, aliases)
    {
    }

    protected CommandAsyncBase(ICommand parent)
        : this((object)parent, name: null, aliases: [])
    {
    }

    protected CommandAsyncBase(ICommand parent, string[] aliases)
        : this((object)parent, name: null, aliases)
    {
    }

    protected CommandAsyncBase(ICommand parent, string name)
        : this((object)parent, name, aliases: [])
    {
    }

    protected CommandAsyncBase(ICommand parent, string name, string[] aliases)
        : this((object)parent, name, aliases)
    {
    }

    private CommandAsyncBase(object? parent, string? name, string[] aliases)
    {
        Name = name ?? CommandUtility.ToSpinalCase(GetType());
        Aliases = aliases;
        _usageDescriptor = CommandDescriptor.GetUsageDescriptor(GetType());
        this.SetParent(parent as ICommand);
    }

    public string Name { get; }

    public string[] Aliases { get; }

    public virtual bool IsEnabled => true;

    bool ICommand.AllowsSubCommands => false;

    public TextWriter Out => Context.Out;

    public TextWriter Error => Context.Error;

    public ICommandContext Context
        => _context ?? throw new InvalidOperationException("The command node is not available.");

    ICommand? ICommand.Parent { get; set; }

    CommandCollection ICommand.Commands => _commands;

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Minor Code Smell",
        "S2292:Trivial properties should be auto-implemented",
        Justification = "This property does not need to be public.")]
    ICommandContext? ICommand.Context
    {
        get => _context;
        set => _context = value;
    }

    string ICommand.Summary => _usageDescriptor.Summary;

    string ICommand.Description => _usageDescriptor.Description;

    string ICommand.Example => _usageDescriptor.Example;

    string ICommand.Category => AttributeUtility.GetCategory(GetType());

    internal string ExecutionName
    {
        get
        {
            var executionName = CommandUtility.GetExecutionName(Name, Aliases);
            if (Context.ExecutionName != string.Empty)
            {
                return $"{Context.ExecutionName} {executionName}";
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

    string ICommand.GetUsage(bool isDetail) => OnUsagePrint(isDetail);

    protected abstract Task OnExecuteAsync(CancellationToken cancellationToken);

    protected CommandMemberDescriptor GetDescriptor(string propertyName)
        => CommandDescriptor.GetMemberDescriptors(GetType())[propertyName];

    protected virtual string OnUsagePrint(bool isDetail)
    {
        var settings = Context.Settings;
        var usagePrinter = new CommandUsagePrinter(this, settings)
        {
            IsDetail = isDetail,
        };
        using var sw = new StringWriter();
        usagePrinter.Print(sw);
        return sw.ToString();
    }
}
