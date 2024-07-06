// <copyright file="CommandAttributeUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands;

public static class CommandAttributeUtility
{
    public static bool IsCommandProperty(PropertyInfo propertyInfo)
    {
        if (AttributeUtility.GetCustomAttribute<CommandPropertyBaseAttribute>(propertyInfo, inherit: true) is { } == true)
        {
            return true;
        }
        return false;
    }

    public static bool IsCommandMethod(MethodInfo methodInfo)
    {
        if (AttributeUtility.GetCustomAttribute<CommandMethodAttribute>(methodInfo) is { } == true)
        {
            return true;
        }
        return false;
    }

    public static bool GetBrowsable(MemberInfo memberInfo)
    {
        if (AttributeUtility.GetBrowsable(memberInfo) == false)
            return false;
        if (CommandSettings.IsConsoleMode == false && AttributeUtility.GetCustomAttribute<ConsoleModeOnlyAttribute>(memberInfo) is not { })
            return false;
        return true;
    }

    public static string GetSummary(MemberInfo memberInfo)
    {
        if (AttributeUtility.GetCustomAttribute<CommandSummaryAttribute>(memberInfo) is { } commandSummaryAttribute)
        {
            return commandSummaryAttribute.Summary;
        }
        return string.Empty;
    }

    public static string GetSummary(ParameterInfo parameterInfo)
    {
        if (AttributeUtility.GetCustomAttribute<CommandSummaryAttribute>(parameterInfo) is { } commandSummaryAttribute)
        {
            return commandSummaryAttribute.Summary;
        }
        return string.Empty;
    }

    public static string GetDescription(MemberInfo memberInfo)
    {
        if (AttributeUtility.GetCustomAttribute<DescriptionAttribute>(memberInfo) is { } descriptionAttribute)
        {
            return descriptionAttribute.Description;
        }
        return string.Empty;
    }

    public static string GetDescription(ParameterInfo parameterInfo)
    {
        if (AttributeUtility.GetCustomAttribute<DescriptionAttribute>(parameterInfo) is { } descriptionAttribute)
        {
            return descriptionAttribute.Description;
        }
        return string.Empty;
    }

    public static string GetExample(MemberInfo memberInfo)
    {
        if (AttributeUtility.GetCustomAttribute<CommandExampleAttribute>(memberInfo) is { } commandExampleAttribute)
        {
            return commandExampleAttribute.Example;
        }
        return string.Empty;
    }

    public static string GetExample(ParameterInfo parameterInfo)
    {
        if (AttributeUtility.GetCustomAttribute<CommandExampleAttribute>(parameterInfo) is { } commandExampleAttribute)
        {
            return commandExampleAttribute.Example;
        }
        return string.Empty;
    }
}
