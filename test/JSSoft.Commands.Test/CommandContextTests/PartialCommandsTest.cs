// <copyright file="PartialCommandsTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>


namespace JSSoft.Commands.Test.CommandContextTests;

public class PartialCommandsTest
{
    [Fact]
    public void PartialCommands_Test()
    {
        var expectedCommands = new ICommand[]
        {
            new TestCommand(),
            new PartialCommand1(),
            new PartialCommand2(),
        };
        var commandContext = new TestCommandContext(expectedCommands);
        var rootNode = commandContext.Node;
        Assert.Single(rootNode.Children);

        var testNode = rootNode.Children["test"];
        var actualCommands = testNode.Commands.ToArray();
        Assert.Equal("test", testNode.Name);
        Assert.Equal(expectedCommands[0], actualCommands[0]);
        Assert.Equal(expectedCommands[1], actualCommands[1]);
        Assert.Equal(expectedCommands[2], actualCommands[2]);
    }

    sealed class TestCommandContext : CommandContextBase
    {
        public TestCommandContext(params ICommand[] commands)
            : base(commands)
        {
        }
    }

    sealed class TestCommand : CommandMethodBase
    {
        [CommandMethod]
        public void Method()
        {
        }
    }

    [PartialCommand]
    sealed class PartialCommand1 : CommandMethodBase
    {
        public PartialCommand1()
            : base("test")
        {
        }

        [CommandMethod]
        public void Method1()
        {
        }
    }

    [PartialCommand]
    sealed class PartialCommand2 : CommandMethodBase
    {
        public PartialCommand2()
            : base("test")
        {
        }

        [CommandMethod]
        public void Method2()
        {
        }
    }
}
