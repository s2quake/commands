// <copyright file="CommandMethodCompletionAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Method)]
public class CommandMethodCompletionAttribute : CommandStaticTypeAttribute
{
    public CommandMethodCompletionAttribute(string methodName)
    {
        MethodName = methodName;
    }

    public CommandMethodCompletionAttribute(string staticTypeName, string methodName)
        : base(staticTypeName)
    {
        MethodName = methodName;
    }

    public CommandMethodCompletionAttribute(Type staticType, string methodName)
        : base(staticType)
    {
        MethodName = methodName;
    }

    public string MethodName { get; }
}
