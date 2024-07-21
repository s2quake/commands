// <copyright file="CommandMethodBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;

namespace JSSoft.Commands;

public abstract class CommandMethodBase
    : ICommand,
    ICommandHost,
    ICommandHierarchy,
    ICommandCompleter,
    ICommandUsage,
    ICommandUsagePrinter
{
    private readonly CommandCollection _commands;
    private readonly CommandUsageDescriptorBase _usageDescriptor;
    private ICommandNode? _node;

    protected CommandMethodBase()
        : this(aliases: [])
    {
    }

    protected CommandMethodBase(string[] aliases)
    {
        Name = CommandUtility.ToSpinalCase(GetType());
        Aliases = aliases;
        _commands = CreateCommands(this);
        _usageDescriptor = CommandDescriptor.GetUsageDescriptor(GetType());
    }

    protected CommandMethodBase(string name)
        : this(name, [])
    {
    }

    protected CommandMethodBase(string name, string[] aliases)
    {
        Name = name;
        Aliases = aliases;
        _commands = CreateCommands(this);
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

    ICommandCollection ICommandHierarchy.Commands => _commands;

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

    public virtual string[] GetCompletions(
        CommandMethodDescriptor methodDescriptor,
        CommandMemberDescriptor memberDescriptor,
        string find)
        => methodDescriptor.GetCompletionInternal(this, memberDescriptor, find);

    string[] ICommandCompleter.GetCompletions(CommandCompletionContext completionContext) => [];

    void ICommandUsagePrinter.Print(bool isDetail) => OnUsagePrint(isDetail);

    internal bool InvokeIsMethodEnabled(CommandMethodDescriptor memberDescriptor)
        => IsMethodEnabled(memberDescriptor);

    protected virtual bool IsMethodEnabled(CommandMethodDescriptor methodDescriptor) => true;

    protected virtual void OnUsagePrint(bool isDetail)
    {
        var settings = CommandContext.Settings;
        var commands = _node?.Commands ?? CommandCollection.Empty;
        var query = from command in commands
                    from item in CommandDescriptor.GetMethodDescriptors(command.GetType())
                    where item.CanExecute(this)
                    select item;
        var usagePrinter = new CommandInvocationUsagePrinter(this, settings)
        {
            IsDetail = isDetail,
        };
        usagePrinter.Print(Out, [.. query]);
    }

    private static CommandCollection CreateCommands(CommandMethodBase obj)
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(obj.GetType());
        var query = from methodDescriptor in methodDescriptors
                    select CreateCommand(obj, methodDescriptor);
        return new CommandCollection(query);

        static ICommand CreateCommand(
            CommandMethodBase obj, CommandMethodDescriptor methodDescriptor)
        {
            return methodDescriptor.IsAsync == true ?
                new SubCommandAsync(obj, methodDescriptor) :
                new SubCommand(obj, methodDescriptor);
        }
    }
}
