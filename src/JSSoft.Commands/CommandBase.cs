// <copyright file="CommandBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

public abstract class CommandBase : ICommand, IExecutable
{
    private readonly CommandUsageDescriptorBase _usageDescriptor;
    private readonly CommandCollection _commands = [];
    private ICommandContext? _context;

    protected CommandBase()
        : this(parent: (object?)null, name: null, aliases: [])
    {
    }

    protected CommandBase(string[] aliases)
        : this(parent: (object?)null, name: null, aliases)
    {
    }

    protected CommandBase(string name)
        : this(parent: (object?)null, name, aliases: [])
    {
    }

    protected CommandBase(string name, string[] aliases)
        : this(parent: (object?)null, name, aliases)
    {
    }

    protected CommandBase(ICommand parent)
        : this((object)parent, name: null, aliases: [])
    {
    }

    protected CommandBase(ICommand parent, string[] aliases)
        : this((object)parent, name: null, aliases)
    {
    }

    protected CommandBase(ICommand parent, string name)
        : this((object)parent, name, aliases: [])
    {
    }

    protected CommandBase(ICommand parent, string name, string[] aliases)
        : this((object)parent, name, aliases)
    {
    }

    private CommandBase(object? parent, string? name, string[] aliases)
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

    void IExecutable.Execute() => OnExecute();

    string ICommand.GetUsage(bool isDetail) => OnUsagePrint(isDetail);

    string[] ICommand.GetCompletions(CommandCompletionContext completionContext)
        => GetCompletions(completionContext);

    protected virtual string[] GetCompletions(CommandCompletionContext completionContext) => [];

    protected abstract void OnExecute();

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
