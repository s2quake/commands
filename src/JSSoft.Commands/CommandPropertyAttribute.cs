// <copyright file="CommandPropertyAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public sealed class CommandPropertyAttribute : CommandPropertyBaseAttribute
{
    public CommandPropertyAttribute()
    {
    }

    public CommandPropertyAttribute(string name)
        : base(name)
    {
    }

    public CommandPropertyAttribute(string name, char shortName)
        : base(name, shortName)
    {
    }

    public CommandPropertyAttribute(char shortName)
        : base(shortName)
    {
    }

    public CommandPropertyAttribute(char shortName, bool useName)
        : base(shortName, useName)
    {
    }

    /// <summary>
    /// Gets or sets the initial value when the option is not specified on the command line.
    /// </summary>
    public object InitValue { get; set; } = DBNull.Value;

    /// <summary>
    /// Gets or sets the default value when the option is specified on the command line
    /// but the value is not set.
    /// </summary>
    public object DefaultValue { get; set; } = DBNull.Value;
}
