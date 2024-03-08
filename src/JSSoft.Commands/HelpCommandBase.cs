// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

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
        var query = from item in rootNode.Childs
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
            var query = from item in node.Childs
                        let command = item.Value
                        where command.IsEnabled == true
                        from name in new string[] { command.Name }.Concat(command.Aliases)
                        where name.StartsWith(find)
                        where name != Name
                        orderby name
                        select name;
            return query.ToArray();
        }
        else if (node.Childs.ContainsKey(commandName) == true)
        {
            return GetCommandNames(node.Childs[commandName], commandNames.Skip(1).ToArray(), find);
        }
        else if (node.ChildsByAlias.ContainsKey(commandName) == true)
        {
            return GetCommandNames(node.ChildsByAlias[commandName], commandNames.Skip(1).ToArray(), find);
        }
        return [];
    }
}
