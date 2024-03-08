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

using System.IO;
using System.Text;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

public class CommandInvocationUsagePrinter(ICommandUsage commandUsage, CommandSettings settings)
    : CommandUsagePrinterBase(commandUsage, settings)
{
    public virtual void Print(TextWriter writer, CommandMethodDescriptor[] methodDescriptors)
    {
        using var commandWriter = new CommandTextWriter(writer, Settings);
        if (IsDetail == true)
        {
            PrintSummary(commandWriter, CommandUsage.Summary);
            PrintUsage(commandWriter, CommandUsage.ExecutionName);
            PrintDescription(commandWriter, CommandUsage.Description);
            PrintExample(commandWriter, CommandUsage.Example);
            PrintCommandsInDetail(commandWriter, methodDescriptors);
        }
        else
        {
            PrintSummary(commandWriter, CommandUsage.Summary);
            PrintUsage(commandWriter, CommandUsage.ExecutionName);
            PrintExample(commandWriter, CommandUsage.Example);
            PrintCommands(commandWriter, methodDescriptors);
        }
    }

    public virtual void Print(TextWriter writer, CommandMethodDescriptor methodDescriptor)
    {
        using var commandWriter = new CommandTextWriter(writer, Settings);
        if (IsDetail == true)
        {
            PrintMethodSummary(commandWriter, methodDescriptor);
            PrintUsage(commandWriter, CommandUsage.ExecutionName, methodDescriptor.Members);
            PrintMethodDescription(commandWriter, methodDescriptor);
            PrintMethodExample(commandWriter, methodDescriptor);
            PrintRequirementsInDetail(commandWriter, methodDescriptor);
            PrintVariablesInDetail(commandWriter, methodDescriptor);
            PrintOptionsInDetail(commandWriter, methodDescriptor);
        }
        else
        {
            PrintMethodSummary(commandWriter, methodDescriptor);
            PrintUsage(commandWriter, CommandUsage.ExecutionName, methodDescriptor.Members);
            PrintMethodExample(commandWriter, methodDescriptor);
            PrintRequirements(commandWriter, methodDescriptor);
            PrintVariables(commandWriter, methodDescriptor);
            PrintOptions(commandWriter, methodDescriptor);
        }
    }

    public virtual void Print(TextWriter writer, CommandMethodDescriptor methodDescriptor, CommandMemberDescriptor memberDescriptor)
    {
        using var commandWriter = new CommandTextWriter(writer, Settings);
        if (IsDetail == true)
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

    protected static void PrintMethodSummary(CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        var groupName = StringByName[TextSummary];
        using var _ = commandWriter.Group(groupName);
        var summary = methodDescriptor.UsageDescriptor.Summary;
        var width = commandWriter.Width - commandWriter.TotalIndentSpaces;
        var lines = CommandTextWriter.Wrap(summary, width);
        commandWriter.WriteLine(lines);
    }

    protected static void PrintMethodDescription(CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        var groupName = StringByName[TextDescription];
        using var _ = commandWriter.Group(groupName);
        var description = methodDescriptor.UsageDescriptor.Description;
        var width = commandWriter.Width - commandWriter.TotalIndentSpaces;
        var lines = CommandTextWriter.Wrap(description, width);
        commandWriter.WriteLine(lines);
    }

    protected static void PrintMethodExample(CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        var groupName = StringByName[TextExample];
        using var _ = commandWriter.Group(groupName);
        var example = methodDescriptor.UsageDescriptor.Example;
        commandWriter.WriteLine(example);
    }

    protected static void PrintCommands(CommandTextWriter commandWriter, CommandMethodDescriptor[] methodDescriptors)
    {
        var query = from item in methodDescriptors
                    orderby item.Name
                    orderby item.Category
                    group item by item.Category into @group
                    select @group;

        foreach (var @group in query)
        {
            var itemList = new List<string>
            {
                @group.Key,
                StringByName[TextCommands],
            };
            var groupName = CommandUtility.Join(" ", itemList);
            using var _ = commandWriter.Group(groupName);
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

    protected static void PrintCommandsInDetail(CommandTextWriter commandWriter, CommandMethodDescriptor[] methodDescriptors)
    {
        var query = from item in methodDescriptors
                    orderby item.Name
                    orderby item.Category
                    group item by item.Category into @group
                    select @group;

        foreach (var @group in query)
        {
            var itemList = new List<string>
            {
                @group.Key,
                StringByName[TextCommands],
            };
            var groupName = CommandUtility.Join(" ", itemList);
            using var _ = commandWriter.Group(groupName);
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
                commandWriter.WriteLineIf(condition: isLast == false);
            }
        }
    }

    protected static void PrintUsage(CommandTextWriter commandWriter, string executionName)
    {
        var groupName = StringByName[TextUsage];
        using var _ = commandWriter.Group(groupName);
        commandWriter.WriteLine($"{executionName} <command> [options...]");
    }

    protected static bool PrintRequirements(CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        var memberDescriptors = methodDescriptor.Members;
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

    protected static bool PrintRequirementsInDetail(CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        var memberDescriptors = methodDescriptor.Members;
        if (memberDescriptors.HasRequirements == true)
        {
            var requirementDescriptors = memberDescriptors.RequirementDescriptors;
            var groupName = StringByName[TextRequirements];
            using var _ = commandWriter.Group(groupName);
            PrintMany(commandWriter, requirementDescriptors, PrintRequirementInDetail, separatorCount: 1);
            return true;
        }
        return false;
    }

    protected static bool PrintVariables(CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        if (methodDescriptor.Members.VariablesDescriptor is { } variablesDescriptor)
        {
            var groupName = StringByName[TextVariables];
            using var _ = commandWriter.Group(groupName);
            PrintVariables(commandWriter, variablesDescriptor);
            return true;
        }
        return false;
    }

    protected static bool PrintVariablesInDetail(CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        if (methodDescriptor.Members.VariablesDescriptor is { } variablesDescriptor)
        {
            var groupName = StringByName[TextVariables];
            using var _ = commandWriter.Group(groupName);
            PrintVariablesInDetail(commandWriter, variablesDescriptor);
            return true;
        }
        return false;
    }

    protected static bool PrintOptions(CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        var memberDescriptors = methodDescriptor.Members;
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

    protected static bool PrintOptionsInDetail(CommandTextWriter commandWriter, CommandMethodDescriptor methodDescriptor)
    {
        var memberDescriptors = methodDescriptor.Members;
        if (memberDescriptors.HasOptions == true)
        {
            var optionDescriptors = memberDescriptors.OptionDescriptors;
            var groupName = StringByName[TextOptions];
            using var _ = commandWriter.Group(groupName);
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
