// <copyright file="CommandPropertyNotSupportedTypeException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Exceptions;

internal sealed class CommandPropertyNotSupportedTypeException(PropertyInfo propertyInfo)
    : CommandDefinitionException(
        message: $"Property '{propertyInfo}' of '{propertyInfo.DeclaringType}' " +
                 $"is an unsupported type.",
        memberInfo: propertyInfo)
{
}
