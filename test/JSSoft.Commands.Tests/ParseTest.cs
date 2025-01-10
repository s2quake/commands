// <copyright file="ParseTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands.Tests;

public class ParseTest : ISupportInitialize
{
    private readonly CommandParser _parser;

    public ParseTest()
    {
        _parser = new CommandParser("parse", this);
    }

    [Fact]
    public void TestMethod1()
    {
        _parser.ParseCommandLine("parse --boolean false");
        Assert.Equal(string.Empty, Value);
        Assert.Equal(0, Number);
        Assert.False(Boolean);
        Assert.Equal(string.Empty, String);
    }

    [Fact]
    public void TestMethod2()
    {
        _parser.ParseCommandLine("parse --number 1");
        Assert.Equal(string.Empty, Value);
        Assert.Equal(1, Number);
        Assert.False(Boolean);
        Assert.Equal(string.Empty, String);
    }

    [Fact]
    public void TestMethod3()
    {
        _parser.ParseCommandLine("parse --string qwer");
        Assert.Equal(string.Empty, Value);
        Assert.Equal(0, Number);
        Assert.False(Boolean);
        Assert.Equal("qwer", String);
    }

    [Fact]
    public void TestMethod4()
    {
        Assert.Throws<CommandLineException>(() => _parser.ParseCommandLine("parse --boolean ewe"));
    }

    [Fact]
    public void TestMethod5()
    {
        Assert.Throws<CommandLineException>(() => _parser.ParseCommandLine("parse --number ewe"));
    }

    [Fact]
    public void TestMethod6()
    {
        _parser.ParseCommandLine("parse --string qwer value");
        Assert.Equal("qwer", String);
        Assert.Equal("value", Value);
    }

    [Fact]
    public void TestMethod7()
    {
        _parser.ParseCommandLine("parse value --string qwer");
        Assert.Equal("qwer", String);
        Assert.Equal("value", Value);
    }

    [CommandPropertyRequired(DefaultValue = "")]
    public string Value { get; set; } = string.Empty;

    [CommandProperty]
    public bool Boolean { get; set; }

    [CommandProperty]
    public int Number { get; set; }

    [CommandProperty]
    public string String { get; set; } = string.Empty;

    void ISupportInitialize.BeginInit()
    {
    }

    void ISupportInitialize.EndInit()
    {
    }
}
