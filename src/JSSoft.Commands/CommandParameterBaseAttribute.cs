// <copyright file="CommandParameterBaseAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Parameter)]
public abstract class CommandParameterBaseAttribute : CommandMemberBaseAttribute
{
    private protected CommandParameterBaseAttribute()
    {
    }

    private protected CommandParameterBaseAttribute(string name)
        : base(name)
    {
    }

    private protected CommandParameterBaseAttribute(string name, char shortName)
        : base(name, shortName)
    {
    }

    private protected CommandParameterBaseAttribute(char shortName)
        : base(shortName)
    {
    }

    private protected CommandParameterBaseAttribute(char shortName, bool useName)
        : base(shortName, useName)
    {
    }
}
