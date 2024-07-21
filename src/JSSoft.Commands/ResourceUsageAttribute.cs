// <copyright file="ResourceUsageAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Class)]
public class ResourceUsageAttribute : CommandUsageAttribute
{
    public ResourceUsageAttribute()
        : base(typeof(ResourceUsageDescriptor))
    {
    }

    public ResourceUsageAttribute(string resourceTypeName)
        : base(typeof(ResourceUsageDescriptor))
    {
        ResourceTypeName = resourceTypeName;
        ResourceType = Type.GetType(resourceTypeName)!;
    }

    public ResourceUsageAttribute(Type resourceType)
        : base(typeof(ResourceUsageDescriptor))
    {
        ResourceType = resourceType;
        ResourceTypeName = resourceType.AssemblyQualifiedName!;
    }

    public string ResourceTypeName { get; } = string.Empty;

    public Type? ResourceType { get; }

    public string ResourceName { get; set; } = string.Empty;
}
