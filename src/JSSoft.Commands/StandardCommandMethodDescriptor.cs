// <copyright file="StandardCommandMethodDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;
using System.Threading.Tasks;
using JSSoft.Commands.Exceptions;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

internal sealed class StandardCommandMethodDescriptor : CommandMethodDescriptor
{
    private readonly PropertyInfo? _validationPropertyInfo;
    private readonly MethodInfo? _completionMethodInfo;

    public StandardCommandMethodDescriptor(MethodInfo methodInfo)
        : base(methodInfo)
    {
        if (methodInfo.DeclaringType is null)
        {
            throw new CommandDeclaringTypeNullException(methodInfo);
        }

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

    public override string MethodName { get; }

    public override string Name { get; }

    public override string[] Aliases { get; }

    public override string DisplayName { get; }

    public override string Category { get; }

    public override bool IsAsync { get; }

    public override CommandMemberDescriptorCollection Members { get; }

    protected override object? OnInvoke(object instance, object?[] parameters)
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

    protected override bool OnCanExecute(object instance)
    {
        if (_validationPropertyInfo?.GetValue(instance) is bool @bool)
        {
            return @bool;
        }

        return base.OnCanExecute(instance);
    }

    protected override string[] GetCompletion(object instance, object?[] parameters)
    {
        if (_completionMethodInfo is not null)
        {
            return InvokeCompletionMethod(instance, parameters);
        }

        return base.GetCompletion(instance, parameters);
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
