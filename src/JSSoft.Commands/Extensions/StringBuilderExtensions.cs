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
        if (predicate(value) is true)
        {
            @this.AppendLine(value);
        }

        return @this;
    }

    public static StringBuilder AppendIf(
        this StringBuilder @this, string value, Func<string, bool> predicate)
    {
        if (predicate(value) is true)
        {
            @this.Append(value);
        }

        return @this;
    }

    public static StringBuilder AppendMany<T>(
        this StringBuilder @this, IEnumerable<T> items, Func<T, string> formatter)
    {
        foreach (var item in items)
        {
            @this.Append(formatter(item));
        }

        return @this;
    }
}
