// <copyright file="UnlockTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests;

public sealed class UnlockTest
{
    private readonly CommandParser _parser;

    public UnlockTest()
    {
        _parser = new TestCommandParser("unlock", this);
    }

    [Fact]
    public void TestMethod1()
    {
        _parser.ParseCommandLine("unlock");
        Assert.Equal(string.Empty, Path);
    }

    [Fact]
    public void TestMethod2()
    {
        Assert.Throws<CommandLineException>(() => _parser.ParseCommandLine("unlock -m"));
    }

    [Fact]
    public void TestMethod3()
    {
        _parser.ParseCommandLine("unlock current_path");
        Assert.Equal("current_path", Path);
    }

    [CommandPropertyRequired(DefaultValue = "")]
    public string Path { get; set; } = string.Empty;

    private sealed class TestCommandParser(string name, object instance)
        : CommandParser(name, instance)
    {
        protected override void OnValidate(string[] args)
        {
        }
    }
}
