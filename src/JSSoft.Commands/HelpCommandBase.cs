// <copyright file="HelpCommandBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Text;
using JSSoft.Commands.Extensions;
using static JSSoft.Commands.CommandUsagePrinterBase;

namespace JSSoft.Commands;

[ResourceUsage]
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

    protected override void OnExecute()
    {
        if (CommandNames.Length is 0)
        {
            PrintHelp();
        }
        else
        {
            PrintCommandHelp();
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

            return GetCommandNames(Context.Node, commandNames, completionContext.Find);
        }

        return base.GetCompletions(completionContext);
    }

    private static void PrintSummary(CommandTextWriter commandWriter, string summary)
    {
        var groupName = StringByName[TextSummary];
        using var groupScope = commandWriter.Group(groupName);
        commandWriter.WriteIndentLine(summary);
    }

    private static void PrintUsage(CommandTextWriter commandWriter, string executionName)
    {
        var groupName = StringByName[TextUsage];
        using var groupScope = commandWriter.Group(groupName);
        var items = new string[] { executionName, "<command>", "[options...]" };
        var text = string.Join(" ", items.Where(item => item != string.Empty));
        commandWriter.WriteLine(text);
    }

    private static void PrintDescription(CommandTextWriter commandWriter, string description)
    {
        var groupName = StringByName[TextDescription];
        using var groupScope = commandWriter.Group(groupName);
        commandWriter.WriteIndentLine(description);
    }

    private static void PrintCommands(
        CommandTextWriter commandWriter,
        ICommandContext commandContext,
        Predicate<string> categoryPredicate)
    {
        var rootNode = commandContext.Node;
        var groups = rootNode.Commands.GroupByCategory(categoryPredicate);
        foreach (var @group in groups)
        {
            var itemList = new List<string>
            {
                @group.Key,
                StringByName[TextCommands],
            };
            var groupName = CommandUtility.Join(" ", itemList);
            using var groupScope = commandWriter.Group(groupName);
            PrintCommands(@group);
        }

        void PrintCommands(IEnumerable<ICommand> nodes)
        {
            foreach (var node in nodes)
            {
                var label = GetCommandNames(node);
                var summary = node.Summary;
                commandWriter.WriteLine(label: label, summary: summary);
            }
        }
    }

    private static string GetCommandNames(ICommand node)
    {
        var sb = new StringBuilder();
        sb.Append(node.Name);
        sb.AppendMany(node.Aliases, item => $", {item}");

        return sb.ToString();
    }

    private void PrintHelp()
    {
        var settings = Context.Settings;
        using var commandWriter = new CommandTextWriter(settings);
        var commandUsageDescriptor = CommandDescriptor.GetUsage(Context.GetType());
        var categoryPredicate = new Predicate<string>(
            category => IsDetail is true || settings.CategoryPredicate(category) is true);

        PrintSummary(commandWriter, commandUsageDescriptor.Summary);
        PrintUsage(commandWriter, Context.ExecutionName);
        if (IsDetail is true)
        {
            PrintDescription(commandWriter, commandUsageDescriptor.Description);
        }

        PrintCommands(commandWriter, Context, categoryPredicate);
        Out.Write(commandWriter.ToString());
    }

    private void PrintCommandHelp()
    {
        var argList = new List<string>(CommandNames);
        var command = CommandContextBase.GetCommand(Context.Node, argList);
        if (command is not null && argList.Count is 0)
        {
            var usage = command.GetUsage(IsDetail);
            Out.Write(usage);
        }
        else
        {
            var commandName = CommandUtility.Join(CommandNames);
            throw new InvalidOperationException($"Command '{commandName}' does not exist.");
        }
    }

    private string[] GetCommandNames(ICommand node, string[] names, string find)
    {
        if (names.Length is 0)
        {
            var query = from child in node.Commands
                        where child.IsEnabled is true
                        from name in new string[] { child.Name }.Concat(child.Aliases)
                        where name.StartsWith(find)
                        where name != Name
                        orderby name
                        select name;
            return [.. query];
        }
        else if (node.TryGetCommand(names[0], out var childNode) is true)
        {
            return GetCommandNames(childNode, [.. names.Skip(1)], find);
        }

        return [];
    }
}
