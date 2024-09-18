// <copyright file="Validation_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace JSSoft.Commands.Tests.CommandInvokerTests;

#if !NETSTANDARD
public class Validation_Test
{
    private static class InstanceClass
    {
        public static string Value { get; set; } = string.Empty;

        [CommandMethod]
        public static void Test0(
            [RegularExpression(@"^\d+$")]
            string value)
        {
            Value = value;
        }
    }

    [Fact]
    public void Invoke_Test0_Test()
    {
        var invoker = new CommandInvoker(typeof(InstanceClass));
        InstanceClass.Value = string.Empty;
        invoker.Invoke("test0 1234");
        var actualValue = InstanceClass.Value;

        Assert.Equal("1234", actualValue);
    }

    [Fact]
    public void Invoke_Test0_FailTest()
    {
        var invoker = new CommandInvoker(typeof(InstanceClass));
        InstanceClass.Value = string.Empty;
        Assert.Throws<CommandLineException>(() => invoker.Invoke("test0 abc"));
    }
}
#endif // !NETSTANDARD
