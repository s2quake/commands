// <copyright file="ResourceManagerExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Resources;

namespace JSSoft.Commands.Extensions;

public static class ResourceManagerExtensions
{
    public const string ReferencePrefix = "&";
    public const string DescriptionPrefix = "d:";
    public const string ExamplePrefix = "e:";

    public static string GetSummary(this ResourceManager @this, string id)
        => GetString(@this, id) ?? string.Empty;

    public static string GetDescription(this ResourceManager @this, string id)
        => GetString(@this, $"{DescriptionPrefix}{id}") ?? string.Empty;

    public static string GetExample(this ResourceManager @this, string id)
        => GetString(@this, $"{ExamplePrefix}{id}") ?? string.Empty;

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
}
