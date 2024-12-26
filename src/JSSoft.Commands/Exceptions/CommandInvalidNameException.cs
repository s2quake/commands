// <copyright file="CommandInvalidNameException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Exceptions;

internal sealed class CommandInvalidNameException(CommandMemberInfo memberInfo, string name)
    : CommandDefinitionException(
        message: $"'{name}' is not a name that matches the regular expression pattern " +
                     $"'{CommandUtility.NamePattern}'.",
        memberInfo: memberInfo)
{
}
