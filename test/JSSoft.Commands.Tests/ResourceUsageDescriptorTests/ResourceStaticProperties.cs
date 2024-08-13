// <copyright file="ResourceStaticProperties.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.ResourceUsageDescriptorTests;

[ResourceUsage]
internal static class ResourceStaticProperties
{
    [CommandProperty]
    public static string Id { get; set; } = string.Empty;

    [CommandProperty]
    public static string Password { get; set; } = string.Empty;
}
