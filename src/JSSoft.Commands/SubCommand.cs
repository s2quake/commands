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

namespace JSSoft.Commands;

sealed class SubCommand(CommandMethodBase commandMethod, CommandMethodDescriptor methodDescriptor)
    : ICommand, ICommandCompleter, IExecutable, ICommandUsage, ICommandUsagePrinter, ICustomCommandDescriptor
{
    public string Name => methodDescriptor.Name;

    public string[] Aliases => methodDescriptor.Aliases;

    public CommandSettings Settings => commandMethod.CommandContext.Settings;

    public CommandMemberDescriptorCollection GetMembers() => methodDescriptor.Members;

    public object GetMemberOwner(CommandMemberDescriptor memberDescriptor)
    {
        return commandMethod;
    }

    public void Execute()
    {
        methodDescriptor.Invoke(commandMethod, methodDescriptor.Members);
    }

    public string[] GetCompletions(CommandCompletionContext completionContext)
    {
        return commandMethod.GetCompletions(methodDescriptor, completionContext.MemberDescriptor, completionContext.Find);
    }

    #region ICommand

    bool ICommand.IsEnabled => methodDescriptor.CanExecute(commandMethod);

    #endregion

    #region ICommandUsagePrinter

    void ICommandUsagePrinter.Print(bool isDetail)
    {
        var settings = Settings;
        var usagePrinter = new CommandInvocationUsagePrinter(this, settings)
        {
            IsDetail = isDetail,
        };
        usagePrinter.Print(commandMethod.Out, methodDescriptor);
    }

    #endregion

    #region ICommandUsage

    string ICommandUsage.ExecutionName => $"{commandMethod.ExecutionName} {CommandUtility.GetExecutionName(Name, Aliases)}";

    string ICommandUsage.Summary => methodDescriptor.UsageDescriptor.Summary;

    string ICommandUsage.Description => methodDescriptor.UsageDescriptor.Description;

    string ICommandUsage.Example => methodDescriptor.UsageDescriptor.Example;

    #endregion
}
