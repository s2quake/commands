// <copyright file="CommandPropertyExclusionAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class CommandPropertyExclusionAttribute(string propertyName) : Attribute
{
    public string PropertyName { get; } = propertyName;
}
