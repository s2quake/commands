// <copyright file="SubCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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
