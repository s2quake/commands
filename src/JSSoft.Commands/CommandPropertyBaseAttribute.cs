// <copyright file="CommandPropertyBaseAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using JSSoft.Commands.Exceptions;

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Property)]
public abstract class CommandPropertyBaseAttribute : CommandMemberBaseAttribute
{
    private protected CommandPropertyBaseAttribute()
    {
    }

    private protected CommandPropertyBaseAttribute(string name)
        : base(name)
    {
    }

    private protected CommandPropertyBaseAttribute(string name, char shortName)
        : base(name, shortName)
    {
    }

    private protected CommandPropertyBaseAttribute(char shortName)
        : base(shortName)
    {
    }

    private protected CommandPropertyBaseAttribute(char shortName, bool useName)
        : base(shortName, useName)
    {
    }

    internal static bool IsRequired(
        CommandPropertyBaseAttribute attribute, PropertyInfo propertyInfo)
        => attribute is CommandPropertyRequiredAttribute
            or CommandPropertyExplicitRequiredAttribute;

    internal static bool IsExplicit(
        CommandPropertyBaseAttribute attribute, PropertyInfo propertyInfo)
        => attribute is CommandPropertyExplicitRequiredAttribute
            or CommandPropertyAttribute
            or CommandPropertySwitchAttribute;

    internal static bool IsSwitch(
        CommandPropertyBaseAttribute attribute, PropertyInfo propertyInfo)
    {
        if (attribute is CommandPropertySwitchAttribute)
        {
            if (propertyInfo.PropertyType != typeof(bool))
            {
                throw new CommandPropertyCannotBeUsedAsSwitchTypeException(propertyInfo);
            }

            return true;
        }

        return false;
    }

    internal static bool IsVariables(
        CommandPropertyBaseAttribute attribute, PropertyInfo propertyInfo)
    {
        if (attribute is CommandPropertyArrayAttribute)
        {
            if (propertyInfo.PropertyType.IsArray != true)
            {
                throw new CommandPropertyCannotBeUsedAsVariablesTypeException(propertyInfo);
            }

            return true;
        }

        return false;
    }

    internal static bool IsGeneral(
        CommandPropertyBaseAttribute attribute, PropertyInfo propertyInfo)
        => attribute is CommandPropertyAttribute;
}
