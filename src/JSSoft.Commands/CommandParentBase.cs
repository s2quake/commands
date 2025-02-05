// <copyright file="CommandParentBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

public abstract class CommandParentBase : ICommand
{
    private readonly CommandUsage _usage;
    private readonly CommandCollection _commands = [];
    private ICommandContext? _context;

    protected CommandParentBase()
        : this(parent: null!, name: string.Empty, aliases: [])
    {
    }

    protected CommandParentBase(string[] aliases)
        : this(parent: null!, name: string.Empty, aliases)
    {
    }

    protected CommandParentBase(string name)
        : this(parent: null!, name, aliases: [])
    {
    }

    protected CommandParentBase(string name, string[] aliases)
        : this(parent: null!, name, aliases)
    {
    }

    protected CommandParentBase(ICommand parent)
        : this(parent, name: string.Empty, aliases: [])
    {
    }

    protected CommandParentBase(ICommand parent, string[] aliases)
        : this(parent, name: string.Empty, aliases)
    {
    }

    protected CommandParentBase(ICommand parent, string name)
        : this(parent, name, aliases: [])
    {
    }

    protected CommandParentBase(ICommand parent, string name, string[] aliases)
    {
        Name = name == string.Empty ? CommandUtility.ToSpinalCase(GetType()) : name;
        Aliases = aliases;
        _usage = CommandDescriptor.GetUsage(GetType());
        ICommandExtensions.SetParent(this, parent);
    }

    public string Name { get; }

    public string[] Aliases { get; }

    public virtual bool IsEnabled => true;

    bool ICommand.AllowsSubCommands => true;

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

    string ICommand.GetUsage(bool isDetail) => OnUsagePrint(isDetail);

    string[] ICommand.GetCompletions(CommandCompletionContext completionContext)
        => GetCompletions(completionContext);

    protected virtual string[] GetCompletions(CommandCompletionContext completionContext)
    {
        var memberDescriptor = completionContext.MemberDescriptor;
        var instance = this;
        return memberDescriptor.GetCompletionsInternal(instance);
    }

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
