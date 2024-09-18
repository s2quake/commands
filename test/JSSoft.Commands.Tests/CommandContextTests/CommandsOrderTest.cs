// <copyright file="CommandsOrderTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandContextTests;

public class CommandsOrderTest
{
    [Fact]
    public void Commands_Order_Test()
    {
        var commands = new ICommand[]
        {
            new TestCommand(),
            new AddCommand(),
            new InitCommand(),
        };
        var commandContext = new TestCommandContext(commands);

        var index = 0;
        Assert.IsType<TestCommand>(commandContext.Node.Commands[index++]);
        Assert.IsType<AddCommand>(commandContext.Node.Commands[index++]);
        Assert.IsType<InitCommand>(commandContext.Node.Commands[index++]);
        Assert.IsAssignableFrom<HelpCommandBase>(commandContext.Node.Commands[index++]);
        Assert.IsAssignableFrom<VersionCommandBase>(commandContext.Node.Commands[index++]);
        Assert.Equal(5, index);
    }

    private sealed class TestCommand : CommandBase
    {
        protected override void OnExecute()
        {
            // do nothing
        }
    }

    private sealed class AddCommand : CommandBase
    {
        protected override void OnExecute()
        {
            // do nothing
        }
    }

    private sealed class InitCommand : CommandBase
    {
        protected override void OnExecute()
        {
            // do nothing
        }
    }
}
