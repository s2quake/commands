// <copyright file="HelpCommandBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Text;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

[ResourceUsage]
public abstract class HelpCommandBase : CommandBase
{
    [CommandPropertyArray]
    public string[] CommandNames { get; set; } = [];

    [CommandPropertySwitch("detail")]
    public bool IsDetail { get; set; }

    protected override void OnExecute()
    {
        if (CommandNames.Length == 0)
        {
            PrintHelp();
        }
        else
        {
            var argList = new List<string>(CommandNames);
            var command = CommandContextBase.GetCommand(CommandContext.Node, argList);
            if (command != null && argList.Count == 0)
            {
                if (command is ICommandUsagePrinter usagePrinter)
                    usagePrinter.Print(IsDetail);
                else
                    throw new InvalidOperationException($"Command '{command.Name}' does not support help.");
            }
            else
            {
                var commandName = CommandUtility.Join(CommandNames);
                throw new InvalidOperationException($"Command '{commandName}' does not exist.");
            }
        }
    }

    protected override string[] GetCompletions(CommandCompletionContext completionContext)
    {
        var memberDescriptor = completionContext.MemberDescriptor;
        var properties = completionContext.Properties;
        if (memberDescriptor.MemberName == nameof(CommandNames))
        {
            var commandNames = Array.Empty<string>();
            if (properties.TryGetValue(nameof(CommandNames), out var value) == true && value is string[] items)
            {
                commandNames = items;
            }
            return GetCommandNames(CommandContext.Node, commandNames, completionContext.Find);
        }
        return base.GetCompletions(completionContext);
    }

    private static void PrintSummary(CommandTextWriter commandWriter, string summary)
    {
        var groupName = CommandUsagePrinterBase.StringByName[CommandUsagePrinterBase.TextSummary];
        using var _ = commandWriter.Group(groupName);
        commandWriter.WriteIndentLine(summary);
    }

    private static void PrintUsage(CommandTextWriter commandWriter, string executionName)
    {
        var groupName = CommandUsagePrinterBase.StringByName[CommandUsagePrinterBase.TextUsage];
        using var _ = commandWriter.Group(groupName);
        var text = string.Join(" ", [executionName, "<command>", "[options...]"]);
        commandWriter.WriteLine(text);
    }

    private static void PrintDescription(CommandTextWriter commandWriter, string description)
    {
        var groupName = CommandUsagePrinterBase.StringByName[CommandUsagePrinterBase.TextDescription];
        using var _ = commandWriter.Group(groupName);
        commandWriter.WriteIndentLine(description);
    }

    private static void PrintCommands(CommandTextWriter commandWriter, ICommandContext commandContext)
    {
        var rootNode = commandContext.Node;
        var query = from item in rootNode.Children
                    let command = item.Value
                    where command.IsEnabled == true
                    orderby command.Name
                    orderby command.Category
                    group command by command.Category into @group
                    select @group;

        foreach (var @group in query)
        {
            var itemList = new List<string>
            {
                @group.Key,
                CommandUsagePrinterBase.StringByName[CommandUsagePrinterBase.TextCommands],
            };
            var groupName = CommandUtility.Join(" ", itemList);
            using var _ = commandWriter.Group(groupName);
            PrintCommands(@group);
        }

        void PrintCommands(IEnumerable<ICommandNode> commandNodes)
        {
            foreach (var item in commandNodes)
            {
                var label = GetCommandNames(item);
                var summary = item.Usage?.Summary ?? string.Empty;
                commandWriter.WriteLine(label: label, summary: summary);
            }
        }
    }

    private static string GetCommandNames(ICommandNode node)
    {
        var sb = new StringBuilder();
        sb.Append(node.Name);
        foreach (var item in node.Aliases)
        {
            sb.Append($", {item}");
        }
        return sb.ToString();
    }

    private void PrintHelp()
    {
        var settings = CommandContext.Settings;
        using var commandWriter = new CommandTextWriter(settings);
        var commandUsageDescriptor = CommandDescriptor.GetUsageDescriptor(CommandContext.GetType());

        PrintSummary(commandWriter, commandUsageDescriptor.Summary);
        PrintUsage(commandWriter, CommandContext.ExecutionName);
        if (IsDetail == true)
        {
            PrintDescription(commandWriter, commandUsageDescriptor.Description);
        }
        PrintCommands(commandWriter, CommandContext);
        Out.Write(commandWriter.ToString());
    }

    private string[] GetCommandNames(ICommandNode node, string[] commandNames, string find)
    {
        var commandName = commandNames.FirstOrDefault() ?? string.Empty;
        if (commandName == string.Empty)
        {
            var query = from item in node.Children
                        let command = item.Value
                        where command.IsEnabled == true
                        from name in new string[] { command.Name }.Concat(command.Aliases)
                        where name.StartsWith(find)
                        where name != Name
                        orderby name
                        select name;
            return query.ToArray();
        }
        else if (node.Children.ContainsKey(commandName) == true)
        {
            return GetCommandNames(node.Children[commandName], commandNames.Skip(1).ToArray(), find);
        }
        else if (node.ChildByAlias.ContainsKey(commandName) == true)
        {
            return GetCommandNames(node.ChildByAlias[commandName], commandNames.Skip(1).ToArray(), find);
        }
        return [];
    }
}
