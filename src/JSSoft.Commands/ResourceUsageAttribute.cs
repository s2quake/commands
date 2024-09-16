// <copyright file="ResourceUsageAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Resources;
using JSSoft.Commands.Extensions;
using static JSSoft.Commands.ResourceManagerUtility;

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Class)]
public class ResourceUsageAttribute : CommandUsageAttribute
{
    public ResourceUsageAttribute()
    {
    }

    public ResourceUsageAttribute(string resourceTypeName)
    {
        ResourceTypeName = resourceTypeName;
        ResourceType = Type.GetType(resourceTypeName)!;
    }

    public ResourceUsageAttribute(Type resourceType)
    {
        ResourceType = resourceType;
        ResourceTypeName = resourceType.AssemblyQualifiedName ?? string.Empty;
    }

    public string ResourceTypeName { get; } = string.Empty;

    public Type? ResourceType { get; }

    public string ResourceName { get; set; } = string.Empty;

    public override CommandUsage GetUsage(CommandMemberInfo memberInfo)
    {
        var resourceManager = GetResourceManager(this, memberInfo);
        var identifier = GetIdentifier(resourceManager, memberInfo);

        return new CommandUsage
        {
            Summary = resourceManager.GetSummary(identifier),
            Description = resourceManager.GetDescription(identifier),
            Example = resourceManager.GetExample(identifier),
        };
    }

    private static string GetIdentifier(
        ResourceManager resourceManager, CommandMemberInfo memberInfo)
    {
        if (memberInfo.Type == CommandMemberType.Type
            && resourceManager.BaseName != memberInfo.FullName)
        {
            var items = resourceManager.BaseName.Split('.');
            return items[^1];
        }

        return memberInfo.Identifier;
    }
}
