// <copyright file="SubCommandTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandContextTests;

public class SubCommandTest
{
    [Fact]
    public void Test1()
    {
        var parentCommand = new ParentCommand();
        var childCommand = new ChildCommand(parentCommand);
        var commandContext = new SubCommandContext(parentCommand, childCommand);

        commandContext.Execute("parent child method");

        Assert.True(true);
    }

    private sealed class SubCommandContext(params ICommand[] commands)
        : CommandContextBase(commands)
    {
    }

    private sealed class ParentCommand : CommandMethodBase
    {
        [CommandMethod]
        public void Method()
        {
        }
    }

    private sealed class ChildCommand(ParentCommand parentCommand)
        : CommandMethodBase(parentCommand)
    {
        [CommandMethod]
        public void Method()
        {
        }
    }
}
