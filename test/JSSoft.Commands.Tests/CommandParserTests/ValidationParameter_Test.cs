// <copyright file="ValidationParameter_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#if !NETSTANDARD
using System.ComponentModel.DataAnnotations;

namespace JSSoft.Commands.Tests.CommandParserTests;

public class ValidationParameter_Test
{
    private sealed class RegularExpressionMethodClass
    {
        public string Value { get; set; } = string.Empty;

        [CommandMethod]
        public void Method1([RegularExpression(@"^\d+")] string value)
        {
            Value = value;
        }
    }

    [Fact]
    public void RegularExpressionClass_Method1_Test()
    {
        var obj = new RegularExpressionMethodClass();
        var invoker = new CommandInvoker(obj);
        var argumentLine = $"method1 1";
        invoker.Invoke(argumentLine);
        Assert.Equal("1", obj.Value);
    }

    [Fact]
    public void RegularExpressionClass_Method1_FailTest()
    {
        var obj = new RegularExpressionMethodClass();
        var invoker = new CommandInvoker(obj);
        var argumentLine = $"method1 abc";
        Assert.Throws<CommandLineException>(() => invoker.Invoke(argumentLine));
    }

    private static class RegularExpressionMethodStaticClass
    {
        public static string Value { get; set; } = string.Empty;

        [CommandMethod]
        public static void Method1([RegularExpression(@"^\d+")] string value)
        {
            Value = value;
        }
    }

    [Fact]
    public void RegularExpressionMethodStaticClass_Method1_Test()
    {
        var invoker = new CommandInvoker(typeof(RegularExpressionMethodStaticClass));
        var argumentLine = $"method1 1";
        invoker.Invoke(argumentLine);
        Assert.Equal("1", RegularExpressionMethodStaticClass.Value);
    }

    [Fact]
    public void RegularExpressionMethodStaticClass_Method1_FailTest()
    {
        var invoker = new CommandInvoker(typeof(RegularExpressionMethodStaticClass));
        var argumentLine = $"method1 abc";
        Assert.Throws<CommandLineException>(() => invoker.Invoke(argumentLine));
    }

    // 123
    private sealed class MinLengthMethodClass
    {
        public string Value { get; set; } = string.Empty;

        [CommandMethod]
        public void Method1([MinLength(2)] string value)
        {
            Value = value;
        }
    }

    [Fact]
    public void MinLengthClass_Method1_Test()
    {
        var obj = new MinLengthMethodClass();
        var invoker = new CommandInvoker(obj);
        var argumentLine = $"method1 12";
        invoker.Invoke(argumentLine);
        Assert.Equal("12", obj.Value);
    }

    [Fact]
    public void MinLengthClass_Method1_FailTest()
    {
        var obj = new MinLengthMethodClass();
        var invoker = new CommandInvoker(obj);
        var argumentLine = $"method1 1";
        Assert.Throws<CommandLineException>(() => invoker.Invoke(argumentLine));
    }

    private static class MinLengthMethodStaticClass
    {
        public static string Value { get; set; } = string.Empty;

        [CommandMethod]
        public static void Method1([MinLength(2)] string value)
        {
            Value = value;
        }
    }

    [Fact]
    public void MinLengthMethodStaticClass_Method1_Test()
    {
        var invoker = new CommandInvoker(typeof(MinLengthMethodStaticClass));
        var argumentLine = $"method1 12";
        invoker.Invoke(argumentLine);
        Assert.Equal("12", MinLengthMethodStaticClass.Value);
    }

    [Fact]
    public void MinLengthMethodStaticClass_Method1_FailTest()
    {
        var invoker = new CommandInvoker(typeof(MinLengthMethodStaticClass));
        var argumentLine = $"method1 1";
        Assert.Throws<CommandLineException>(() => invoker.Invoke(argumentLine));
    }
}
#endif // !NETSTANDARD
