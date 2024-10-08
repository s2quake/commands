// <copyright file="CommandMethodPropertyAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class CommandMethodPropertyAttribute(params string[] propertyNames) : Attribute
{
    public string[] PropertyNames { get; } = propertyNames;
}
