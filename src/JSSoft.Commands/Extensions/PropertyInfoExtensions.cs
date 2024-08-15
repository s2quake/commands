// <copyright file="PropertyInfoExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using static JSSoft.Commands.AttributeUtility;

namespace JSSoft.Commands.Extensions;

public static class PropertyInfoExtensions
{
    public static bool IsCommandProperty(this PropertyInfo @this)
        => IsDefined<CommandPropertyBaseAttribute>(@this, inherit: true);
}
