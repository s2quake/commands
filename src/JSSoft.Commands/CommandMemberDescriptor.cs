// <copyright file="CommandMemberDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;
using System.Threading.Tasks;
using JSSoft.Commands.Exceptions;

namespace JSSoft.Commands;

public abstract class CommandMemberDescriptor(
    CommandMemberInfo memberInfo,
    CommandMemberBaseAttribute attribute,
    string memberName)
{
    private string? _displayName;

    public CommandMemberInfo MemberInfo { get; } = memberInfo;

    public string Name { get; } = attribute.GetName(memberInfo, defaultName: memberName);

    public char ShortName { get; } = attribute.GetShortName(memberInfo);

    public string DisplayName => _displayName ??= GenerateDisplayName(this);

    public abstract object? InitValue { get; }

    public abstract object? DefaultValue { get; }

    public abstract bool IsRequired { get; }

    public abstract bool IsExplicit { get; }

    public abstract bool IsSwitch { get; }

    public abstract bool IsVariables { get; }

    public abstract bool IsGeneral { get; }

    public abstract bool IsNullable { get; }

    public abstract Type MemberType { get; }

    public string MemberName { get; } = memberName;

    public abstract string Category { get; }

    public abstract CommandUsage Usage { get; }

    protected CommandMemberBaseAttribute Attribute { get; } = attribute;

    public override string ToString() => $"{MemberName} [{DisplayName}]";

    internal void SetValueInternal(object instance, object? value) => SetValue(instance, value);

    internal object? GetValueInternal(object instance) => GetValue(instance);

    internal void VerifyTrigger(ParseDescriptorCollection parseDescriptors)
        => OnVerifyTrigger(parseDescriptors);

    internal string[] GetCompletionsInternal(object instance) => GetCompletions(instance);

    protected abstract void SetValue(object instance, object? value);

    protected abstract object? GetValue(object instance);

    protected virtual void OnVerifyTrigger(ParseDescriptorCollection parseDescriptors)
    {
    }

    protected virtual string[] GetCompletions(object instance) => [];

    protected string[] GetCompletions(
        object instance, CommandMemberCompletionAttribute attribute)
    {
        var declaringType = MemberInfo.DeclaringType;
        var methodName = attribute.MethodName;
        var parameterTypes = Array.Empty<Type>();
        var methodInfo = attribute.GetMethodInfo(
            MemberInfo.DeclaringType, methodName, parameterTypes);
        var obj = declaringType.IsInstanceOfType(instance) ? instance : null;

        try
        {
            var arrayType = MemberType.MakeArrayType();
            var value = methodInfo.Invoke(obj, null);
            if (value?.GetType() == arrayType && value is Array @array)
            {
                var itemList = new List<string>(@array.Length);
                foreach (var item in @array)
                {
                    itemList.Add($"{item}");
                }

                return [.. itemList];
            }
            else if (value is string[] items)
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

            throw new NotSupportedException();
        }
        catch (Exception e)
        {
            Trace.TraceError($"{e}");
            return [];
        }
    }

    private static string GenerateDisplayName(CommandMemberDescriptor memberDescriptor)
    {
        var memberInfo = memberDescriptor.MemberInfo;
        if (memberInfo.TryGetDisplayName(out var displayName) is true)
        {
            return displayName;
        }

        if (memberDescriptor.IsVariables is true)
        {
            return $"{memberDescriptor.Name}...";
        }

        var itemList = new List<string>(2)
        {
            GetNamePattern(),
            GetShortNamePattern(),
        };
        if (memberDescriptor.Attribute.AllowName is true)
        {
            itemList.Reverse();
        }

        return string.Join(" | ", itemList.Where(item => item != string.Empty));

        string GetNamePattern()
        {
            if (memberDescriptor.Name == string.Empty)
            {
                return string.Empty;
            }

            if (memberDescriptor.IsExplicit is true)
            {
                return CommandUtility.Delimiter + memberDescriptor.Name;
            }

            return memberDescriptor.Name;
        }

        string GetShortNamePattern()
        {
            if (memberDescriptor.ShortName == char.MinValue)
            {
                return string.Empty;
            }

            if (memberDescriptor.IsExplicit is true)
            {
                return CommandUtility.ShortDelimiter + memberDescriptor.ShortName;
            }

            return $"{memberDescriptor.ShortName}";
        }
    }
}
