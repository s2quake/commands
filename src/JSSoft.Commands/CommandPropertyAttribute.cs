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

    public override CommandType CommandType => CommandType.General;
}
