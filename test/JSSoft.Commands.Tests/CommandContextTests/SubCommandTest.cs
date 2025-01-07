// <copyright file="SubCommandTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using NuGet.Frameworks;

namespace JSSoft.Commands.Tests.CommandContextTests;

public class SubCommandTest
{
    [Fact]
    public void ChildMethod_Test()
    {
        var parentCommand = new ParentCommand();
        var childCommand = new ChildCommand(parentCommand);
        var tw = new StringWriter();
        var commandContext = new SubCommandContext(parentCommand, childCommand) { Out = tw };

        commandContext.Execute("parent child method");

        Assert.Equal("Child\n", tw.ToString());
    }

    [Fact]
    public void ParentMethod_Test()
    {
        var parentCommand = new ParentCommand();
        var childCommand = new ChildCommand(parentCommand);
        var tw = new StringWriter();
        var commandContext = new SubCommandContext(parentCommand, childCommand) { Out = tw };

        commandContext.Execute("parent method");

        Assert.Equal("Parent\n", tw.ToString());
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
            Out.WriteLine("Parent");
        }
    }

    private sealed class ChildCommand(ParentCommand parentCommand)
        : CommandMethodBase(parentCommand)
    {
        [CommandMethod]
        public void Method()
        {
            Out.WriteLine("Child");
        }
    }
}
