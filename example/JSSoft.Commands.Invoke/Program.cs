// <copyright file="Program.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using JSSoft.Commands.Invoke;

try
{
    var commands = new Commands();
    var invoker = new CommandInvoker(commands);
    invoker.Invoke(args);
}
catch (CommandInvocationException e)
{
    e.Print(Console.Out);
    Environment.Exit(1);
}
