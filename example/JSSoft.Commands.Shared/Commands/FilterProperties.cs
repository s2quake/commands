// <copyright file="FilterProperties.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;

namespace JSSoft.Commands.Applications.Commands;

[ResourceUsage]
public static class FilterProperties
{
    [CommandProperty("filter")]
    [CommandPropertyCondition(nameof(FilterFile), "")]
    public static string Filter { get; set; } = string.Empty;

    [CommandProperty(DefaultValue = "")]
    [CommandPropertyCondition(nameof(Filter), "")]
    public static string FilterFile { get; set; } = string.Empty;

    public static string FilterExpression
    {
        get
        {
            if (FilterFile != string.Empty)
            {
                var lines = File.ReadAllLines(FilterFile);
                return string.Join(";", lines);
            }

            return Filter;
        }
    }
}
