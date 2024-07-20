// <copyright file="SingleQuoteParseTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests;

public class SingleQuoteParseTest
{
    [Fact]
    public void SingleQuotTest()
    {
        var parser = new CommandParser(this);
        var text1 = "abc test 123";
        var text2 = "'abc test 123'";
        var args = string.Join(" ", "--value", text2);
        parser.Parse(args);

        Assert.Equal(text1, Value);
    }

    [Fact]
    public void SingleQuotInSingleQuotTest1()
    {
        var parser = new CommandParser(this);
        var text1 = "abc 'test' 123";
        var text2 = "\"abc 'test' 123\"";
        var args = string.Join(" ", "--value", text2);
        parser.Parse(args);

        Assert.Equal(text1, Value);
    }

    [Fact]
    public void SingleQuotInSingleQuotTest2()
    {
        var parser = new CommandParser(this);
        var text1 = "abc test 123";
        var text2 = "'abc 'test' 123'";
        var args = string.Join(" ", "--value", text2);
        parser.Parse(args);

        Assert.Equal(text1, Value);
    }

    [CommandProperty]
    public string Value { get; set; } = string.Empty;
}
