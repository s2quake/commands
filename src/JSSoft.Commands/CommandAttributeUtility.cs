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

    public static bool IsBrowsable(MemberInfo memberInfo)
    {
        var isBrowsable = GetValue<BrowsableAttribute, bool>(
            memberInfo: memberInfo,
            getter: memberInfo => memberInfo.Browsable,
            defaultValue: true);
        if (isBrowsable != true)
        {
            return false;
        }

        if (IsConsoleMode == true && IsDefined<ConsoleModeOnlyAttribute>(memberInfo) == true)
        {
            return false;
        }

        return true;
    }
}
