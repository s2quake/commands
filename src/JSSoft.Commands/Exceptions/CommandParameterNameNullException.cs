// <copyright file="CommandParameterNameNullException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Exceptions;

internal sealed class CommandParameterNameNullException : CommandDefinitionException
{
    public CommandParameterNameNullException(ParameterInfo parameterInfo)
        : this(memberInfo: parameterInfo)
    {
    }

    private CommandParameterNameNullException(CommandMemberInfo memberInfo)
        : base(
            message: $"'{memberInfo}' does not have a name.",
            memberInfo: memberInfo)
    {
    }
}
