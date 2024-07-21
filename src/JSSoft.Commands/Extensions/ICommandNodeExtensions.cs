// <copyright file="ICommandNodeExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace JSSoft.Commands.Extensions;

public static class ICommandNodeExtensions
{
    public static bool TryGetCommand(
        this ICommandNode @this,
        string commandName,
        [MaybeNullWhen(false)] out ICommandNode commandNode)
    {
        return @this.Children.TryGetValue(commandName, out commandNode) == true
            || @this.ChildByAlias.TryGetValue(commandName, out commandNode) == true;
    }

    public static bool TryGetEnabledCommand(
        this ICommandNode @this,
        string commandName,
        [MaybeNullWhen(false)] out ICommandNode commandNode)
    {
        return @this.TryGetCommand(commandName, out commandNode) == true
            && commandNode.IsEnabled == true;
    }
}
