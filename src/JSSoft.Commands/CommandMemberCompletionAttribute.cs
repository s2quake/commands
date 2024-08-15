// <copyright file="CommandMemberCompletionAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public abstract class CommandMemberCompletionAttribute : CommandStaticTypeAttribute
{
    protected CommandMemberCompletionAttribute(string methodName)
    {
        MethodName = methodName;
    }

    protected CommandMemberCompletionAttribute(string staticTypeName, string methodName)
        : base(staticTypeName)
    {
        MethodName = methodName;
    }

    protected CommandMemberCompletionAttribute(Type staticType, string methodName)
        : base(staticType)
    {
        MethodName = methodName;
    }

    public string MethodName { get; }
}
