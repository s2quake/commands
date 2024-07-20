// <copyright file="CommandPropertySwitchAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public sealed class CommandPropertySwitchAttribute : CommandPropertyBaseAttribute
{
    public CommandPropertySwitchAttribute()
    {
    }

    public CommandPropertySwitchAttribute(string name)
        : base(name)
    {
    }

    public CommandPropertySwitchAttribute(string name, char shortName)
        : base(name, shortName)
    {
    }

    public CommandPropertySwitchAttribute(char shortName)
        : base(shortName)
    {
    }

    public CommandPropertySwitchAttribute(char shortName, bool useName)
        : base(shortName, useName)
    {
    }

    public override CommandType CommandType => CommandType.Switch;

    public bool Invert { get; set; }

    [Obsolete("In the Switch property, InitValue is not used.")]
    public new object InitValue => base.InitValue;

    [Obsolete("In the Switch property, DefaultValue is not used.")]
    public new object DefaultValue => base.DefaultValue;
}
