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
        if (IsDetail == true)
        {
            PrintSummary(commandWriter, command.Summary);
            PrintUsage(commandWriter, command.GetExecutionName());
            PrintDescription(commandWriter, command.Description);
            PrintExample(commandWriter, command.Example);
            if (command.AllowsSubCommands == true)
            {
                PrintCommandsInDetail(commandWriter, command.Commands);
            }
            else
            {
                var memberDescriptors = CommandDescriptor.GetMemberDescriptors(command.GetType());
                PrintRequirementsInDetail(commandWriter, memberDescriptors);
                PrintVariablesInDetail(commandWriter, memberDescriptors);
                PrintOptionsInDetail(commandWriter, memberDescriptors);
            }
        }
        else
        {
            PrintSummary(commandWriter, command.Summary);
            PrintUsage(commandWriter, command.GetExecutionName());
            PrintExample(commandWriter, command.Example);
            if (command.AllowsSubCommands == true)
            {
                PrintCommands(commandWriter, command.Commands);
            }
            else
            {
                var memberDescriptors = CommandDescriptor.GetMemberDescriptors(command.GetType());
                PrintRequirements(commandWriter, memberDescriptors);
                PrintVariables(commandWriter, memberDescriptors);
                PrintOptions(commandWriter, memberDescriptors);
            }
        }
    }

    protected static void PrintMethodSummary(
        CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        var groupName = StringByName[TextSummary];
        using var groupScope = commandWriter.Group(groupName);
        var summary = methodDescriptor.UsageDescriptor.Summary;
        var width = commandWriter.Width - commandWriter.TotalIndentSpaces;
        var lines = CommandTextWriter.Wrap(summary, width);
        commandWriter.WriteLine(lines);
    }

    protected static void PrintMethodDescription(
        CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        var groupName = StringByName[TextDescription];
        using var groupScope = commandWriter.Group(groupName);
        var description = methodDescriptor.UsageDescriptor.Description;
        var width = commandWriter.Width - commandWriter.TotalIndentSpaces;
        var lines = CommandTextWriter.Wrap(description, width);
        commandWriter.WriteLine(lines);
    }

    protected static void PrintMethodExample(
        CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        var groupName = StringByName[TextExample];
        using var groupScope = commandWriter.Group(groupName);
        var example = methodDescriptor.UsageDescriptor.Example;
        commandWriter.WriteLine(example);
    }

    protected static void PrintCommands(
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

    protected static void PrintUsage(CommandTextWriter commandWriter, string executionName)
    {
        var groupName = StringByName[TextUsage];
        using var groupScope = commandWriter.Group(groupName);
        commandWriter.WriteLine($"{executionName} <command> [options...]");
    }

    protected static bool PrintRequirements(
        CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        var memberDescriptors = methodDescriptor.Members;
        if (memberDescriptors.HasRequirements == true)
        {
            var requirementDescriptors = memberDescriptors.RequirementDescriptors;
            var groupName = StringByName[TextRequirements];
            using var groupScope = commandWriter.Group(groupName);
            PrintMany(commandWriter, requirementDescriptors, PrintRequirement);
            return true;
        }

        return false;
    }

    protected static bool PrintRequirementsInDetail(
        CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        var memberDescriptors = methodDescriptor.Members;
        if (memberDescriptors.HasRequirements == true)
        {
            var requirementDescriptors = memberDescriptors.RequirementDescriptors;
            var groupName = StringByName[TextRequirements];
            using var groupScope = commandWriter.Group(groupName);
            PrintMany(
                commandWriter, requirementDescriptors, PrintRequirementInDetail, separatorCount: 1);
            return true;
        }

        return false;
    }

    protected static bool PrintVariables(
        CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        if (methodDescriptor.Members.VariablesDescriptor is { } variablesDescriptor)
        {
            var groupName = StringByName[TextVariables];
            using var groupScope = commandWriter.Group(groupName);
            PrintVariables(commandWriter, variablesDescriptor);
            return true;
        }

        return false;
    }

    protected static bool PrintVariablesInDetail(
        CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        if (methodDescriptor.Members.VariablesDescriptor is { } variablesDescriptor)
        {
            var groupName = StringByName[TextVariables];
            using var groupScope = commandWriter.Group(groupName);
            PrintVariablesInDetail(commandWriter, variablesDescriptor);
            return true;
        }

        return false;
    }

    protected static bool PrintOptions(
        CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        var memberDescriptors = methodDescriptor.Members;
        if (memberDescriptors.HasOptions == true)
        {
            var optionDescriptors = memberDescriptors.OptionDescriptors;
            var groupName = StringByName[TextOptions];
            using var groupScope = commandWriter.Group(groupName);
            PrintMany(commandWriter, optionDescriptors, PrintOption, separatorCount: 1);
            return true;
        }

        return false;
    }

    protected static bool PrintOptionsInDetail(
        CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        var memberDescriptors = methodDescriptor.Members;
        if (memberDescriptors.HasOptions == true)
        {
            var optionDescriptors = memberDescriptors.OptionDescriptors;
            var groupName = StringByName[TextOptions];
            using var groupScope = commandWriter.Group(groupName);
            PrintMany(commandWriter, optionDescriptors, PrintOptionInDetail, separatorCount: 1);
            return true;
        }

        return false;
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
            var summary = command.Summary;
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
            var description = command.Description;
            var isLast = command == commands.Last();
            commandWriter.WriteLine(label);
            commandWriter.WriteLineIndent(description, commandWriter.Indent + 1);
            commandWriter.WriteLineIf(condition: isLast != true);
        }
    }
}
