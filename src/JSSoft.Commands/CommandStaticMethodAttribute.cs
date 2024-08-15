// <copyright file="CommandStaticMethodAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class CommandStaticMethodAttribute : CommandStaticTypeAttribute
{
    public CommandStaticMethodAttribute(string staticTypeName, params string[] methodNames)
        : base(staticTypeName)
    {
        MethodNames = methodNames;
    }

    public CommandStaticMethodAttribute(Type staticType, params string[] methodNames)
        : base(staticType)
    {
        MethodNames = methodNames;
    }

    public string[] MethodNames { get; }
}
