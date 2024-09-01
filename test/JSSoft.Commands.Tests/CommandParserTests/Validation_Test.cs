// <copyright file="Validation_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace JSSoft.Commands.Tests.CommandParserTests;

#if !NETSTANDARD
public class Validation_Test
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
}
#endif // !NETSTANDARD
