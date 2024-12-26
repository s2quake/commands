// <copyright file="CommandInvalidShortNameException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Exceptions;

internal sealed class CommandInvalidShortNameException(CommandMemberInfo memberInfo, char value)
    : CommandDefinitionException(
        message: $"'{value}' is an invalid short name. Only alphabetic characters can be used.",
        memberInfo: memberInfo)
{
}
