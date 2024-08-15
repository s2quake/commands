// <copyright file="MemberInfoExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using static JSSoft.Commands.AttributeUtility;
using static JSSoft.Commands.CommandSettings;

namespace JSSoft.Commands.Extensions;

public static class MemberInfoExtensions
{
    public static bool IsBrowsable(this MemberInfo @this)
    {
        var isBrowsable = GetValue<BrowsableAttribute, bool>(
            memberInfo: @this,
            getter: memberInfo => memberInfo.Browsable,
            defaultValue: true);
        if (isBrowsable != true)
        {
            return false;
        }

        if (IsConsoleMode != true && IsDefined<ConsoleModeOnlyAttribute>(@this) != true)
        {
            return false;
        }

        if (@this.DeclaringType is { } declaringType)
        {
            return IsBrowsable(declaringType);
        }

        return true;
    }
}
