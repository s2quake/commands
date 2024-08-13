// <copyright file="CommandParameterNotSupportedTypeException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Exceptions;

internal sealed class CommandParameterNotSupportedTypeException(ParameterInfo parameterInfo)
    : CommandDefinitionException(
        message: $"Parameter '{parameterInfo}' of '{parameterInfo.Member}' is an unsupported type.",
        memberInfo: parameterInfo)
{
}
