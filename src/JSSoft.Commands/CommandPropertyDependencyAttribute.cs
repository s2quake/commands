// <copyright file="CommandPropertyDependencyAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class CommandPropertyDependencyAttribute(string propertyName)
    : Attribute
{
    public string PropertyName { get; } = propertyName;
}
