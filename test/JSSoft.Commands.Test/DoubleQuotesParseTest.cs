// <copyright file="DoubleQuotesParseTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Test;

public class DoubleQuotesParseTest
{
    [Fact]
    public void Test1()
    {
        var parser = new CommandParser(this);
        var text1 = "abc test 123";
        var text2 = "\"abc test 123\"";
        var args = string.Join(" ", "--value", text2);
        parser.Parse(args);

        Assert.Equal(text1, Value);
    }

    [Fact]
    public void Test2()
    {
        var parser = new CommandParser(this);
        var expectedText = "abc \"test\" 123";
        var valueText = "\"abc \\\"test\\\" 123\"";
        var args = string.Join(" ", "--value", valueText);
        parser.Parse(args);

        Assert.Equal(expectedText, Value);
    }

    [Fact]
    public void EscapeTest1()
    {
        var parser = new CommandParser(this);
        var expectedText = "\\";
        var valueText = "\"\\\\\"";
        var args = string.Join(" ", "--value", valueText);
        parser.Parse(args);

        Assert.Equal(expectedText, Value);
    }

    [Fact]
    public void EscapeTest2()
    {
        var parser = new CommandParser(this);
        var expectedText = "\"";
        var valueText = "\"\\\"\"";
        var args = string.Join(" ", "--value", valueText);
        parser.Parse(args);

        Assert.Equal(expectedText, Value);
    }

    [CommandProperty]
    public string Value { get; set; } = string.Empty;
}
