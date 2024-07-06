// <copyright file="ICommandUsage.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public interface ICommandUsage
{
    string ExecutionName { get; }

    string Summary { get; }

    string Description { get; }

    string Example { get; }
}
