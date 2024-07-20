// <copyright file="CommandAttributeUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using static JSSoft.Commands.AttributeUtility;
using static JSSoft.Commands.CommandSettings;

namespace JSSoft.Commands;

public static class CommandAttributeUtility
{
    public static bool IsCommandProperty(PropertyInfo propertyInfo)
        => IsDefined<CommandPropertyBaseAttribute>(propertyInfo, inherit: true);

    public static bool IsCommandMethod(MethodInfo methodInfo)
        => IsDefined<CommandMethodAttribute>(methodInfo);

    public static bool GetBrowsable(MemberInfo memberInfo)
    {
        var browsable = GetValue<BrowsableAttribute, bool>(
            memberInfo: memberInfo,
            getter: memberInfo => memberInfo.Browsable,
            defaultValue: true);
        if (browsable != true)
        {
            return false;
        }

        if (IsConsoleMode == true && IsDefined<ConsoleModeOnlyAttribute>(memberInfo) == true)
        {
            return false;
        }

        return true;
    }

    public static string GetSummary(MemberInfo memberInfo)
        => GetValue<CommandSummaryAttribute>(memberInfo, memberInfo => memberInfo.Summary);

    public static string GetSummary(ParameterInfo parameterInfo)
        => GetValue<CommandSummaryAttribute>(parameterInfo, memberInfo => memberInfo.Summary);

    public static string GetDescription(MemberInfo memberInfo)
        => GetValue<DescriptionAttribute>(memberInfo, memberInfo => memberInfo.Description);

    public static string GetDescription(ParameterInfo parameterInfo)
        => GetValue<DescriptionAttribute>(parameterInfo, memberInfo => memberInfo.Description);

    public static string GetExample(MemberInfo memberInfo)
        => GetValue<CommandExampleAttribute>(memberInfo, memberInfo => memberInfo.Example);

    public static string GetExample(ParameterInfo parameterInfo)
        => GetValue<CommandExampleAttribute>(parameterInfo, parameterInfo => parameterInfo.Example);
}
