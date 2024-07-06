// <copyright file="RunTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Test;

public class RunTest
{
    private readonly CommandParser _parser;

    public RunTest()
    {
        _parser = new TestCommandParser("run", this);
    }

    [Fact]
    public void TestMethod1()
    {
        Assert.Throws<CommandLineException>(() => _parser.ParseCommandLine("run"));
    }

    [Fact]
    public void TestMethod2()
    {
        Assert.Throws<CommandLineException>(() => _parser.ParseCommandLine("run -l"));
    }

    [Fact]
    public void TestMethod3()
    {
        _parser.ParseCommandLine("run current_path");
        Assert.Equal("current_path", RepositoryPath);
        Assert.Equal(string.Empty, Authentication);
    }

    [Fact]
    public void TestMethod4()
    {
        _parser.ParseCommandLine("run current_path -l");
        Assert.Equal("current_path", RepositoryPath);
        Assert.Equal("admin", Authentication);
    }

    [Fact]
    public void TestMethod5()
    {
        _parser.ParseCommandLine("run current_path -l member");
        Assert.Equal("current_path", RepositoryPath);
        Assert.Equal("member", Authentication);
    }

    [CommandPropertyRequired]
    public string RepositoryPath { get; set; } = string.Empty;

    [CommandProperty('l', DefaultValue = "admin")]
    public string Authentication { get; set; } = string.Empty;

    #region TestCommandParser

    sealed class TestCommandParser(string name, object instance)
        : CommandParser(name, instance)
    {
        protected override void OnValidate(string[] args)
        {
        }
    }

    #endregion
}
