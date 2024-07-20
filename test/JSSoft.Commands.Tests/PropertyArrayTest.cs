// <copyright file="PropertyArrayTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests;

public class PropertyArrayTest
{
    [Fact]
    public void TestMethod1()
    {
        var instance = new Instance1();
        var parser = new TestCommandParser("array", instance);
        parser.ParseCommandLine("array");
        Assert.NotNull(instance.Arguments);
        Assert.Empty(instance.Arguments);
    }

    private class Instance1
    {
        [CommandPropertyArray]
        public string[] Arguments { get; set; } = [];
    }

    [Fact]
    public void TestMethod2()
    {
        var instance = new Instance2();
        var parser = new TestCommandParser("array", instance);
        parser.ParseCommandLine("array");
        Assert.NotNull(instance.Arguments);
        Assert.Empty(instance.Arguments);
        Assert.True(Array.Empty<string>().SequenceEqual(instance.Arguments));
    }

    private class Instance2
    {
        [CommandPropertyArray]
        public string[] Arguments { get; set; } = [];
    }

    [Fact]
    public void TestMethod3()
    {
        var instance = new Instance3();
        var parser = new TestCommandParser("array", instance);
        Assert.Throws<CommandDefinitionException>(() => parser.ParseCommandLine("array"));
    }

    private class Instance3
    {
        [CommandPropertyArray]
        public string[] Arguments1 { get; set; } = [];

        [CommandPropertyArray]
        public string[] Arguments2 { get; set; } = [];
    }

    [Fact]
    public void TestMethod4()
    {
        var instance = new Instance4();
        var parser = new TestCommandParser("array", instance);
        parser.ParseCommandLine("array");
        Assert.NotNull(instance.Arguments);
        Assert.Empty(instance.Arguments);
        Assert.Equivalent(Array.Empty<string>(), instance.Arguments);
    }

    private class Instance4
    {
        [CommandPropertyArray]
        public string[] Arguments { get; set; } = [];
    }

    private sealed class TestCommandParser(string name, object instance)
        : CommandParser(name, instance)
    {
        protected override void OnValidate(string[] args)
        {
        }
    }
}
