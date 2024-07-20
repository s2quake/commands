// <copyright file="StringBuilderExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Text;

namespace JSSoft.Commands.Extensions;

internal static class StringBuilderExtensions
{
    public static StringBuilder AppendLineIf(
        this StringBuilder @this, string value, Func<string, bool> predicate)
    {
        if (predicate(value) == true)
        {
            @this.AppendLine(value);
        }

        return @this;
    }

    public static StringBuilder AppendIf(
        this StringBuilder @this, string value, Func<string, bool> predicate)
    {
        if (predicate(value) == true)
        {
            @this.Append(value);
        }

        return @this;
    }
}
