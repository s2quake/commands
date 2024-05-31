// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 


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
