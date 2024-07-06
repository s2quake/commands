// <copyright file="Program.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using JSSoft.Commands.Repl;

using var application = new Application();
Console.WriteLine();
await application.StartAsync();
Console.WriteLine("\u001b0");
