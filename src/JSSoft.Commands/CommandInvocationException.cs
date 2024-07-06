// <copyright file="CommandInvocationException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public class CommandInvocationException(CommandInvoker invoker, CommandInvocationError error, string[] args, Exception? innerException)
    : Exception(innerException?.Message, innerException), ICommandUsage
{
    private readonly CommandUsageDescriptorBase _usageDescriptor = CommandDescriptor.GetUsageDescriptor(invoker.Instance.GetType());

    public CommandInvocationException(CommandInvoker invoker, CommandInvocationError error, string[] args)
        : this(invoker, error, args, innerException: null)
    {
    }

    public CommandInvocationError Error { get; } = error;

    public string[] Arguments { get; } = args;

    public CommandInvoker Invoker { get; } = invoker;

    public CommandMethodDescriptorCollection MethodDescriptors => CommandDescriptor.GetMethodDescriptors(Invoker.Instance.GetType());

    #region ICommandUsage

    string ICommandUsage.ExecutionName => Invoker.ExecutionName;

    string ICommandUsage.Summary => _usageDescriptor.Summary;

    string ICommandUsage.Description => _usageDescriptor.Description;

    string ICommandUsage.Example => _usageDescriptor.Example;

    #endregion
}
