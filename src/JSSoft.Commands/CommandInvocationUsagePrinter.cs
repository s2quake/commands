// <copyright file="CommandInvocationUsagePrinter.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Text;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

public class CommandInvocationUsagePrinter(ICommandUsage commandUsage, CommandSettings settings)
    : CommandUsagePrinterBase(settings)
{
    public virtual void Print(TextWriter writer, CommandMethodDescriptor[] methodDescriptors)
    {
        using var commandWriter = new CommandTextWriter(writer, Settings);
        if (IsDetail is true)
        {
            PrintSummary(commandWriter, commandUsage.Summary);
            PrintUsage(commandWriter, commandUsage.ExecutionName);
            PrintDescription(commandWriter, commandUsage.Description);
            PrintExample(commandWriter, commandUsage.Example);
            PrintCommandsInDetail(commandWriter, methodDescriptors);
        }
        else
        {
            PrintSummary(commandWriter, commandUsage.Summary);
            PrintUsage(commandWriter, commandUsage.ExecutionName);
            PrintExample(commandWriter, commandUsage.Example);
            PrintCommands(commandWriter, methodDescriptors);
        }
    }

    public virtual void Print(TextWriter writer, CommandMethodDescriptor methodDescriptor)
    {
        using var commandWriter = new CommandTextWriter(writer, Settings);
        if (IsDetail is true)
        {
            PrintMethodSummary(commandWriter, methodDescriptor);
            PrintUsage(commandWriter, commandUsage.ExecutionName, methodDescriptor.Members);
            PrintMethodDescription(commandWriter, methodDescriptor);
            PrintMethodExample(commandWriter, methodDescriptor);
            PrintRequirementsInDetail(commandWriter, methodDescriptor);
            PrintVariablesInDetail(commandWriter, methodDescriptor);
            PrintOptionsInDetail(commandWriter, methodDescriptor);
        }
        else
        {
            PrintMethodSummary(commandWriter, methodDescriptor);
            PrintUsage(commandWriter, commandUsage.ExecutionName, methodDescriptor.Members);
            PrintMethodExample(commandWriter, methodDescriptor);
            PrintRequirements(commandWriter, methodDescriptor);
            PrintVariables(commandWriter, methodDescriptor);
            PrintOptions(commandWriter, methodDescriptor);
        }
    }

    public virtual void Print(
        TextWriter writer,
        CommandMethodDescriptor methodDescriptor,
        CommandMemberDescriptor memberDescriptor)
    {
        using var commandWriter = new CommandTextWriter(writer, Settings);
        if (IsDetail is true)
        {
            PrintMemberSummary(commandWriter, memberDescriptor);
            PrintMemberUsage(commandWriter, memberDescriptor);
            PrintMemberDescription(commandWriter, memberDescriptor);
            PrintMemberExample(commandWriter, memberDescriptor);
        }
        else
        {
            PrintMemberSummary(commandWriter, memberDescriptor);
            PrintMemberUsage(commandWriter, memberDescriptor);
            PrintMemberExample(commandWriter, memberDescriptor);
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
        CommandTextWriter commandWriter, CommandMethodDescriptor[] methodDescriptors)
    {
        var query = from methodDescriptor in methodDescriptors
                    orderby methodDescriptor.Name
                    orderby methodDescriptor.Category
                    group methodDescriptor by methodDescriptor.Category into @group
                    select @group;

        foreach (var @group in query)
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

        void PrintCommands(IEnumerable<CommandMethodDescriptor> methodDescriptors)
        {
            foreach (var item in methodDescriptors)
            {
                var label = GetMethodString(item);
                var summary = item.UsageDescriptor.Summary;
                commandWriter.WriteLine(label: label, summary: summary);
            }
        }
    }

    protected static void PrintCommandsInDetail(
        CommandTextWriter commandWriter, CommandMethodDescriptor[] methodDescriptors)
    {
        var query = from methodDescriptor in methodDescriptors
                    orderby methodDescriptor.Name
                    orderby methodDescriptor.Category
                    group methodDescriptor by methodDescriptor.Category into @group
                    select @group;

        foreach (var @group in query)
        {
            var itemList = new List<string>
            {
                @group.Key,
                StringByName[TextCommands],
            };
            var groupName = CommandUtility.Join(" ", itemList);
            using var groupScope = commandWriter.Group(groupName);
            PrintCommandsInDetail(@group);
        }

        void PrintCommandsInDetail(IEnumerable<CommandMethodDescriptor> methodDescriptors)
        {
            foreach (var item in methodDescriptors)
            {
                var label = GetMethodString(item);
                var description = item.UsageDescriptor.Description;
                var isLast = item == methodDescriptors.Last();
                commandWriter.WriteLine(label);
                commandWriter.WriteLineIndent(description, commandWriter.Indent + 1);
                commandWriter.WriteLineIf(condition: isLast is false);
            }
        }
    }

    protected static bool PrintRequirements(
        CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        var memberDescriptors = methodDescriptor.Members;
        if (memberDescriptors.HasRequirements is true)
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
        if (memberDescriptors.HasRequirements is true)
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
        if (memberDescriptors.HasOptions is true)
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
        if (memberDescriptors.HasOptions is true)
        {
            var optionDescriptors = memberDescriptors.OptionDescriptors;
            var groupName = StringByName[TextOptions];
            using var groupScope = commandWriter.Group(groupName);
            PrintMany(commandWriter, optionDescriptors, PrintOptionInDetail, separatorCount: 1);
            return true;
        }

        return false;
    }

    protected static string GetMethodString(CommandMethodDescriptor methodDescriptor)
    {
        var sb = new StringBuilder();
        sb.Append(methodDescriptor.Name);
        foreach (var item in methodDescriptor.Aliases)
        {
            sb.Append($", {item}");
        }

        return sb.ToString();
    }
}
