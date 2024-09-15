// <copyright file="CommandParsingUsagePrinter.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;

namespace JSSoft.Commands;

public class CommandParsingUsagePrinter(ICommandUsage commandUsage, CommandSettings settings)
    : CommandUsagePrinterBase(settings with { })
{
    public virtual void Print(
        TextWriter writer, CommandMemberDescriptorCollection memberDescriptors)
    {
        using var commandWriter = new CommandTextWriter(writer, Settings);
        if (IsDetail is true)
        {
            PrintSummary(commandWriter, commandUsage.Summary);
            PrintUsage(commandWriter, commandUsage.ExecutionName, memberDescriptors);
            PrintDescription(commandWriter, commandUsage.Description);
            PrintExample(commandWriter, commandUsage.Example);
            PrintRequirementsInDetail(commandWriter, memberDescriptors);
            PrintVariablesInDetail(commandWriter, memberDescriptors);
            PrintOptionsInDetail(commandWriter, memberDescriptors);
        }
        else
        {
            PrintSummary(commandWriter, commandUsage.Summary);
            PrintUsage(commandWriter, commandUsage.ExecutionName, memberDescriptors);
            PrintExample(commandWriter, commandUsage.Example);
            PrintRequirements(commandWriter, memberDescriptors);
            PrintVariables(commandWriter, memberDescriptors);
            PrintOptions(commandWriter, memberDescriptors, settings.CategoryPredicate);
        }
    }

    public virtual void Print(TextWriter writer, CommandMemberDescriptor memberDescriptor)
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
}
