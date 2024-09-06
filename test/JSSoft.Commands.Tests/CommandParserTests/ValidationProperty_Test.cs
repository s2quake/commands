// <copyright file="ValidationProperty_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#if !NETSTANDARD
using System.ComponentModel.DataAnnotations;

namespace JSSoft.Commands.Tests.CommandParserTests;

public class ValidationProperty_Test
{
    private sealed class RegularExpressionClass
    {
        [CommandProperty]
        [RegularExpression(@"^\d+")]
        public string Member1 { get; set; } = string.Empty;
    }

    [Fact]
    public void RegularExpressionClass_Member1_Test()
    {
        var obj = new RegularExpressionClass();
        var parser = new CommandParser(obj);
        var argumentLine = $"--member1 123";
        parser.Parse(argumentLine);
        var actualValue = obj.Member1;

        Assert.Equal("123", actualValue);
    }

    [Fact]
    public void RegularExpressionClass_Member1_FailTest()
    {
        var obj = new RegularExpressionClass();
        var parser = new CommandParser(obj);
        var argumentLine = $"--member1 test";
        Assert.Throws<CommandLineException>(() => parser.Parse(argumentLine));
    }

    private static class RegularExpressionStaticClass
    {
        [CommandProperty]
        [RegularExpression(@"^\d+")]
        public static string Member1 { get; set; } = string.Empty;
    }

    [Fact]
    public void RegularExpressionStaticClass_Member1_Test()
    {
        var parser = new CommandParser(typeof(RegularExpressionStaticClass));
        var argumentLine = $"--member1 123";
        parser.Parse(argumentLine);
        var actualValue = RegularExpressionStaticClass.Member1;

        Assert.Equal("123", actualValue);
    }

    [Fact]
    public void RegularExpressionStaticClass_Member1_FailTest()
    {
        var parser = new CommandParser(typeof(RegularExpressionStaticClass));
        var argumentLine = $"--member1 test";
        Assert.Throws<CommandLineException>(() => parser.Parse(argumentLine));
    }

    private sealed class MinLengthClass
    {
        [CommandProperty]
        [MinLength(2)]
        public string Member1 { get; set; } = string.Empty;
    }

    [Fact]
    public void MinLengthClass_Member1_Test()
    {
        var obj = new MinLengthClass();
        var parser = new CommandParser(obj);
        var argumentLine = $"--member1 12";
        parser.Parse(argumentLine);
        var actualValue = obj.Member1;

        Assert.Equal("12", actualValue);
    }

    [Fact]
    public void MinLengthClass_Member1_FailTest()
    {
        var obj = new MinLengthClass();
        var parser = new CommandParser(obj);
        var argumentLine = $"--member1 1";
        Assert.Throws<CommandLineException>(() => parser.Parse(argumentLine));
    }

    private static class MinLengthStaticClass
    {
        [CommandProperty]
        [MinLength(2)]
        public static string Member1 { get; set; } = string.Empty;
    }

    [Fact]
    public void MinLengthStaticClass_Member1_Test()
    {
        var parser = new CommandParser(typeof(MinLengthStaticClass));
        var argumentLine = $"--member1 12";
        parser.Parse(argumentLine);
        var actualValue = MinLengthStaticClass.Member1;

        Assert.Equal("12", actualValue);
    }

    [Fact]
    public void MinLengthStaticClass_Member1_FailTest()
    {
        var parser = new CommandParser(typeof(MinLengthStaticClass));
        var argumentLine = $"--member1 1";
        Assert.Throws<CommandLineException>(() => parser.Parse(argumentLine));
    }
}
#endif // !NETSTANDARD
