// <copyright file="CommandMethodDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands;

public abstract class CommandMethodDescriptor
{
    protected CommandMethodDescriptor(MethodInfo methodInfo)
    {
        MethodInfo = methodInfo;
        UsageDescriptor = CommandDescriptor.GetUsageDescriptor(methodInfo);
    }

    public abstract string Name { get; }

    public abstract string[] Aliases { get; }

    public abstract string DisplayName { get; }

    public abstract string MethodName { get; }

    public abstract bool IsAsync { get; }

    public abstract CommandMemberDescriptorCollection Members { get; }

    public abstract string Category { get; }

    public MethodInfo MethodInfo { get; }

    public CommandUsageDescriptorBase UsageDescriptor { get; }

    protected abstract object? OnInvoke(object instance, object?[] parameters);

    protected virtual bool OnCanExecute(object instance)
    {
        return true;
    }

    protected virtual string[] GetCompletion(object instance, object?[] parameters)
    {
        return [];
    }

    internal bool CanExecute(object instance)
    {
        return OnCanExecute(instance);
    }

    internal object? Invoke(object instance, string[] args, CommandMemberDescriptorCollection memberDescriptors)
    {
        var parseContext = new ParseContext(memberDescriptors, args);
        var parameters = MethodInfo.GetParameters();
        var valueList = new List<object?>(parameters.Length);
        parseContext.SetValue(instance);
        foreach (var item in parameters)
        {
            var memberDescriptor = memberDescriptors[item.Name!];
            var value = memberDescriptor.GetValueInternal(instance);
            valueList.Add(value);
        }
        return OnInvoke(instance, [.. valueList]);
    }

    internal Task InvokeAsync(object instance, string[] args, CommandMemberDescriptorCollection memberDescriptors, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        var parseContext = new ParseContext(memberDescriptors, args);
        var parameters = MethodInfo.GetParameters();
        var valueList = new List<object?>(parameters.Length);
        parseContext.SetValue(instance);
        foreach (var item in parameters)
        {
            if (item.ParameterType == typeof(CancellationToken))
            {
                valueList.Add(cancellationToken);
            }
            else if (CommandMethodUtility.IsProgressParameter(item) == true)
            {
                valueList.Add(new CommandProgress(progress));
            }
            else
            {
                var memberDescriptor = memberDescriptors[item.Name!];
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

    internal object? Invoke(object instance, CommandMemberDescriptorCollection memberDescriptors)
    {
        var parameters = MethodInfo.GetParameters();
        var valueList = new List<object?>(parameters.Length);
        foreach (var item in parameters)
        {
            var memberDescriptor = memberDescriptors[item.Name!];
            var value = memberDescriptor.GetValueInternal(instance);
            valueList.Add(value);
        }
        return OnInvoke(instance, [.. valueList]);
    }

    internal Task InvokeAsync(object instance, CommandMemberDescriptorCollection memberDescriptors, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        var parameters = MethodInfo.GetParameters();
        var valueList = new List<object?>(parameters.Length);
        foreach (var item in parameters)
        {
            if (item.ParameterType == typeof(CancellationToken))
            {
                valueList.Add(cancellationToken);
            }
            else if (CommandMethodUtility.IsProgressParameter(item) == true)
            {
                valueList.Add(new CommandProgress(progress));
            }
            else
            {
                var memberDescriptor = memberDescriptors[item.Name!];
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

    internal string[] GetCompletionInternal(object instance, CommandMemberDescriptor memberDescriptor, string find)
    {
        if (memberDescriptor.GetCompletionInternal(instance, find) is string[] items)
            return items;
        return GetCompletion(instance, [memberDescriptor, find]);
    }
}
