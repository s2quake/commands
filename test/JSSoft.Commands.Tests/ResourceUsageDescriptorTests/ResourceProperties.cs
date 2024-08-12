// <copyright file="ResourceProperties.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.ResourceUsageDescriptorTests;

[ResourceUsage]
internal class ResourceProperties
{
    [CommandProperty]
    public string Id { get; set; } = string.Empty;

    [CommandProperty]
    public string Password { get; set; } = string.Empty;
}
