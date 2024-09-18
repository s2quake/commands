// <copyright file="CommandUsagePrinter.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Text;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

public class CommandUsagePrinter(ICommand command, CommandSettings settings)
    : CommandUsagePrinterBase(settings)
{
    public virtual void Print(TextWriter writer)
    {
        using var commandWriter = new CommandTextWriter(writer, Settings);
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(command);
        var executionName = command.GetExecutionName();
        if (IsDetail is true)
        {
            PrintSummary(commandWriter, command.Usage.Summary);
            if (command.AllowsSubCommands is true)
            {
                PrintUsage(commandWriter, executionName);
            }
            else
            {
                PrintUsage(commandWriter, executionName, memberDescriptors);
            }

            PrintDescription(commandWriter, command.Usage.Description);
            PrintExample(commandWriter, command.Usage.Example);
            if (command.AllowsSubCommands is true)
            {
                PrintCommandsInDetail(commandWriter, command.Commands);
            }
            else
            {
                PrintRequirementsInDetail(commandWriter, memberDescriptors);
                PrintVariablesInDetail(commandWriter, memberDescriptors);
                PrintOptionsInDetail(commandWriter, memberDescriptors);
            }
        }
        else
        {
            PrintSummary(commandWriter, command.Usage.Summary);
            if (command.AllowsSubCommands is true)
            {
                PrintUsage(commandWriter, executionName);
            }
            else
            {
                PrintUsage(commandWriter, executionName, memberDescriptors);
            }

            PrintExample(commandWriter, command.Usage.Example);
            if (command.AllowsSubCommands is true)
            {
                PrintCommands(commandWriter, command.Commands, Settings.CategoryPredicate);
            }
            else
            {
                PrintRequirements(commandWriter, memberDescriptors);
                PrintVariables(commandWriter, memberDescriptors);
                PrintOptions(commandWriter, memberDescriptors, Settings.CategoryPredicate);
            }
        }

        if (command.AllowsSubCommands is true)
        {
            PrintHelpMessage(commandWriter, executionName);
        }
    }

    protected static void PrintCommands(
        CommandTextWriter commandWriter,
        CommandCollection commands,
        Predicate<string> categoryPredicate)
    {
        var groups = commands.GroupByCategory(categoryPredicate);
        foreach (var @group in groups)
        {
            var itemList = new List<string>
            {
                @group.Key,
                StringByName[TextCommands],
            };
            var groupName = CommandUtility.Join(" ", itemList);
            PrintCommandsByGroup(groupName, commandWriter, @group);
        }
    }

    protected static void PrintCommandsInDetail(
        CommandTextWriter commandWriter, IEnumerable<ICommand> commands)
    {
        var query = from command in commands
                    orderby command.Name
                    orderby command.Category
                    group command by command.Category into @group
                    select @group;

        foreach (var @group in query)
        {
            var itemList = new List<string>
            {
                @group.Key,
                StringByName[TextCommands],
            };
            var groupName = CommandUtility.Join(" ", itemList);
            PrintCommandsInDetailByGroup(groupName, commandWriter, @group);
        }
    }

    protected static string GetCommandString(ICommand command)
    {
        var sb = new StringBuilder();
        sb.Append(command.Name);
        foreach (var item in command.Aliases)
        {
            sb.Append($", {item}");
        }

        return sb.ToString();
    }

    private static void PrintCommandsByGroup(
        string groupName, CommandTextWriter commandWriter, IEnumerable<ICommand> commands)
    {
        using var groupScope = commandWriter.Group(groupName);
        foreach (var command in commands)
        {
            var label = GetCommandString(command);
            var summary = command.Usage.Summary;
            commandWriter.WriteLine(label: label, summary: summary);
        }
    }

    private static void PrintCommandsInDetailByGroup(
        string groupName, CommandTextWriter commandWriter, IEnumerable<ICommand> commands)
    {
        using var groupScope = commandWriter.Group(groupName);
        foreach (var command in commands)
        {
            var label = GetCommandString(command);
            var description = command.Usage.Description;
            var isLast = command == commands.Last();
            commandWriter.WriteLine(label);
            commandWriter.WriteLineIndent(description, commandWriter.Indent + 1);
            commandWriter.WriteLineIf(condition: isLast is false);
        }
    }
}
