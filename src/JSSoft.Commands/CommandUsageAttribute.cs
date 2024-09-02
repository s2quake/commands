// <copyright file="CommandUsageAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(
    validOn: AttributeTargets.Class | AttributeTargets.Method
        | AttributeTargets.Property | AttributeTargets.Parameter)]
public class CommandUsageAttribute : Attribute
{
    public CommandUsageAttribute(string usageDescriptorTypeName)
        => UsageDescriptorTypeName = usageDescriptorTypeName;

    public CommandUsageAttribute(Type usageDescriptorType)
        => UsageDescriptorTypeName = usageDescriptorType.AssemblyQualifiedName ?? string.Empty;

    internal CommandUsageAttribute()
        => UsageDescriptorTypeName
            = typeof(CommandUsageDescriptor).AssemblyQualifiedName ?? string.Empty;

    public string UsageDescriptorTypeName { get; } = string.Empty;

    internal Type GetUsageDescriptorType(CommandMemberInfo memberInfo)
    {
        if (Type.GetType(UsageDescriptorTypeName) is not { } type)
        {
            var message = $"'{UsageDescriptorTypeName}' is not a valid type name.";
            throw new CommandDefinitionException(message, memberInfo);
        }

        if (typeof(CommandUsageDescriptorBase).IsAssignableFrom(type) is false)
        {
            var message = $"Type '{type}' is not subclass of " +
                          $"'{typeof(CommandUsageDescriptorBase)}'.";
            throw new CommandDefinitionException(message, memberInfo);
        }

        var argumentTypes = new Type[] { typeof(CommandUsageAttribute), typeof(CommandMemberInfo) };
        if (type.GetConstructor(argumentTypes) is null)
        {
            var args = string.Join(", ", argumentTypes.Select(item => item.Name));
            var message = $"Type '{type}' does not have a public constructor with args ({args}).";
            throw new CommandDefinitionException(message, memberInfo);
        }

        return type;
    }
}
