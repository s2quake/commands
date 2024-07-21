// <copyright file="CommandPropertyCompletionAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Property)]
public sealed class CommandPropertyCompletionAttribute : CommandMemberCompletionAttribute
{
    public CommandPropertyCompletionAttribute(string methodName)
        : base(methodName)
    {
    }

    public CommandPropertyCompletionAttribute(string staticTypeName, string methodName)
        : base(staticTypeName, methodName)
    {
    }

    public CommandPropertyCompletionAttribute(Type staticType, string methodName)
        : base(staticType, methodName)
    {
    }
}
