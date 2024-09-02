// <copyright file="CommandStaticTypeAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

public abstract class CommandStaticTypeAttribute : Attribute
{
    protected CommandStaticTypeAttribute()
        => StaticTypeName = string.Empty;

    protected CommandStaticTypeAttribute(string staticTypeName)
        => StaticTypeName = staticTypeName;

    protected CommandStaticTypeAttribute(Type staticType)
        => StaticTypeName = staticType.AssemblyQualifiedName ?? string.Empty;

    public string StaticTypeName { get; }

    internal Type GetTypeWithFallback(CommandMemberInfo memberInfo)
    {
        if (Type.GetType(StaticTypeName) is { } type && type.IsStaticClass() is true)
        {
            return type;
        }

        return memberInfo.DeclaringType;
    }

    internal Type GetStaticType(CommandMemberInfo memberInfo)
    {
        if (Type.GetType(StaticTypeName) is { } type && type.IsStaticClass() is true)
        {
            return type;
        }

        throw new CommandDefinitionException(
            $"'{StaticTypeName}' is not a static class.", memberInfo);
    }

    internal PropertyInfo GetPropertyInfo(CommandMemberInfo memberInfo, string propertyName)
    {
        var type = GetTypeWithFallback(memberInfo);
        var bindingFlags = CommandSettings.GetBindingFlags(type);

        return type.GetProperty(propertyName, bindingFlags)
            ?? throw new CommandDefinitionException(
                $"'{propertyName}' property not found in '{type.FullName}'.", memberInfo);
    }

    internal MethodInfo GetMethodInfo(CommandMemberInfo memberInfo, string methodName)
    {
        var type = GetTypeWithFallback(memberInfo);
        var bindingFlags = CommandSettings.GetBindingFlags(type);

        return type.GetMethod(methodName, bindingFlags)
            ?? throw new CommandDefinitionException(
                $"'{methodName}' method not found in '{type.FullName}'.", memberInfo);
    }
}
