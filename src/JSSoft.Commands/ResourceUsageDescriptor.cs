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
    private readonly Type _declaringType;

    public ResourceUsageDescriptor(CommandUsageAttribute usageAttribute, object target)
        : base(usageAttribute, target)
    {
        _resourceType = ((ResourceUsageAttribute)Attribute).ResourceType;
        _resourceName = ((ResourceUsageAttribute)Attribute).ResourceName;
        if (target is Type type)
        {
            _declaringType = GetDeclaringType(type, usageAttribute);
            Summary = GetSummary();
            Description = GetDescription();
            Example = GetExample();
        }
        else if (target is PropertyInfo propertyInfo)
        {
            ThrowUtility.ThrowIfDeclaringTypeNull(propertyInfo);
            _declaringType = GetDeclaringType(propertyInfo.DeclaringType!, usageAttribute);
            Summary = GetSummary(propertyInfo);
            Description = GetDescription(propertyInfo);
            Example = string.Empty;
        }
        else if (target is MethodInfo methodInfo)
        {
            ThrowUtility.ThrowIfDeclaringTypeNull(methodInfo);

            _declaringType = GetDeclaringType(methodInfo.DeclaringType!, usageAttribute);
            Summary = GetSummary(methodInfo);
            Description = GetDescription(methodInfo);
            Example = GetExample(methodInfo);
        }
        else if (target is ParameterInfo parameterInfo)
        {
            ThrowUtility.ThrowIfParameterInfoNameNull(parameterInfo);
            ThrowUtility.ThrowIfDeclaringTypeNull(parameterInfo);

            _declaringType = GetDeclaringType(parameterInfo.Member.DeclaringType!, usageAttribute);
            Summary = GetSummary(parameterInfo);
            Description = GetDescription(parameterInfo);
            Example = string.Empty;
        }
        else
        {
            throw new UnreachableException();
        }
    }

    public override string Summary { get; }

    public override string Description { get; }

    public override string Example { get; }

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
        ThrowUtility.ThrowIfTypeFullNameIsNull(resourceType, paramName: nameof(resourceName));

        var resourceNames = resourceType.Assembly.GetManifestResourceNames();
        var baseName = resourceName == string.Empty ? resourceType.FullName! : resourceName;

        if (resourceNames.Contains(baseName + Extension) != true)
        {
            return null;
        }

        if (_resourceManagers.TryGetValue(baseName, out var value) != true)
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

        if (resourceNames.Contains(baseName + Extension) != true)
        {
            return null;
        }

        if (_resourceManagers.TryGetValue(baseName, out var value) != true)
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
            if (text.StartsWith(ReferencePrefix) == true
                && resourceManager.GetString(text[ReferencePrefix.Length..]) is { } referenceText)
            {
                return referenceText;
            }

            return text;
        }

        return null;
    }

    private static Type GetDeclaringType(Type type, Attribute attribute)
    {
        var typeItem = type;
        while (typeItem is not null)
        {
            if (System.Attribute.IsDefined(typeItem, attribute.GetType(), inherit: false) == true)
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
        var resourceType = _resourceType ?? _declaringType;
        var id = _declaringType.Name;
        return GetResourceSummary(resourceType, resourceName, id);
    }

    private string GetSummary(PropertyInfo propertyInfo)
    {
        var resourceName = _resourceName;
        var resourceType = _resourceType ?? _declaringType;
        var id = $"{propertyInfo.DeclaringType!.Name}.{propertyInfo.Name}";
        return GetResourceSummary(resourceType, resourceName, id);
    }

    private string GetSummary(MethodInfo methodInfo)
    {
        var resourceName = _resourceName;
        var resourceType = _resourceType ?? _declaringType;
        var id = $"{methodInfo.DeclaringType!.Name}.{methodInfo.Name}";
        return GetResourceSummary(resourceType, resourceName, id);
    }

    private string GetSummary(ParameterInfo parameterInfo)
    {
        ThrowUtility.ThrowIfDeclaringTypeNull(parameterInfo.Member);
        ThrowUtility.ThrowIfParameterInfoNameNull(parameterInfo);

        var resourceName = _resourceName;
        var resourceType = _resourceType ?? _declaringType;
        string[] items =
        [
            parameterInfo.Member.DeclaringType!.Name,
            parameterInfo.Member.Name,
            parameterInfo.Name!,
        ];
        var id = string.Join(".", items);
        return GetResourceSummary(resourceType, resourceName, id);
    }

    private string GetDescription()
    {
        var resourceName = _resourceName;
        var resourceType = _resourceType ?? _declaringType;
        var id = _declaringType.Name;
        return GetResourceDescription(resourceType, resourceName, id);
    }

    private string GetDescription(PropertyInfo propertyInfo)
    {
        var resourceName = _resourceName;
        var resourceType = _resourceType ?? _declaringType;
        var id = $"{propertyInfo.DeclaringType!.Name}.{propertyInfo.Name}";
        return GetResourceDescription(resourceType, resourceName, id);
    }

    private string GetDescription(ParameterInfo parameterInfo)
    {
        ThrowUtility.ThrowIfDeclaringTypeNull(parameterInfo.Member);
        ThrowUtility.ThrowIfParameterInfoNameNull(parameterInfo);

        var resourceName = _resourceName;
        var resourceType = _resourceType ?? _declaringType;
        string[] items =
        [
            parameterInfo.Member.DeclaringType!.Name,
            parameterInfo.Member.Name,
            parameterInfo.Name!,
        ];
        var id = string.Join(".", items);
        return GetResourceDescription(resourceType, resourceName, id);
    }

    private string GetDescription(MethodInfo methodInfo)
    {
        var resourceName = _resourceName;
        var resourceType = _resourceType ?? _declaringType;
        var id = $"{methodInfo.DeclaringType!.Name}.{methodInfo.Name}";
        return GetResourceDescription(resourceType, resourceName, id);
    }

    private string GetExample()
    {
        var resourceName = _resourceName;
        var resourceType = _resourceType ?? _declaringType;
        var id = _declaringType.Name;
        return GetResourceExample(resourceType, resourceName, id);
    }

    private string GetExample(MethodInfo methodInfo)
    {
        var resourceName = _resourceName;
        var resourceType = _resourceType ?? _declaringType;
        var id = $"{methodInfo.DeclaringType!.Name}.{methodInfo.Name}";
        return GetResourceExample(resourceType, resourceName, id);
    }
}
