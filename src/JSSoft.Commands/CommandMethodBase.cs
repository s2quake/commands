// <copyright file="CommandMethodBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

public abstract class CommandMethodBase : ICommand
{
    private readonly CommandCollection _commands;
    private readonly CommandUsage _usage;
    private ICommandContext? _context;

    protected CommandMethodBase()
        : this(parent: null!, name: string.Empty, aliases: [])
    {
    }

    protected CommandMethodBase(ICommand parent)
        : this(parent, name: string.Empty, aliases: [])
    {
    }

    protected CommandMethodBase(string[] aliases)
        : this(parent: null!, name: string.Empty, aliases)
    {
    }

    protected CommandMethodBase(ICommand parent, string[] aliases)
        : this(parent, name: string.Empty, aliases)
    {
    }

    protected CommandMethodBase(string name)
        : this(parent: null!, name, aliases: [])
    {
    }

    protected CommandMethodBase(ICommand parent, string name)
        : this(parent, name, aliases: [])
    {
    }

    protected CommandMethodBase(string name, string[] aliases)
        : this(parent: null!, name, aliases)
    {
    }

    protected CommandMethodBase(ICommand parent, string name, string[] aliases)
    {
        Name = name == string.Empty ? CommandUtility.ToSpinalCase(GetType()) : name;
        Aliases = aliases;
        ICommandExtensions.SetParent(this, parent);
        _commands = CreateCommands(this);
        _usage = CommandDescriptor.GetUsage(GetType());
    }

    public string Name { get; }

    public string[] Aliases { get; }

    public virtual bool IsEnabled => true;

    bool ICommand.AllowsSubCommands => true;

    public TextWriter Out => Context.Out;

    public TextWriter Error => Context.Error;

    public ICommandContext Context
        => _context ?? throw new InvalidOperationException("The command node is not available.");

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Minor Code Smell",
        "S2292:Trivial properties should be auto-implemented",
        Justification = "This property does not need to be public.")]
    ICommandContext? ICommand.Context
    {
        get => _context;
        set => _context = value;
    }

    ICommand? ICommand.Parent { get; set; }

    CommandCollection ICommand.Commands => _commands;

    CommandUsage ICommand.Usage => _usage;

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

    public virtual string[] GetCompletions(
        CommandMethodDescriptor methodDescriptor,
        CommandMemberDescriptor memberDescriptor,
        string find)
        => methodDescriptor.GetCompletionInternal(this, memberDescriptor, find);

    string[] ICommand.GetCompletions(CommandCompletionContext completionContext) => [];

    string ICommand.GetUsage(bool isDetail) => OnUsagePrint(isDetail);

    internal bool InvokeIsMethodEnabled(CommandMethodDescriptor memberDescriptor)
        => IsMethodEnabled(memberDescriptor);

    protected virtual bool IsMethodEnabled(CommandMethodDescriptor methodDescriptor) => true;

    protected virtual string OnUsagePrint(bool isDetail)
    {
        if (_context is null)
        {
            throw new InvalidOperationException(
                "This command is not in the CommandContext.");
        }

        var settings = Context.Settings;
        var usagePrinter = new CommandUsagePrinter(this, settings)
        {
            IsDetail = isDetail,
        };
        using var sw = new StringWriter();
        usagePrinter.Print(sw);
        return sw.ToString();
    }

    private static CommandCollection CreateCommands(CommandMethodBase obj)
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(obj.GetType());
        var query = from methodDescriptor in methodDescriptors
                    select CreateCommand(obj, methodDescriptor);
        var commands = query.ToArray();
        return new CommandCollection(commands);

        static ICommand CreateCommand(
            CommandMethodBase obj, CommandMethodDescriptor methodDescriptor)
        {
            return methodDescriptor.IsAsync is true ?
                new SubCommandAsync(obj, methodDescriptor) :
                new SubCommand(obj, methodDescriptor);
        }
    }
}
