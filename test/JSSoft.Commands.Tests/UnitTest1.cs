// <copyright file="UnitTest1.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests;

public class UnitTest1
{
    [Fact]
    public void TestMethod1()
    {
        var settings = new Settings();
        var parser = new CommandParser(settings);
        parser.Parse("--list -c");

        Assert.Equal(string.Empty, settings.List);
        Assert.True(settings.IsCancel);
        Assert.Equal(5005, settings.Port);
    }

    [Fact]
    public void TestMethod2()
    {
        var settings = new Settings();
        var parser = new CommandParser(settings);
        parser.Parse("--list wer -c");

        Assert.Equal("wer", settings.List);
        Assert.True(settings.IsCancel);
        Assert.Equal(5005, settings.Port);
    }

    [Fact]
    public void TestMethod3()
    {
        var settings = new Settings();
        var parser = new CommandParser(settings);
        parser.Parse("--list \"a \\\"b\\\" c\" -c");

        Assert.Equal("a \"b\" c", settings.List);
        Assert.True(settings.IsCancel);
        Assert.Equal(5005, settings.Port);
    }

    [Fact]
    public void TestMethod4()
    {
        var commands = new Commands();
        var invoker = new CommandInvoker(commands);
        invoker.Invoke("test a -m wow");
        Assert.Equal("test", commands.Result);
    }

    [Fact]
    public void TestMethod5()
    {
        var commands = new Commands();
        var invoker = new CommandInvoker(commands);
        invoker.Invoke("push-many -- a b");
        Assert.Equal("push-many", commands.Result);
    }

    [Fact]
    public void TestMethod6()
    {
        var commands = new Commands();
        var invoker = new CommandInvoker(commands);
        invoker.Invoke("items");
        Assert.False(commands.IsReverseResult);
    }

    [Fact]
    public void TestMethod7()
    {
        var commands = new Commands();
        var invoker = new CommandInvoker(commands);
        invoker.Invoke("items -r");
        Assert.True(commands.IsReverseResult);
    }

    [Fact]
    public void TestMethod8()
    {
        var commands = new Commands();
        var invoker = new CommandInvoker(commands);
        invoker.Invoke("items --reverse");
        Assert.True(commands.IsReverseResult);
    }

    private class Settings
    {
        [CommandProperty(DefaultValue = "")]
        public string List { get; set; } = string.Empty;

        [CommandPropertySwitch('c')]
        public bool IsCancel { get; set; }

        [CommandProperty(InitValue = 5005)]
        public int Port { get; set; }
    }

    private class Commands
    {
        public string Result { get; private set; } = string.Empty;

        [CommandMethod]
        [CommandMethodProperty(nameof(Message))]
        public void Test(string target1, string target2 = "")
        {
            Assert.Equal("a", target1);
            Assert.Empty(target2);
            Assert.Equal("wow", Message);
            Result = "test";
        }

        [CommandMethod]
        public void PushMany(params string[] items)
        {
            Assert.Equal("a", items[0]);
            Assert.Equal("b", items[1]);
            Result = "push-many";
        }

        [CommandMethod("items")]
        [CommandMethodProperty(nameof(IsReverse))]
        public void ShowItems()
        {
            IsReverseResult = IsReverse;
        }

        public bool IsReverseResult { get; set; }

        [CommandPropertySwitch("reverse", 'r')]
        public bool IsReverse { get; set; }

        [CommandPropertyExplicitRequired('m')]
        public string Message { get; set; } = string.Empty;
    }
}
