// <copyright file="CommandUsage.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public sealed record class CommandUsage
{
    public string ExecutionName { get; init; } = string.Empty;

    public string Summary { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string Example { get; init; } = string.Empty;
}
