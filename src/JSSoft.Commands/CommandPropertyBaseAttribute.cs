// <copyright file="CommandPropertyBaseAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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
        ThrowUtility.ThrowIfInvalidName(name);

        Name = name;
    }

    private protected CommandPropertyBaseAttribute(string name, char shortName)
    {
        ThrowUtility.ThrowIfInvalidName(name);
        ThrowUtility.ThrowIfInvalidShortName(shortName);

        Name = name;
        ShortName = shortName;
    }

    private protected CommandPropertyBaseAttribute(char shortName)
        : this(shortName, useName: false)
    {
    }

    private protected CommandPropertyBaseAttribute(char shortName, bool useName)
    {
        ThrowUtility.ThrowIfInvalidShortName(shortName);

        ShortName = shortName;
        AllowName = useName;
    }

    public string Name { get; } = string.Empty;

    public char ShortName { get; }

    public bool AllowName { get; }

    internal string GetName(string defaultName)
    {
        if (Name != string.Empty)
        {
            return Name;
        }

        return AllowName == true ? CommandUtility.ToSpinalCase(defaultName) : string.Empty;
    }
}
