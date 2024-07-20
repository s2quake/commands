// <copyright file="CommandUsageAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;

namespace JSSoft.Commands;

[AttributeUsage(
    validOn: AttributeTargets.Class | AttributeTargets.Method
        | AttributeTargets.Property | AttributeTargets.Parameter)]
public class CommandUsageAttribute : Attribute
{
    public CommandUsageAttribute(string usageDescriptorTypeName)
    {
        try
        {
            TypeUtility.ThrowIfTypeIsNotSubclassOf(
                typeName: usageDescriptorTypeName,
                baseType: typeof(CommandUsageDescriptorBase));
            TypeUtility.ThrowIfTypeDoesNotHavePublicConstructor(
                typeName: usageDescriptorTypeName,
                argumentTypes: [typeof(CommandUsageAttribute), typeof(object)]);
        }
        catch (Exception e)
        {
            Trace.TraceWarning(e.Message);
        }

        UsageDescriptorType = Type.GetType(usageDescriptorTypeName)!;
    }

    public CommandUsageAttribute(Type usageDescriptorType)
    {
        try
        {
            TypeUtility.ThrowIfTypeIsNotSubclassOf(
                type: usageDescriptorType,
                baseType: typeof(CommandUsageDescriptorBase));
            TypeUtility.ThrowIfTypeDoesNotHavePublicConstructor(
                type: usageDescriptorType,
                argumentTypes: [typeof(CommandUsageAttribute), typeof(object)]);
        }
        catch (Exception e)
        {
            Trace.TraceWarning(e.Message);
        }

        UsageDescriptorType = usageDescriptorType;
    }

    internal CommandUsageAttribute()
    {
        UsageDescriptorType = typeof(CommandUsageDescriptor);
    }

    public Type UsageDescriptorType { get; }
}
