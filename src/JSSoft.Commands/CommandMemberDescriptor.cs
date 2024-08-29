// <copyright file="CommandMemberDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;
using System.Threading.Tasks;

namespace JSSoft.Commands;

public abstract class CommandMemberDescriptor
{
    private string? _displayName;

    protected CommandMemberDescriptor(
        CommandMemberInfo memberInfo,
        CommandMemberBaseAttribute attribute,
        string memberName)
    {
        Owner = memberInfo;
        Attribute = attribute;
        MemberName = memberName;
        Name = attribute.GetName(memberInfo, defaultName: memberName);
        ShortName = attribute.GetShortName(memberInfo);
    }

    public CommandMemberInfo Owner { get; }

    public string Name { get; }

    public char ShortName { get; }

    public string DisplayName => _displayName ??= GenerateDisplayName(this);

    public abstract object? InitValue { get; }

    public abstract object? DefaultValue { get; }

    public abstract bool IsRequired { get; }

    public abstract bool IsExplicit { get; }

    public abstract bool IsSwitch { get; }

    public abstract bool IsVariables { get; }

    public abstract bool IsGeneral { get; }

    public abstract bool IsUnhandled { get; }

    public abstract bool IsNullable { get; }

    public abstract Type MemberType { get; }

    public string MemberName { get; }

    public abstract CommandUsageDescriptorBase UsageDescriptor { get; }

    protected CommandMemberBaseAttribute Attribute { get; }

    public override string ToString() => $"{MemberName} [{DisplayName}]";

    internal void SetValueInternal(object instance, object? value) => SetValue(instance, value);

    internal object? GetValueInternal(object instance) => GetValue(instance);

    internal void VerifyTrigger(ParseDescriptorCollection parseDescriptors)
        => OnVerifyTrigger(parseDescriptors);

    internal string[]? GetCompletionInternal(object instance, string find)
    {
        if (GetCompletion(instance, find) is { } items)
        {
            var query = from item in items
                        where item.StartsWith(find)
                        select item;
            return query.ToArray();
        }

        return null;
    }

    protected abstract void SetValue(object instance, object? value);

    protected abstract object? GetValue(object instance);

    protected virtual void OnVerifyTrigger(ParseDescriptorCollection parseDescriptors)
    {
    }

    protected virtual string[]? GetCompletion(object instance, string find) => null;

    protected string[] GetCompletion(
        object instance, string find, CommandMemberCompletionAttribute attribute)
    {
        var methodName = attribute.MethodName;
        var methodInfo = attribute.GetMethodInfo(instance.GetType(), methodName);
        var obj = methodInfo.DeclaringType == instance.GetType() ? instance : null;

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
                if (task.Wait(CommandSettings.AsyncTimeout) != true)
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
        var memberInfo = memberDescriptor.Owner;
        if (memberInfo.TryGetDisplayName(out var displayName) == true)
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
        if (memberDescriptor.Attribute.AllowName == true)
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

            if (memberDescriptor.IsExplicit == true)
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

            if (memberDescriptor.IsExplicit == true)
            {
                return CommandUtility.ShortDelimiter + memberDescriptor.ShortName;
            }

            return $"{memberDescriptor.ShortName}";
        }
    }
}
