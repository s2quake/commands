// <copyright file="TerminalControlCompleter.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.AppUI.Controls;

public delegate string[] TerminalControlCompleter(string[] items, string find);
