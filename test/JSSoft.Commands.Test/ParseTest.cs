// <copyright file="ParseTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.ComponentModel;

namespace JSSoft.Commands.Test;

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
        Assert.False(Boolean);
    }

    [Fact]
    public void TestMethod2()
    {
        _parser.ParseCommandLine("parse --number 1");
        Assert.Equal(1, Number);
    }

    [Fact]
    public void TestMethod3()
    {
        _parser.ParseCommandLine("parse --string qwer");
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

    [CommandProperty]
    public bool Boolean { get; set; }

    [CommandProperty]
    public int Number { get; set; }

    [CommandProperty]
    public string String { get; set; } = string.Empty;

    #region ISupportInitialize

    void ISupportInitialize.BeginInit()
    {
    }

    void ISupportInitialize.EndInit()
    {
    }

    #endregion
}
