// <copyright file="TestTerminal.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Tests;

public sealed class TestTerminal : Terminal
{
    public TestTerminal()
        : base(TestTerminalStyle.Default, TestTerminalScroll.Default)
    {
    }
}
