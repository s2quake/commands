// <copyright file="CustomParameterFailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandContextTests;

public class CustomParameterFailTest
{
    [Fact]
    public void Test1Command_ThrowTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => new Test1Command());
    }

    [Fact]
    public void Test2Command_ThrowTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => new Test2Command());
    }

    private sealed class TestCommandContext(params ICommand[] commands)
        : CommandContextBase(commands)
    {
    }

    private sealed class Test1Command : CommandMethodBase
    {
        [CommandProperty]
        public string String { get; set; } = string.Empty;

        [CommandMethod]
        [CommandMethodProperty(nameof(String))]
        [CommandMethodParameter(nameof(customParameter))]
        public void Method1(CustomParameter customParameter)
        {
        }
    }

    private sealed class Test2Command : CommandMethodBase
    {
        [CommandMethod]
        [CommandMethodParameter(nameof(id))]
        public void Method1(CustomParameter customParameter, string id)
        {
        }
    }

    private sealed class CustomParameter
    {
        [CommandPropertyRequired]
        public string String { get; set; } = string.Empty;

        [CommandPropertyRequired(DefaultValue = 0)]
        public int Integer { get; set; }

        [CommandPropertySwitch]
        public bool Boolean { get; set; }

        [CommandProperty]
        public string Name { get; set; } = string.Empty;
    }
}
