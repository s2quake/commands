// <copyright file="CommandPropertyArrayTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Test;

public class CommandPropertyArrayTest
{
    [Fact]
    public void Test1()
    {
        var parser = new CommandParser(this);
        parser.Parse("get database=a port=123 userid=abc password=1234 comment=\"connect database to \\\"a\\\"\"");
    }

    [Fact]
    public void Test2()
    {
        var parser = new CommandParser(this);
        parser.Parse("get \"database=a b c\"");

        Assert.Single(Arguments);
        Assert.Equal("database=a b c", Arguments[0]);
    }

    [Fact]
    public void Test3()
    {
        var parser = new CommandParser(this);
        parser.Parse("get \"database=\\\"a b c\\\"\"");

        Assert.Single(Arguments);
        Assert.Equal("database=\"a b c\"", Arguments[0]);
    }

    // [Fact]
    // public void ValueIncludedEqualsTest()
    // {
    //     var parser = new CommandParser(this);
    //     Assert.Throws<CommandLineException>(() => parser.Parse("--value=0"));
    // }

    [Fact]
    public void ValueIncludedEqualsTest2()
    {
        var parser = new CommandParser(this);
        parser.Parse("get value=0");
        Assert.Equal("get", Command);
        Assert.Equal("value=0", Arguments[0]);
    }

    [CommandPropertyRequired]
    public string Command { get; set; } = string.Empty;

    [CommandPropertyArray]
    public string[] Arguments { get; set; } = [];
}
