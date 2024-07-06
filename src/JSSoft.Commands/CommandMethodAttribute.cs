// <copyright file="CommandMethodAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class CommandMethodAttribute(string name) : Attribute
{
    public CommandMethodAttribute()
        : this(string.Empty)
    {
    }

    public string Name { get; } = name;

    public string[] Aliases { get; set; } = [];
}
