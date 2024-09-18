// <copyright file="GlobalSettings.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Parse;

[ResourceUsage]
internal static class GlobalSettings
{
    [CommandProperty]
    public static string Id { get; set; } = string.Empty;

    [CommandProperty]
    public static string Password { get; set; } = string.Empty;
}
