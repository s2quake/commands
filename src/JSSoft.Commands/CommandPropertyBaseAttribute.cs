// <copyright file="CommandPropertyBaseAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Property)]
public abstract class CommandPropertyBaseAttribute : Attribute
{
    internal CommandPropertyBaseAttribute()
    {
        AllowName = true;
    }

    internal CommandPropertyBaseAttribute(string name)
    {
        ThrowUtility.ThrowIfInvalidName(name);

        Name = name;
    }

    internal CommandPropertyBaseAttribute(string name, char shortName)
    {
        ThrowUtility.ThrowIfInvalidName(name);
        ThrowUtility.ThrowIfInvalidShortName(shortName);

        Name = name;
        ShortName = shortName;
    }

    internal CommandPropertyBaseAttribute(char shortName)
        : this(shortName, useName: false)
    {
    }

    internal CommandPropertyBaseAttribute(char shortName, bool useName)
    {
        ThrowUtility.ThrowIfInvalidShortName(shortName);

        ShortName = shortName;
        AllowName = useName;
    }

    public string Name { get; } = string.Empty;

    public char ShortName { get; }

    public bool AllowName { get; }

    public object DefaultValue { get; set; } = DBNull.Value;

    public object InitValue { get; set; } = DBNull.Value;

    public abstract CommandType CommandType { get; }

    internal bool IsRequired => CommandType == CommandType.Required || CommandType == CommandType.ExplicitRequired;

    internal bool IsExplicit => CommandType == CommandType.General || CommandType == CommandType.ExplicitRequired || CommandType == CommandType.Switch;

    internal bool IsSwitch => CommandType == CommandType.Switch;

    internal bool IsVariables => CommandType == CommandType.Variables;

    internal string GetName(string defaultName)
    {
        if (Name != string.Empty)
            return Name;
        return AllowName == true ? CommandUtility.ToSpinalCase(defaultName) : string.Empty;
    }
}
