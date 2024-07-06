// <copyright file="TerminalModes_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Tests;

public class TerminalModes_Test
{
    [Fact]
    public void Set_Test()
    {
        var terminal = new TestTerminal();
        var mode = RandomUtility.NextEnum<TerminalMode>();

        terminal.Modes[mode] = true;
        Assert.True(terminal.Modes[mode]);

        terminal.Modes[mode] = false;
        Assert.False(terminal.Modes[mode]);
    }

    [Fact]
    public void SetUnspecified_FailTest()
    {
        var terminal = new TestTerminal();
        var mode = RandomUtility.NextUnspecifiedEnum<TerminalMode>();

        Assert.Throws<KeyNotFoundException>(() => terminal.Modes[mode] = true);
    }
}
