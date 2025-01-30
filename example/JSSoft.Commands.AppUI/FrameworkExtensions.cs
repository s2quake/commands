// <copyright file="FrameworkExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.AppUI;

internal static class FrameworkExtensions
{
#if NETFRAMEWORK
    public static void AppendJoin(
        this System.Text.StringBuilder @this, string separator, params string[] values)
    {
        @this.Append(string.Join(separator, values));
    }
#endif
}
