// <copyright file="ResourceManagerUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;
using System.Resources;

namespace JSSoft.Commands;

public static class ResourceManagerUtility
{
    public const string ReferencePrefix = "&";
    public const string DescriptionPrefix = "d:";
    public const string ExamplePrefix = "e:";
    private const string Extension = ".resources";

    private static readonly Dictionary<string, ResourceManager> _resourceManagers = [];

    public static ResourceManager GetResourceManager(
        ResourceUsageAttribute attribute, CommandMemberInfo memberInfo)
    {
        if (attribute.ResourceType is not null)
        {
            if (attribute.ResourceType.FullName is null)
            {
                throw new ArgumentException(
                    $"ResourceType does not have a full name.", nameof(attribute));
            }

            var assembly = attribute.ResourceType.Assembly;
            var resourceName = attribute.ResourceName == string.Empty
                ? attribute.ResourceType.FullName : attribute.ResourceName;
            return GetResourceManager(assembly, resourceName);
        }
        else if (attribute.ResourceName != string.Empty)
        {
            var resourceType = GetDeclaredType(memberInfo.DeclaringType, attribute);
            var resourceName = attribute.ResourceName;
            var resourceAssembly = resourceType.Assembly;
            var resourceManager = GetResourceManager(resourceAssembly, resourceName);
            return resourceManager;
        }
        else
        {
            if (memberInfo.DeclaringType.FullName is null)
            {
                var items = new string[]
                {
                    nameof(CommandMemberInfo),
                    nameof(CommandMemberInfo.DeclaringType),
                    nameof(Type.FullName),
                };
                throw new UnreachableException(
                    $"{string.Join('.', items)} cannot be null.");
            }

            var resourceType = GetDeclaredType(memberInfo.DeclaringType, attribute);
            var resourceName = resourceType.FullName
                ?? throw new UnreachableException("The resource type does not have a full name.");
            var resourceAssembly = resourceType.Assembly;
            var resourceManager = GetResourceManager(resourceAssembly, resourceName);
            return resourceManager;
        }
    }

    public static ResourceManager GetResourceManager(Assembly assembly, string resourceName)
    {
        var resourceNames = assembly.GetManifestResourceNames();
        var baseName = resourceName;

        if (resourceNames.Contains(baseName + Extension) is false)
        {
            throw new ArgumentException($"Resource '{resourceName}' ", nameof(resourceName));
        }

        if (_resourceManagers.TryGetValue(baseName, out var value) is false)
        {
            value = new ResourceManager(baseName, assembly);
            _resourceManagers.Add(baseName, value);
        }

        return value;
    }

    private static Type GetDeclaredType(Type type, ResourceUsageAttribute attribute)
    {
        var typeItem = type;
        while (typeItem is not null)
        {
            var attributeType = typeof(ResourceUsageAttribute);
            var attributes = Attribute.GetCustomAttributes(typeItem, attributeType, inherit: false);
            if (attributes.Contains(attribute) is true)
            {
                return typeItem;
            }

            typeItem = typeItem.BaseType;
        }

        var message = $"The attribute '{nameof(ResourceUsageAttribute)}' is not declared in " +
                      $"the type '{type}'.";
        throw new ArgumentException(message, nameof(attribute));
    }
}
