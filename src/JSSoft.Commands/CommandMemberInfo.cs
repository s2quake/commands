// <copyright file="CommandMemberInfo.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using static JSSoft.Commands.AttributeUtility;

namespace JSSoft.Commands;

public sealed record class CommandMemberInfo
{
    public CommandMemberInfo(Type type)
    {
        Value = type;
        Type = CommandMemberType.Type;
        Name = type.Name;
        FullName = $"{type.Namespace}.{type.Name}";
        DeclaringType = type;
    }

    public CommandMemberInfo(PropertyInfo propertyInfo)
    {
        if (propertyInfo.DeclaringType is null)
        {
            throw new ArgumentException(
                $"{propertyInfo} does not have a declaring type.", nameof(propertyInfo));
        }

        Value = propertyInfo;
        Type = CommandMemberType.Property;
        Name = $"{GetName(propertyInfo.DeclaringType)}.{propertyInfo.Name}";
        FullName = $"{GetFullName(propertyInfo.DeclaringType)}.{propertyInfo.Name}";
        DeclaringType = propertyInfo.DeclaringType;
    }

    public CommandMemberInfo(MethodInfo methodInfo)
    {
        if (methodInfo.DeclaringType is null)
        {
            throw new ArgumentException(
                $"{methodInfo} does not have a declaring type.", nameof(methodInfo));
        }

        Value = methodInfo;
        Type = CommandMemberType.Method;
        Name = $"{GetName(methodInfo.DeclaringType)}.{methodInfo.Name}";
        FullName = $"{GetFullName(methodInfo.DeclaringType)}.{methodInfo.Name}";
        DeclaringType = methodInfo.DeclaringType;
    }

    public CommandMemberInfo(ParameterInfo parameterInfo)
    {
        if (parameterInfo.Member is not MethodInfo)
        {
            throw new ArgumentException(
                $"{parameterInfo.Member} is not a method.", nameof(parameterInfo));
        }

        if (parameterInfo.Member.DeclaringType is null)
        {
            throw new ArgumentException(
                $"{parameterInfo.Member} does not have a declaring type.", nameof(parameterInfo));
        }

        if (parameterInfo.Name is null)
        {
            throw new ArgumentException(
                $"{parameterInfo.Member} does not have a name.", nameof(parameterInfo));
        }

        Value = parameterInfo;
        Type = CommandMemberType.Parameter;
        Name = $"{GetName(parameterInfo.Member.DeclaringType)}." +
               $"{parameterInfo.Member.Name}.{parameterInfo.Name}";
        FullName = $"{GetFullName(parameterInfo.Member.DeclaringType)}." +
               $"{parameterInfo.Member.Name}.{parameterInfo.Name}";
        DeclaringType = parameterInfo.Member.DeclaringType;
    }

    public object Value { get; }

    public CommandMemberType Type { get; }

    public string Name { get; }

    public string FullName { get; }

    public string Namespace => DeclaringType.Namespace ?? string.Empty;

    public Type DeclaringType { get; }

    public string Summary
        => GetAttributeValue<CommandSummaryAttribute>(attr => attr.Summary, string.Empty);

    public string Description
        => GetAttributeValue<DescriptionAttribute>(attr => attr.Description, string.Empty);

    public string Example
        => GetAttributeValue<CommandExampleAttribute>(attr => attr.Example, string.Empty);

    public bool IsBrowsable
        => GetAttributeValue<BrowsableAttribute, bool>(attr => attr.Browsable, true);

    public static implicit operator CommandMemberInfo(Type type)
        => new(type);

    public static implicit operator CommandMemberInfo(PropertyInfo propertyInfo)
        => new(propertyInfo);

    public static implicit operator CommandMemberInfo(MethodInfo methodInfo)
        => new(methodInfo);

    public static implicit operator CommandMemberInfo(ParameterInfo parameterInfo)
        => new(parameterInfo);

    public override string? ToString() => $"[{Type}] {FullName}";

    public CommandMemberInfo? GetParent()
    {
        return Value switch
        {
            PropertyInfo propertyInfo => new(propertyInfo.DeclaringType!),
            MethodInfo methodInfo => new(methodInfo.DeclaringType!),
            ParameterInfo parameterInfo => new((MethodInfo)parameterInfo.Member),
            _ => null,
        };
    }

    public T? GetAttribute<T>()
        where T : Attribute
        => Value switch
        {
            Type type => GetCustomAttribute<T>(type),
            PropertyInfo propertyInfo => GetCustomAttribute<T>(propertyInfo),
            MethodInfo methodInfo => GetCustomAttribute<T>(methodInfo),
            ParameterInfo parameterInfo => GetCustomAttribute<T>(parameterInfo),
            _ => default,
        };

    public T? GetAttribute<T>(bool inherit)
        where T : Attribute
        => Value switch
        {
            Type type => GetCustomAttribute<T>(type, inherit),
            PropertyInfo propertyInfo => GetCustomAttribute<T>(propertyInfo, inherit),
            MethodInfo methodInfo => GetCustomAttribute<T>(methodInfo, inherit),
            ParameterInfo parameterInfo => GetCustomAttribute<T>(parameterInfo, inherit),
            _ => default,
        };

    public T[] GetAttributes<T>()
        where T : Attribute
        => Value switch
        {
            Type type => GetCustomAttributes<T>(type),
            PropertyInfo propertyInfo => GetCustomAttributes<T>(propertyInfo),
            MethodInfo methodInfo => GetCustomAttributes<T>(methodInfo),
            ParameterInfo parameterInfo => GetCustomAttributes<T>(parameterInfo),
            _ => [],
        };

    public T[] GetAttributes<T>(bool inherit)
        where T : Attribute
        => Value switch
        {
            Type type => GetCustomAttributes<T>(type, inherit),
            PropertyInfo propertyInfo => GetCustomAttributes<T>(propertyInfo, inherit),
            MethodInfo methodInfo => GetCustomAttributes<T>(methodInfo, inherit),
            ParameterInfo parameterInfo => GetCustomAttributes<T>(parameterInfo, inherit),
            _ => [],
        };

    public bool HasAttribute<T>()
        where T : Attribute
        => Value switch
        {
            Type type => IsDefined<T>(type),
            PropertyInfo propertyInfo => IsDefined<T>(propertyInfo),
            MethodInfo methodInfo => IsDefined<T>(methodInfo),
            ParameterInfo parameterInfo => IsDefined<T>(parameterInfo),
            _ => false,
        };

    public bool HasAttribute<T>(bool inherit)
        where T : Attribute
        => Value switch
        {
            Type type => IsDefined<T>(type, inherit),
            PropertyInfo propertyInfo => IsDefined<T>(propertyInfo, inherit),
            MethodInfo methodInfo => IsDefined<T>(methodInfo, inherit),
            ParameterInfo parameterInfo => IsDefined<T>(parameterInfo, inherit),
            _ => false,
        };

    public TV GetAttributeValue<TA, TV>(Func<TA, TV> getter, TV defaultValue)
        where TA : Attribute
        => Value switch
        {
            Type type => GetValue(type, getter, defaultValue),
            PropertyInfo propertyInfo => GetValue(propertyInfo, getter, defaultValue),
            MethodInfo methodInfo => GetValue(methodInfo, getter, defaultValue),
            ParameterInfo parameterInfo => GetValue(parameterInfo, getter, defaultValue),
            _ => defaultValue,
        };

    public string GetAttributeValue<TA>(Func<TA, string> getter, string defaultValue)
        where TA : Attribute
        => Value switch
        {
            Type type => GetValue(type, getter, defaultValue),
            PropertyInfo propertyInfo => GetValue(propertyInfo, getter, defaultValue),
            MethodInfo methodInfo => GetValue(methodInfo, getter, defaultValue),
            ParameterInfo parameterInfo => GetValue(parameterInfo, getter, defaultValue),
            _ => defaultValue,
        };

    private static string GetFullName(Type type) => $"{type.Namespace}.{type.Name}";

    private static string GetName(Type type) => $"{type.Name}";
}
