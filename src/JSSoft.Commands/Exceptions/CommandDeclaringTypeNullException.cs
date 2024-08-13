// <copyright file="CommandDeclaringTypeNullException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Exceptions;

internal sealed class CommandDeclaringTypeNullException : CommandDefinitionException
{
    public CommandDeclaringTypeNullException(CommandMemberInfo memberInfo)
        : base($"{memberInfo}' does not have a declaring type.", memberInfo)
    {
    }
}
