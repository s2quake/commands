// <copyright file="CommandPropertyConditionAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class CommandPropertyConditionAttribute(string propertyName, object? value)
    : Attribute
{
    public string PropertyName { get; } = propertyName;

    public object? Value { get; } = value;

    public int Group { get; set; }

    public bool IsNot { get; set; }

    public bool OnSet { get; set; }
}
