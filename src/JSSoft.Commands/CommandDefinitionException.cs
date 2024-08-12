// <copyright file="CommandDefinitionException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public class CommandDefinitionException : SystemException
{
    public CommandDefinitionException(string message)
        : base(message)
    {
    }

    public CommandDefinitionException(string message, CommandMemberInfo memberInfo)
        : base(message)
    {
        Source = memberInfo.ToString();
    }

    public CommandDefinitionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public CommandDefinitionException(
        string message, CommandMemberInfo memberInfo, Exception innerException)
        : base(message, innerException)
    {
        Source = memberInfo.ToString();
    }
}
