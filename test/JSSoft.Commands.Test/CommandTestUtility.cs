// <copyright file="CommandTestUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Test;

static class CommandTestUtility
{
    public static string GetExpression(Array array) => GetExpression(array, " ");

    public static string GetExpression(Array array, string separator)
    {
        var itemList = new List<string>(array.Length);
        for (var i = 0; i < array.Length; i++)
        {
            var value1 = array.GetValue(i);
            object? value = value1 is string s && CommandUtility.TryWrapDoubleQuotes(s, out var w) ? w : value1;
            itemList.Add($"{value:R}");
        }
        return string.Join(separator, itemList);
    }
}
