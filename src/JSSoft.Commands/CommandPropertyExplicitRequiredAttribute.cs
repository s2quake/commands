// <copyright file="CommandPropertyExplicitRequiredAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public sealed class CommandPropertyExplicitRequiredAttribute : CommandPropertyBaseAttribute
{
    public CommandPropertyExplicitRequiredAttribute()
    {
    }

    public CommandPropertyExplicitRequiredAttribute(string name)
        : base(name)
    {
    }

    public CommandPropertyExplicitRequiredAttribute(string name, char shortName)
        : base(name, shortName)
    {
    }

    public CommandPropertyExplicitRequiredAttribute(char shortName)
        : this(shortName, useName: false)
    {
    }

    public CommandPropertyExplicitRequiredAttribute(char shortName, bool useName)
        : base(shortName, useName)
    {
    }

    public object DefaultValue { get; set; } = DBNull.Value;
}
