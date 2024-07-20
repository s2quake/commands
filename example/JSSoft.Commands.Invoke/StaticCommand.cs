// <copyright file="StaticCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.ComponentModel;

namespace JSSoft.Commands.Invoke;

[ConsoleModeOnly]
[ResourceUsage]
internal static class StaticCommand
{
    [CommandProperty]
    public static string Filter { get; set; } = string.Empty;

    [CommandMethod]
    [CommandMethodProperty(nameof(Filter))]
    [Category("ReadOnly")]
    public static void List()
    {
        Console.WriteLine("list invoked.");
    }
}
