// <copyright file="ICommandHierarchyExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace JSSoft.Commands.Extensions;

public static class ICommandHierarchyExtensions
{
    public static bool TryGetCommand(
        this ICommandHierarchy @this,
        string commandName,
        [MaybeNullWhen(false)] out ICommand command)
    {
        return @this.Commands.TryGetValue(commandName, out command) == true;
    }
}
