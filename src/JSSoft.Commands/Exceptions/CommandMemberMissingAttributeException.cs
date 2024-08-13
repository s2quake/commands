// <copyright file="CommandMemberMissingAttributeException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Exceptions;

internal sealed class CommandMemberMissingAttributeException : CommandDefinitionException
{
    public CommandMemberMissingAttributeException(MethodInfo methodInfo, Type attributeType)
        : base(
            message: $"Method '{methodInfo}' of '{methodInfo.DeclaringType}' " +
                 $"does not have a {attributeType} attribute.",
            memberInfo: methodInfo)
    {
    }

    public CommandMemberMissingAttributeException(PropertyInfo propertyInfo, Type attributeType)
        : base(
            message: $"Property '{propertyInfo}' of '{propertyInfo.DeclaringType}' " +
                 $"does not have a {attributeType} attribute.",
            memberInfo: propertyInfo)
    {
    }
}
