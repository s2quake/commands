﻿// <copyright file="ITerminalMode.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public interface ITerminalModes
{
    bool this[TerminalMode mode] { get; set; }
}
