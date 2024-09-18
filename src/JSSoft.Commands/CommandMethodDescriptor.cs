// <copyright file="CommandMethodDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands;

public abstract class CommandMethodDescriptor(MethodInfo methodInfo)
{
    public abstract string Name { get; }

    public abstract string[] Aliases { get; }

    public abstract string DisplayName { get; }

    public abstract string MethodName { get; }

    public abstract bool IsAsync { get; }

    public abstract CommandMemberDescriptorCollection Members { get; }

    public abstract string Category { get; }

    public MethodInfo MethodInfo { get; } = methodInfo;

    public CommandUsage Usage { get; } = CommandDescriptor.GetUsage(methodInfo);

    internal bool CanExecute(object instance) => OnCanExecute(instance);

    internal object? Invoke(object instance, CommandMethodInstance methodInstance)
    {
        var parameters = methodInstance.GetParameters();
        return OnInvoke(instance, parameters);
    }

    internal Task InvokeAsync(
        object instance,
        CommandMethodInstance methodInstance,
        CancellationToken cancellationToken,
        IProgress<ProgressInfo> progress)
    {
        var parameters = methodInstance.GetParameters(cancellationToken, progress);

        if (OnInvoke(instance, parameters) is Task task)
        {
            return task;
        }

        throw new UnreachableException();
    }

    internal string[] GetCompletionInternal(
        object instance, CommandMemberDescriptor memberDescriptor, string find)
    {
        if (memberDescriptor.GetCompletionInternal(instance, find) is string[] items)
        {
            return items;
        }

        return GetCompletion(instance, [memberDescriptor, find]);
    }

    protected abstract object? OnInvoke(object instance, object?[] parameters);

    protected virtual bool OnCanExecute(object instance) => true;

    protected virtual string[] GetCompletion(object instance, object?[] parameters) => [];
}
