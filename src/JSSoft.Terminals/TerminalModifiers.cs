// <copyright file="TerminalModifiers.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

[Flags]
public enum TerminalModifiers
{
    Alt = 1,

    Shift = 2,

    Control = 4
}
