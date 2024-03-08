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

namespace JSSoft.Commands;

public class CommandParsingUsagePrinter(ICommandUsage commandUsage, CommandSettings settings)
    : CommandUsagePrinterBase(commandUsage, settings)
{
    public virtual void Print(TextWriter writer, CommandMemberDescriptorCollection memberDescriptors)
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
