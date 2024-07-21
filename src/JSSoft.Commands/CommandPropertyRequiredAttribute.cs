// <copyright file="CommandPropertyRequiredAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public sealed class CommandPropertyRequiredAttribute : CommandPropertyBaseAttribute
{
    public CommandPropertyRequiredAttribute()
    {
    }

    public CommandPropertyRequiredAttribute(string name)
        : base(name)
    {
    }

    public object DefaultValue { get; set; } = DBNull.Value;
}
