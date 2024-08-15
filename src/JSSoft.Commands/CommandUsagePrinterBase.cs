// <copyright file="CommandUsagePrinterBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable S4144 // Methods should not have identical implementations

using System.CodeDom.Compiler;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

public abstract class CommandUsagePrinterBase(CommandSettings settings)
{
    public const string TextSummary = "Summary";
    public const string TextDescription = "Description";
    public const string TextUsage = "Usage";
    public const string TextExample = "Example";
    public const string TextRequirements = "Requirements";
    public const string TextOptions = "Options";
    public const string TextCommands = "Commands";
    public const string TextVariables = "Variables";

    public static IDictionary<string, string> StringByName { get; } = new Dictionary<string, string>
    {
        { TextSummary, TextSummary },
        { TextDescription, TextDescription },
        { TextUsage, TextUsage },
        { TextExample, TextExample },
        { TextRequirements, TextRequirements },
        { TextOptions, TextOptions },
        { TextCommands, TextCommands },
        { TextVariables, TextVariables },
    };

    public CommandSettings Settings { get; } = settings;

    public bool IsDetail { get; set; }

    protected static bool PrintSummary(CommandTextWriter commandWriter, string summary)
    {
        if (summary != string.Empty)
        {
            var groupName = StringByName[TextSummary];
            using var _ = commandWriter.Group(groupName);
            commandWriter.WriteIndentLine(summary);
            return true;
        }

        return false;
    }

    protected static bool PrintDescription(CommandTextWriter commandWriter, string description)
    {
        if (description != string.Empty)
        {
            var groupName = StringByName[TextDescription];
            using var _ = commandWriter.Group(groupName);
            commandWriter.WriteIndentLine(description);
            return true;
        }

        return false;
    }

    protected static bool PrintExample(CommandTextWriter commandWriter, string example)
    {
        if (example != string.Empty)
        {
            var groupName = StringByName[TextExample];
            using var _ = commandWriter.Group(groupName);
            commandWriter.WriteLine(example);
            return true;
        }

        return false;
    }

    protected static void PrintUsage(
        CommandTextWriter commandWriter,
        string executionName,
        CommandMemberDescriptorCollection memberDescriptors)
    {
        var groupName = StringByName[TextUsage];
        using var _ = commandWriter.Group(groupName);
        string[] items =
        [
            .. from item in memberDescriptors where item.IsRequired == true select GetString(item),
            memberDescriptors.HasOptions == true ? "[options]" : string.Empty,
            .. from item in memberDescriptors where item.IsVariables == true select GetString(item),
        ];
        var maxWidth = commandWriter.Width
            - (IndentedTextWriter.DefaultTabString.Length * commandWriter.Indent)
            - (executionName.Length + 1);
        var lines = CommandTextWriter.Wrap(items, maxWidth);
        for (var i = 0; i < lines.Length; i++)
        {
            var pre = i == 0 ? executionName : string.Empty.PadRight(executionName.Length);
            commandWriter.WriteLine($"{pre} {lines[i]}");
        }
    }

    protected static bool PrintRequirements(
        CommandTextWriter commandWriter, CommandMemberDescriptorCollection memberDescriptors)
    {
        if (memberDescriptors.HasRequirements == true)
        {
            var requirementDescriptors = memberDescriptors.RequirementDescriptors;
            var groupName = StringByName[TextRequirements];
            using var _ = commandWriter.Group(groupName);
            PrintMany(commandWriter, requirementDescriptors, PrintRequirement);
            return true;
        }

        return false;
    }

    protected static bool PrintRequirementsInDetail(
        CommandTextWriter commandWriter, CommandMemberDescriptorCollection memberDescriptors)
    {
        if (memberDescriptors.HasRequirements == true)
        {
            var requirementDescriptors = memberDescriptors.RequirementDescriptors;
            var groupName = StringByName[TextRequirements];
            using var _ = commandWriter.Group(groupName);
            PrintMany(
                commandWriter, requirementDescriptors, PrintRequirementInDetail, separatorCount: 1);
            return true;
        }

        return false;
    }

    protected static bool PrintVariables(
        CommandTextWriter commandWriter, CommandMemberDescriptorCollection memberDescriptors)
    {
        if (memberDescriptors.VariablesDescriptor is { } variablesDescriptor)
        {
            var groupName = StringByName[TextVariables];
            using var _ = commandWriter.Group(groupName);
            PrintVariables(commandWriter, variablesDescriptor);
            return true;
        }

        return false;
    }

    protected static void PrintVariables(
        CommandTextWriter commandWriter, CommandMemberDescriptor memberDescriptor)
    {
        var label = memberDescriptor.DisplayName;
        var summary = memberDescriptor.UsageDescriptor.Summary;
        commandWriter.WriteLine(label: label, summary: summary);
    }

    protected static bool PrintVariablesInDetail(
        CommandTextWriter commandWriter, CommandMemberDescriptorCollection memberDescriptors)
    {
        if (memberDescriptors.VariablesDescriptor is { } variablesDescriptor)
        {
            var groupName = StringByName[TextVariables];
            using var _ = commandWriter.Group(groupName);
            PrintVariablesInDetail(commandWriter, variablesDescriptor);
            return true;
        }

        return false;
    }

    protected static void PrintVariablesInDetail(
        CommandTextWriter commandWriter, CommandMemberDescriptor memberDescriptor)
    {
        var label = memberDescriptor.DisplayName;
        var description = memberDescriptor.UsageDescriptor.Description;
        commandWriter.WriteLine(label);
        commandWriter.WriteLineIndent(description, commandWriter.Indent + 1);
    }

    protected static bool PrintOptions(
        CommandTextWriter commandWriter, CommandMemberDescriptorCollection memberDescriptors)
    {
        if (memberDescriptors.HasOptions == true)
        {
            var optionDescriptors = memberDescriptors.OptionDescriptors;
            var groupName = StringByName[TextOptions];
            using var _ = commandWriter.Group(groupName);
            PrintMany(commandWriter, optionDescriptors, PrintOption, separatorCount: 1);
            return true;
        }

        return false;
    }

    protected static bool PrintOptionsInDetail(
        CommandTextWriter commandWriter, CommandMemberDescriptorCollection memberDescriptors)
    {
        if (memberDescriptors.HasOptions == true)
        {
            var optionDescriptors = memberDescriptors.OptionDescriptors;
            var groupName = StringByName[TextOptions];
            using var _ = commandWriter.Group(groupName);
            for (var i = 0; i < optionDescriptors.Length; i++)
            {
                var item = optionDescriptors[i];
                var isLast = i + 1 == optionDescriptors.Length;
                PrintOptionInDetail(commandWriter, item);
                commandWriter.WriteLineIf(condition: isLast != true);
            }

            return true;
        }

        return false;
    }

    protected static void PrintRequirement(
        CommandTextWriter commandWriter, CommandMemberDescriptor memberDescriptor)
    {
        var label = memberDescriptor.DisplayName;
        var summary = memberDescriptor.UsageDescriptor.Summary;
        commandWriter.WriteLine(label: label, summary: summary);
    }

    protected static void PrintRequirementInDetail(
        CommandTextWriter commandWriter, CommandMemberDescriptor memberDescriptor)
    {
        var label = memberDescriptor.DisplayName;
        var description = memberDescriptor.UsageDescriptor.Description;
        commandWriter.WriteLine(label);
        commandWriter.WriteLineIndent(description, commandWriter.Indent + 1);
    }

    protected static void PrintOption(
        CommandTextWriter commandWriter, CommandMemberDescriptor memberDescriptor)
    {
        var optionString = GetOptionString(memberDescriptor);
        var summary = memberDescriptor.UsageDescriptor.Summary;
        commandWriter.WriteLine(optionString);
        commandWriter.WriteLineIndent(summary, commandWriter.Indent + 1);
    }

    protected static void PrintOptionInDetail(
        CommandTextWriter commandWriter, CommandMemberDescriptor memberDescriptor)
    {
        var optionString = GetOptionString(memberDescriptor);
        var description = memberDescriptor.UsageDescriptor.Description;
        commandWriter.WriteLine(optionString);
        commandWriter.WriteLineIndent(description, commandWriter.Indent + 1);
    }

    protected static string GetString(CommandMemberDescriptor memberDescriptor)
    {
        var displayName = memberDescriptor.DisplayName;
        if (memberDescriptor.IsRequired == true)
        {
            if (memberDescriptor.DefaultValue is DBNull)
            {
                if (memberDescriptor.IsExplicit == true)
                {
                    return $"<{displayName} 'value'>";
                }
                else
                {
                    return $"<{displayName}>";
                }
            }
            else
            {
                var value = memberDescriptor.DefaultValue ?? "null";
                return $"[{displayName} default='{value:R}']";
            }
        }
        else if (memberDescriptor.IsSwitch != true)
        {
            return $"[{displayName} 'value']";
        }
        else
        {
            return $"[{displayName}]";
        }
    }

    protected static string GetOptionString(CommandMemberDescriptor memberDescriptor)
    {
        if (memberDescriptor.IsSwitch == true)
        {
            return memberDescriptor.DisplayName;
        }
        else if (memberDescriptor.DefaultValue is not DBNull)
        {
            return $"{memberDescriptor.DisplayName} " +
                   $"[value, default='{memberDescriptor.DefaultValue:R}']";
        }
        else
        {
            return $"{memberDescriptor.DisplayName} <value>";
        }
    }

    protected static void PrintMemberSummary(
        CommandTextWriter commandWriter, CommandMemberDescriptor memberDescriptor)
    {
        var groupName = StringByName[TextSummary];
        using var _ = commandWriter.Group(groupName);
        commandWriter.WriteLine(memberDescriptor.UsageDescriptor.Summary);
    }

    protected static void PrintMemberUsage(
        CommandTextWriter commandWriter, CommandMemberDescriptor memberDescriptor)
    {
        var groupName = StringByName[TextUsage];
        using var _ = commandWriter.Group(groupName);
        commandWriter.WriteLine(GetString(memberDescriptor));
    }

    protected static void PrintMemberDescription(
        CommandTextWriter commandWriter, CommandMemberDescriptor memberDescriptor)
    {
        var groupName = StringByName[TextDescription];
        using var _ = commandWriter.Group(groupName);
        commandWriter.WriteLine(memberDescriptor.UsageDescriptor.Description);
    }

    protected static void PrintMemberExample(
        CommandTextWriter commandWriter, CommandMemberDescriptor memberDescriptor)
    {
        var groupName = StringByName[TextExample];
        using var _ = commandWriter.Group(groupName);
        commandWriter.WriteLine(memberDescriptor.UsageDescriptor.Example);
    }

    protected static void PrintMany(
        CommandTextWriter commandWriter,
        CommandMemberDescriptor[] memberDescriptors,
        Action<CommandTextWriter, CommandMemberDescriptor> printer)
    {
        PrintMany(commandWriter, memberDescriptors, printer, separatorCount: 0);
    }

    protected static void PrintMany(
        CommandTextWriter commandWriter,
        CommandMemberDescriptor[] memberDescriptors,
        Action<CommandTextWriter, CommandMemberDescriptor> printer,
        int separatorCount)
    {
        for (var i = 0; i < memberDescriptors.Length; i++)
        {
            var item = memberDescriptors[i];
            var isLast = i + 1 == memberDescriptors.Length;
            printer(commandWriter, item);
            commandWriter.WriteLinesIf(separatorCount, condition: isLast != true);
        }
    }
}
