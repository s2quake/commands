// <copyright file="CommandParameterCompletionAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class CommandParameterCompletionAttribute : CommandMemberCompletionAttribute
{
    public CommandParameterCompletionAttribute(string methodName)
        : base(methodName)
    {
    }

    public CommandParameterCompletionAttribute(string staticTypeName, string methodName)
        : base(staticTypeName, methodName)
    {
    }

    public CommandParameterCompletionAttribute(Type staticType, string methodName)
        : base(staticType, methodName)
    {
    }
}
