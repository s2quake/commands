// <copyright file="CommandUsageAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Parameter)]
public class CommandUsageAttribute : Attribute
{
    internal CommandUsageAttribute()
    {
        UsageDescriptorType = typeof(CommandUsageDescriptor);
    }

    public CommandUsageAttribute(string usageDescriptorTypeName)
    {
        try
        {
            TypeUtility.ThrowIfTypeIsNotSubclassOf(usageDescriptorTypeName, typeof(CommandUsageDescriptorBase));
            TypeUtility.ThrowIfTypeDoesNotHavePublicConstructor(usageDescriptorTypeName, [typeof(CommandUsageAttribute), typeof(object)]);
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
            TypeUtility.ThrowIfTypeIsNotSubclassOf(usageDescriptorType, typeof(CommandUsageDescriptorBase));
            TypeUtility.ThrowIfTypeDoesNotHavePublicConstructor(usageDescriptorType, [typeof(CommandUsageAttribute), typeof(object)]);
        }
        catch (Exception e)
        {
            Trace.TraceWarning(e.Message);
        }

        UsageDescriptorType = usageDescriptorType;
    }

    public Type UsageDescriptorType { get; }
}
