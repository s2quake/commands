// <copyright file="HelpCommandBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using System.IO;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

[ResourceUsage]
[Category]
public abstract class HelpCommandBase : CommandBase
{
    protected HelpCommandBase()
        : base("help")
    {
    }

    [CommandPropertyArray]
    public string[] CommandNames { get; set; } = [];

    [CommandPropertySwitch("detail")]
    public bool IsDetail { get; set; }

    public void PrintHelp(ICommand command)
    {
        var executionName = command.GetExecutionName();
        var commandNames = executionName.Split(' ');
        PrintCommandHelp(commandNames);
    }

    protected override void OnExecute()
    {
        if (CommandNames.Length is 0)
        {
            PrintHelp();
        }
        else
        {
            PrintCommandHelp(CommandNames);
        }
    }

    protected override string[] GetCompletions(CommandCompletionContext completionContext)
    {
        var memberDescriptor = completionContext.MemberDescriptor;
        var properties = completionContext.Properties;
        if (memberDescriptor.MemberName is nameof(CommandNames))
        {
            var commandNames = Array.Empty<string>();
            if (properties.TryGetValue(nameof(CommandNames), out var value) is true
                && value is string[] items)
            {
                commandNames = items;
            }

            return GetCommandNames(Context.Node, commandNames);
        }

        return base.GetCompletions(completionContext);
    }

    private void PrintHelp()
    {
        var settings = Context.Settings;
        using var commandWriter = new CommandTextWriter(settings);
        var usagePrinter = new CommandUsagePrinter(Context.Node, settings)
        {
            IsDetail = IsDetail,
        };
        using var sw = new StringWriter();
        usagePrinter.Print(sw);
        Out.Write(sw.ToString());
    }

    private void PrintCommandHelp(string[] commandNames)
    {
        var argList = new List<string>(commandNames);
        var command = CommandContextBase.GetCommand(Context.Node, argList);
        if (command is not null && argList.Count is 0)
        {
            var usage = command.GetUsage(IsDetail);
            Out.Write(usage);
        }
        else
        {
            var commandName = CommandUtility.Join(commandNames);
            throw new InvalidOperationException($"Command '{commandName}' does not exist.");
        }
    }

    private string[] GetCommandNames(ICommand node, string[] names)
    {
        if (names.Length is 0)
        {
            var query = from child in node.Commands
                        where child.IsEnabled is true
                        from name in new string[] { child.Name }.Concat(child.Aliases)
                        where name != Name
                        orderby name
                        select name;
            return [.. query];
        }
        else if (node.TryGetCommand(names[0], out var childNode) is true)
        {
            return GetCommandNames(childNode, [.. names.Skip(1)]);
        }

        return [];
    }
}
