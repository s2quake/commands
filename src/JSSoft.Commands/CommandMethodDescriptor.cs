// <copyright file="CommandMethodDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using JSSoft.Commands.Exceptions;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

public sealed class CommandMethodDescriptor
{
    private readonly PropertyInfo? _validationPropertyInfo;
    private readonly MethodInfo? _completionMethodInfo;

    public CommandMethodDescriptor(MethodInfo methodInfo)
    {
        if (methodInfo.DeclaringType is null)
        {
            throw new CommandDeclaringTypeNullException(methodInfo);
        }

        MethodInfo = methodInfo;
        Usage = CommandDescriptor.GetUsage(methodInfo);
        MethodName = methodInfo.Name;
        IsAsync = methodInfo.IsAsync();
        Name = methodInfo.GetName();
        Aliases = methodInfo.GetAliases();
        DisplayName = methodInfo.GetDisplayName();
        Members = CommandDescriptor.GetMemberDescriptorsByMethodInfo(methodInfo);
        Category = AttributeUtility.GetCategory(methodInfo);
        _validationPropertyInfo = methodInfo.GetValidationPropertyInfo();
        _completionMethodInfo = methodInfo.GetCompletionMethodInfo();
    }

    public string Name { get; }

    public string[] Aliases { get; }

    public string DisplayName { get; }

    public string MethodName { get; }

    public bool IsAsync { get; }

    public CommandMemberDescriptorCollection Members { get; }

    public string Category { get; }

    public MethodInfo MethodInfo { get; }

    public CommandUsage Usage { get; }

    internal bool CanExecute(object instance) => OnCanExecute(instance);

    internal object? Invoke(object instance, CommandMethodInstance methodInstance)
    {
        var parameters = methodInstance.GetParameters();
        if (MethodInfo.DeclaringType!.IsAbstract && MethodInfo.DeclaringType.IsSealed is true)
        {
            return MethodInfo.Invoke(null, parameters);
        }
        else
        {
            return MethodInfo.Invoke(instance, parameters);
        }
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

    internal string[] GetCompletionsInternal(
        object instance, CommandMemberDescriptor memberDescriptor)
    {
        if (memberDescriptor.GetCompletionsInternal(instance) is string[] items)
        {
            return items;
        }

        return GetCompletions(instance, [memberDescriptor]);
    }

    private object? OnInvoke(object instance, object?[] parameters)
    {
        if (MethodInfo.DeclaringType!.IsAbstract && MethodInfo.DeclaringType.IsSealed is true)
        {
            return MethodInfo.Invoke(null, parameters);
        }
        else
        {
            return MethodInfo.Invoke(instance, parameters);
        }
    }

    private bool OnCanExecute(object instance)
    {
        if (_validationPropertyInfo?.GetValue(instance) is bool @bool)
        {
            return @bool;
        }

        return true;
    }

    private string[] GetCompletions(object instance, object?[] parameters)
    {
        if (_completionMethodInfo is not null)
        {
            return InvokeCompletionMethod(instance, parameters);
        }

        return [];
    }

    private string[] InvokeCompletionMethod(object instance, object?[] parameters)
    {
        try
        {
            var value = _completionMethodInfo!.Invoke(instance, parameters);
            if (value is string[] items)
            {
                return items;
            }
            else if (value is Task<string[]> task)
            {
                if (task.Wait(CommandSettings.AsyncTimeout) is false)
                {
                    return [];
                }

                return task.Result;
            }

            throw new UnreachableException();
        }
        catch (Exception e)
        {
            Trace.TraceError($"{e}");
            return [];
        }
    }
}
