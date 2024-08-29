// <copyright file="CustomParameterTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Tests.CommandContextTests;

public class CustomParameterTest
{
    [Fact]
    public void Method1_Test()
    {
        var testCommand = new TestCommand();
        var commands = new ICommand[]
        {
            testCommand,
        };
        var commandContext = new TestCommandContext(commands);
        commandContext.Execute("test method1 hello1 --boolean --name world1");
        var customParameter = testCommand.CustomParameter;
        Assert.NotNull(customParameter);
        Assert.Equal("hello1", customParameter.String);
        Assert.Equal(0, customParameter.Integer);
        Assert.True(customParameter.Boolean);
        Assert.Equal("world1", customParameter.Name);
    }

    [Fact]
    public async Task MethodAsync2_TestAsync()
    {
        var testCommand = new TestCommand();
        var commands = new ICommand[]
        {
            testCommand,
        };
        var commandContext = new TestCommandContext(commands);
        await commandContext.ExecuteAsync(
                "test method2 hello2 --boolean --name world2", CancellationToken.None);
        var customParameter = testCommand.CustomParameter;
        Assert.NotNull(customParameter);
        Assert.Equal("hello2", customParameter.String);
        Assert.Equal(0, customParameter.Integer);
        Assert.True(customParameter.Boolean);
        Assert.Equal("world2", customParameter.Name);
    }

    [Fact]
    public void Method3_Test()
    {
        var testCommand = new TestCommand();
        var commands = new ICommand[]
        {
            testCommand,
        };
        var commandContext = new TestCommandContext(commands);
        commandContext.Execute("test method3 id hello3 --boolean --name world3");
        var customParameter = testCommand.CustomParameter;
        Assert.NotNull(customParameter);
        Assert.Equal("id", testCommand.Value);
        Assert.Equal("hello3", customParameter.String);
        Assert.Equal(0, customParameter.Integer);
        Assert.True(customParameter.Boolean);
        Assert.Equal("world3", customParameter.Name);
    }

    [Fact]
    public async Task MethodAsync4_TestAsync()
    {
        var testCommand = new TestCommand();
        var commands = new ICommand[]
        {
            testCommand,
        };
        var commandContext = new TestCommandContext(commands);
        await commandContext.ExecuteAsync(
                "test method4 id4 hello4 --boolean --name world4", CancellationToken.None);
        var customParameter = testCommand.CustomParameter;
        Assert.NotNull(customParameter);
        Assert.Equal("id4", testCommand.Value);
        Assert.Equal("hello4", customParameter.String);
        Assert.Equal(0, customParameter.Integer);
        Assert.True(customParameter.Boolean);
        Assert.Equal("world4", customParameter.Name);
    }

    [Fact]
    public void Method5_Test()
    {
        var testCommand = new TestCommand();
        var commands = new ICommand[]
        {
            testCommand,
        };
        var commandContext = new TestCommandContext(commands);
        commandContext.Execute("test method5 hello5 id --boolean --name world5");
        var customParameter = testCommand.CustomParameter;
        Assert.NotNull(customParameter);
        Assert.Equal("hello5", customParameter.String);
        Assert.Equal("id", testCommand.Value);
        Assert.Equal(0, customParameter.Integer);
        Assert.True(customParameter.Boolean);
        Assert.Equal("world5", customParameter.Name);
    }

    private sealed class TestCommandContext(params ICommand[] commands)
        : CommandContextBase(commands)
    {
    }

    private sealed class TestCommand : CommandMethodBase
    {
        public CustomParameter? CustomParameter { get; set; }

        public object Value { get; set; } = DBNull.Value;

        [CommandMethod]
        public void Method1([CommandParameter] CustomParameter customParameter)
        {
            CustomParameter = customParameter;
        }

        [CommandMethod]
        public async Task Method2Async(
            [CommandParameter]
            CustomParameter customParameter,
            CancellationToken cancellationToken)
        {
            CustomParameter = customParameter;
            await Task.CompletedTask;
        }

        [CommandMethod]
        public void Method3(
            string id,
            [CommandParameter]
            CustomParameter customParameter)
        {
            Value = id;
            CustomParameter = customParameter;
        }

        [CommandMethod]
        public async Task Method4Async(
            string id,
            [CommandParameter]
            CustomParameter customParameter,
            CancellationToken cancellationToken)
        {
            Value = id;
            CustomParameter = customParameter;
            await Task.CompletedTask;
        }

        [CommandMethod]
        public void Method5(
            [CommandParameter]
            CustomParameter customParameter,
            string id)
        {
            CustomParameter = customParameter;
            Value = id;
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
