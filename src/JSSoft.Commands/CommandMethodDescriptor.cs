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

    public CommandUsageDescriptorBase UsageDescriptor { get; }
        = CommandDescriptor.GetUsageDescriptor(methodInfo);

    internal bool CanExecute(object instance) => OnCanExecute(instance);

    internal object? Invoke(
        object instance, string[] args, CommandMemberDescriptorCollection memberDescriptors)
    {
        var parseContext = new ParseContext(memberDescriptors, args);
        var parameterInfos = MethodInfo.GetParameters();
        var valueList = new List<object?>(parameterInfos.Length);
        parseContext.SetValue(instance);
        foreach (var parameterInfo in parameterInfos)
        {
            var memberDescriptor = memberDescriptors[parameterInfo.Name!];
            var value = memberDescriptor.GetValueInternal(instance);
            valueList.Add(value);
        }

        return OnInvoke(instance, [.. valueList]);
    }

    internal object? Invoke(object instance, CommandMemberDescriptorCollection memberDescriptors)
    {
        var parameterInfos = MethodInfo.GetParameters();
        var valueList = new List<object?>(parameterInfos.Length);
        foreach (var parameterInfo in parameterInfos)
        {
            var memberDescriptor = memberDescriptors[parameterInfo.Name!];
            var value = memberDescriptor.GetValueInternal(instance);
            valueList.Add(value);
        }

        return OnInvoke(instance, [.. valueList]);
    }

    internal Task InvokeAsync(
        object instance,
        string[] args,
        CommandMemberDescriptorCollection memberDescriptors,
        CancellationToken cancellationToken,
        IProgress<ProgressInfo> progress)
    {
        var parseContext = new ParseContext(memberDescriptors, args);
        var parameterInfos = MethodInfo.GetParameters();
        var valueList = new List<object?>(parameterInfos.Length);
        parseContext.SetValue(instance);
        foreach (var parameterInfo in parameterInfos)
        {
            if (parameterInfo.ParameterType == typeof(CancellationToken))
            {
                valueList.Add(cancellationToken);
            }
            else if (CommandMethodUtility.IsProgressParameter(parameterInfo) == true)
            {
                valueList.Add(new CommandProgress(progress));
            }
            else
            {
                var memberDescriptor = memberDescriptors[parameterInfo.Name!];
                var value = memberDescriptor.GetValueInternal(instance);
                valueList.Add(value);
            }
        }

        if (OnInvoke(instance, [.. valueList]) is Task task)
        {
            return task;
        }

        throw new UnreachableException();
    }

    internal Task InvokeAsync(
        object instance,
        CommandMemberDescriptorCollection memberDescriptors,
        CancellationToken cancellationToken,
        IProgress<ProgressInfo> progress)
    {
        var parameterInfos = MethodInfo.GetParameters();
        var valueList = new List<object?>(parameterInfos.Length);
        foreach (var parameterInfo in parameterInfos)
        {
            if (parameterInfo.ParameterType == typeof(CancellationToken))
            {
                valueList.Add(cancellationToken);
            }
            else if (CommandMethodUtility.IsProgressParameter(parameterInfo) == true)
            {
                valueList.Add(new CommandProgress(progress));
            }
            else
            {
                var memberDescriptor = memberDescriptors[parameterInfo.Name!];
                var value = memberDescriptor.GetValueInternal(instance);
                valueList.Add(value);
            }
        }

        if (OnInvoke(instance, [.. valueList]) is Task task)
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
