// <copyright file="CommandParsingUsagePrinter.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;

namespace JSSoft.Commands;

public class CommandParsingUsagePrinter(ICommandUsage commandUsage, CommandSettings settings)
    : CommandUsagePrinterBase(commandUsage, settings)
{
    public virtual void Print(
        TextWriter writer, CommandMemberDescriptorCollection memberDescriptors)
    {
        using var commandWriter = new CommandTextWriter(writer, Settings);
        if (IsDetail == true)
        {
            PrintSummary(commandWriter, CommandUsage.Summary);
            PrintUsage(commandWriter, CommandUsage.ExecutionName, memberDescriptors);
            PrintDescription(commandWriter, CommandUsage.Description);
            PrintExample(commandWriter, CommandUsage.Example);
            PrintRequirementsInDetail(commandWriter, memberDescriptors);
            PrintVariablesInDetail(commandWriter, memberDescriptors);
            PrintOptionsInDetail(commandWriter, memberDescriptors);
        }
        else
        {
            PrintSummary(commandWriter, CommandUsage.Summary);
            PrintUsage(commandWriter, CommandUsage.ExecutionName, memberDescriptors);
            PrintExample(commandWriter, CommandUsage.Example);
            PrintRequirements(commandWriter, memberDescriptors);
            PrintVariables(commandWriter, memberDescriptors);
            PrintOptions(commandWriter, memberDescriptors);
        }
    }

    public virtual void Print(TextWriter writer, CommandMemberDescriptor memberDescriptor)
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
}
