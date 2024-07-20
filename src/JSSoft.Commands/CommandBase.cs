// <copyright file="CommandBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;

namespace JSSoft.Commands;

public abstract class CommandBase
    : ICommand, IExecutable, ICommandHost, ICommandCompleter, ICommandUsage, ICommandUsagePrinter
{
    private readonly CommandUsageDescriptorBase _usageDescriptor;
    private ICommandNode? _node;

    protected CommandBase()
        : this(aliases: [])
    {
    }

    protected CommandBase(string[] aliases)
    {
        Name = CommandUtility.ToSpinalCase(GetType());
        Aliases = aliases;
        _usageDescriptor = CommandDescriptor.GetUsageDescriptor(GetType());
    }

    protected CommandBase(string name)
        : this(name, [])
    {
    }

    protected CommandBase(string name, string[] aliases)
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

    void IExecutable.Execute() => OnExecute();

    void ICommandUsagePrinter.Print(bool isDetail) => OnUsagePrint(isDetail);

    string[] ICommandCompleter.GetCompletions(CommandCompletionContext completionContext)
        => GetCompletions(completionContext);

    protected virtual string[] GetCompletions(CommandCompletionContext completionContext) => [];

    protected abstract void OnExecute();

    protected CommandMemberDescriptor GetDescriptor(string propertyName)
    {
        return CommandDescriptor.GetMemberDescriptors(GetType())[propertyName];
    }

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
