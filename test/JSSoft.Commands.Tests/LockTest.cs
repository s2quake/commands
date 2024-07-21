// <copyright file="LockTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests;

public class LockTest
{
    private readonly TestCommandParser _parser;

    public LockTest()
    {
        _parser = new TestCommandParser("lock", this);
    }

    [Fact]
    public void TestMethod1()
    {
        _parser.ParseCommandLine("lock");
        Assert.Equal(string.Empty, Path);
    }

    [Fact]
    public void TestMethod2()
    {
        Assert.Throws<CommandLineException>(() => _parser.ParseCommandLine("lock -m"));
    }

    [Fact]
    public void TestMethod3()
    {
        _parser.ParseCommandLine("lock -m 123");
        Assert.Equal(string.Empty, Path);
        Assert.Equal("123", Comment);
    }

    [Fact]
    public void TestMethod4()
    {
        _parser.ParseCommandLine("lock current_path");
        Assert.Equal("current_path", Path);
    }

    [Fact]
    public void TestMethod5()
    {
        _parser.ParseCommandLine("lock current_path -m 123");
        Assert.Equal("current_path", Path);
        Assert.Equal("123", Comment);
    }

    [Fact]
    public void TestMethod6()
    {
        Assert.Throws<CommandPropertyConditionException>(
            () => _parser.ParseCommandLine("lock current_path -m 123 -i"));
    }

    [CommandPropertyRequired(DefaultValue = "")]
    public string Path { get; set; } = string.Empty;

    [CommandProperty('m')]
    [CommandPropertyCondition(nameof(Information), false)]
    public string Comment { get; set; } = string.Empty;

    [CommandPropertySwitch('i')]
    [CommandPropertyCondition(nameof(Comment), "")]
    public bool Information { get; set; }

    [CommandProperty("format", DefaultValue = "xml")]
    [CommandPropertyCondition(nameof(Information), true)]
    public string FormatType { get; set; } = string.Empty;

    private sealed class TestCommandParser(string name, object instance)
        : CommandParser(name, instance)
    {
        protected override void OnValidate(string[] args)
        {
        }
    }
}
