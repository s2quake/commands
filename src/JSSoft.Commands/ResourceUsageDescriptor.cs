// <copyright file="ResourceUsageDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;
using System.Resources;

namespace JSSoft.Commands;

public class ResourceUsageDescriptor : CommandUsageDescriptorBase
{
    public const string ReferencePrefix = "&";
    public const string DescriptionPrefix = "d:";
    public const string ExamplePrefix = "e:";
    private const string Extension = ".resources";

    private static readonly Dictionary<string, ResourceManager> _resourceManagers = [];
    private readonly Type? _resourceType;
    private readonly string _resourceName;
    private readonly Type _declaredType;
    private readonly CommandMemberInfo _memberInfo;
    private string? _summary;
    private string? _description;
    private string? _example;

    public ResourceUsageDescriptor(
        CommandUsageAttribute usageAttribute, CommandMemberInfo memberInfo)
        : base(usageAttribute, memberInfo)
    {
        _resourceType = ((ResourceUsageAttribute)Attribute).ResourceType;
        _resourceName = ((ResourceUsageAttribute)Attribute).ResourceName;
        _declaredType = GetDeclaredType(memberInfo.DeclaringType, usageAttribute);
        _memberInfo = memberInfo;
    }

    public override string Summary => _summary ??= GetSummary();

    public override string Description => _description ??= GetDescription();

    public override string Example => _example ??= GetExample();

    public static string GetString(Assembly assembly, string resourceName, string id)
    {
        if (GetResourceManger(resourceName, assembly) is { } resourceManager &&
            resourceManager.GetString(id) is { } @string)
        {
            return @string;
        }

        return string.Empty;
    }

    private static string GetResourceSummary(Type type, string resourceName, string id)
    {
        var typeItem = type;
        while (typeItem is not null)
        {
            if (GetResourceManager(resourceName, typeItem) is { } resourceManager &&
                GetString(resourceManager, id) is { } @string)
            {
                return @string;
            }

            typeItem = typeItem.BaseType;
        }

        return string.Empty;
    }

    private static string GetResourceDescription(Type type, string resourceName, string id)
    {
        var typeItem = type;
        while (typeItem is not null)
        {
            if (GetResourceManager(resourceName, type) is { } resourceManager &&
                GetString(resourceManager, $"{DescriptionPrefix}{id}") is { } @string)
            {
                return @string;
            }

            typeItem = typeItem.BaseType;
        }

        return string.Empty;
    }

    private static string GetResourceExample(Type type, string resourceName, string id)
    {
        var typeItem = type;
        while (typeItem is not null)
        {
            if (GetResourceManager(resourceName, typeItem) is { } resourceManager &&
                GetString(resourceManager, $"{ExamplePrefix}{id}") is { } @string)
            {
                return @string;
            }

            typeItem = typeItem.BaseType;
        }

        return string.Empty;
    }

    private static ResourceManager? GetResourceManager(string resourceName, Type resourceType)
    {
        if (resourceType.FullName is null)
        {
            var message = $"Property '{nameof(Type.FullName)}' of '{nameof(Type)}' cannot be null.";
            throw new ArgumentException(message, nameof(resourceType));
        }

        var resourceNames = resourceType.Assembly.GetManifestResourceNames();
        var baseName = resourceName == string.Empty ? resourceType.FullName : resourceName;

        if (resourceNames.Contains(baseName + Extension) is false)
        {
            return null;
        }

        if (_resourceManagers.TryGetValue(baseName, out var value) is false)
        {
            value = new ResourceManager(baseName, resourceType.Assembly);
            _resourceManagers.Add(baseName, value);
        }

        return value;
    }

    private static ResourceManager? GetResourceManger(string resourceName, Assembly assembly)
    {
        var resourceNames = assembly.GetManifestResourceNames();
        var baseName = resourceName;

        if (resourceNames.Contains(baseName + Extension) is false)
        {
            return null;
        }

        if (_resourceManagers.TryGetValue(baseName, out var value) is false)
        {
            value = new ResourceManager(baseName, assembly);
            _resourceManagers.Add(baseName, value);
        }

        return value;
    }

    private static string? GetString(ResourceManager resourceManager, string id)
    {
        if (resourceManager.GetString(id) is { } text)
        {
            if (text.StartsWith(ReferencePrefix) is true
                && resourceManager.GetString(text[ReferencePrefix.Length..]) is { } referenceText)
            {
                return referenceText;
            }

            return text;
        }

        return null;
    }

    private static Type GetDeclaredType(Type type, Attribute attribute)
    {
        var typeItem = type;
        while (typeItem is not null)
        {
            if (System.Attribute.IsDefined(typeItem, attribute.GetType(), inherit: false) is true)
            {
                return typeItem;
            }

            typeItem = typeItem.BaseType;
        }

        throw new UnreachableException();
    }

    private string GetSummary()
    {
        var resourceName = _resourceName;
        var resourceType = _resourceType ?? _declaredType;
        var id = _memberInfo.Identifier;
        return GetResourceSummary(resourceType, resourceName, id);
    }

    private string GetDescription()
    {
        var resourceName = _resourceName;
        var resourceType = _resourceType ?? _declaredType;
        var id = _memberInfo.Identifier;
        return GetResourceDescription(resourceType, resourceName, id);
    }

    private string GetExample()
    {
        var resourceName = _resourceName;
        var resourceType = _resourceType ?? _declaredType;
        var id = _memberInfo.Identifier;
        return GetResourceExample(resourceType, resourceName, id);
    }
}
