// <copyright file="CommandMemberDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

// Reflection should not be used to increase accessibility of classes, methods, or fields
#pragma warning disable S3011

using System.Diagnostics;
using System.Threading.Tasks;

namespace JSSoft.Commands;

public abstract class CommandMemberDescriptor
{
    protected CommandMemberDescriptor(CommandPropertyBaseAttribute attribute, string memberName)
    {
        Attribute = attribute;
        MemberName = memberName;
        Name = attribute.GetName(defaultName: memberName);
        ShortName = attribute.ShortName;
        IsRequired = attribute is CommandPropertyRequiredAttribute
                        || attribute is CommandPropertyExplicitRequiredAttribute;
        IsExplicit = attribute is CommandPropertyExplicitRequiredAttribute
                        || attribute is CommandPropertyAttribute
                        || attribute is CommandPropertySwitchAttribute;
        IsSwitch = attribute is CommandPropertySwitchAttribute;
        IsVariables = attribute is CommandPropertyArrayAttribute;
        IsGeneral = attribute is CommandPropertyAttribute;
        DefaultValue = GetDefaultValue(attribute);
        InitValue = GetInitValue(attribute);
        DisplayName = GenerateDisplayName(this);
    }

    public string Name { get; }

    public char ShortName { get; }

    public virtual string DisplayName { get; }

    public virtual object? InitValue { get; }

    public virtual object? DefaultValue { get; }

    public bool IsRequired { get; }

    public bool IsExplicit { get; }

    public bool IsSwitch { get; }

    public bool IsVariables { get; }

    public bool IsGeneral { get; }

    public abstract bool IsNullable { get; }

    public abstract Type MemberType { get; }

    public string MemberName { get; }

    public abstract CommandUsageDescriptorBase UsageDescriptor { get; }

    protected CommandPropertyBaseAttribute Attribute { get; }

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
        var type = attribute.StaticType ?? instance.GetType();
        var obj = attribute.StaticType is not null ? null : instance;
        var flag = attribute.StaticType is not null ? BindingFlags.Static : BindingFlags.Instance;
        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | flag;
        var method = type.GetMethod(methodName, bindingFlags, null, [], null);
        if (method is null)
        {
            throw new ArgumentException($"Cannot found method '{methodName}'.", nameof(attribute));
        }

        try
        {
            var arrayType = MemberType.MakeArrayType();
            var value = method.Invoke(obj, null);
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

    private static object GetDefaultValue(CommandPropertyBaseAttribute attribute)
    {
        if (attribute is CommandPropertyExplicitRequiredAttribute explicitAttribute)
        {
            return explicitAttribute.DefaultValue;
        }

        if (attribute is CommandPropertyRequiredAttribute requiredAttribute)
        {
            return requiredAttribute.DefaultValue;
        }

        if (attribute is CommandPropertyAttribute generalAttribute)
        {
            return generalAttribute.DefaultValue;
        }

        return DBNull.Value;
    }

    private static object GetInitValue(CommandPropertyBaseAttribute attribute)
    {
        if (attribute is CommandPropertyArrayAttribute arrayAttribute)
        {
            return arrayAttribute.InitValue;
        }

        if (attribute is CommandPropertyAttribute generalAttribute)
        {
            return generalAttribute.InitValue;
        }

        return DBNull.Value;
    }
}
