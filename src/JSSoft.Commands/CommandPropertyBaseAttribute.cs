// <copyright file="CommandPropertyBaseAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using JSSoft.Commands.Exceptions;

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Property)]
public abstract class CommandPropertyBaseAttribute : Attribute
{
    private protected CommandPropertyBaseAttribute()
    {
        AllowName = true;
    }

    private protected CommandPropertyBaseAttribute(string name)
    {
        Name = name;
    }

    private protected CommandPropertyBaseAttribute(string name, char shortName)
    {
        Name = name;
        ShortName = shortName;
    }

    private protected CommandPropertyBaseAttribute(char shortName)
        : this(shortName, useName: false)
    {
    }

    private protected CommandPropertyBaseAttribute(char shortName, bool useName)
    {
        ShortName = shortName;
        AllowName = useName;
    }

    public string Name { get; } = string.Empty;

    public char ShortName { get; }

    public bool AllowName { get; }

    internal char GetShortName(CommandMemberInfo memberInfo)
    {
        if (ShortName != char.MinValue)
        {
            if (CommandUtility.IsShortName(ShortName) != true)
            {
                throw new CommandInvalidShortNameException(memberInfo, ShortName);
            }

            return ShortName;
        }

        return char.MinValue;
    }

    internal string GetName(CommandMemberInfo memberInfo, string defaultName)
    {
        if (Name != string.Empty)
        {
            return Name;
        }

        return AllowName == true ? CommandUtility.ToSpinalCase(defaultName) : string.Empty;
    }
}
