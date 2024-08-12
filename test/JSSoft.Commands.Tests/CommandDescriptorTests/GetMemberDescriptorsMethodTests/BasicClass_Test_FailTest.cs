// <copyright file="BasicClass_Test_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable CA1822
using System.Reflection;
using static JSSoft.Commands.CommandDescriptor;

namespace JSSoft.Commands.Tests.CommandDescriptorTests.GetMemberDescriptorsMethodTests;

public sealed class BasicClass_Test_FailTest
{
    private static class StaticClass
    {
        [CommandProperty]
        public static int StaticValue { get; set; }
    }

    private static class StaticClassPropertyArray
    {
        [CommandPropertyArray]
        public static string[] Arguments { get; set; } = [];
    }

    [CommandMethod]
    internal int Method_WithReturnType()
    {
        return 0;
    }

    [CommandMethod]
    [CommandMethodProperty("abc")]
    internal void Method_WithNotFoundProperty()
    {
    }

    [CommandMethod]
    [CommandMethodStaticProperty("abc")]
    internal void Method_WithNotFoundStaticClass()
    {
    }

    [CommandMethod]
    [CommandMethodStaticProperty(typeof(StaticClass), "abc")]
    internal void Method_WithStaticClass_WithNotFoundProperty()
    {
    }

    [CommandMethod]
    [CommandMethodStaticProperty(typeof(StaticClassPropertyArray))]
    internal void Method_WithStaticClass_WithStaticPropertyArray(params string[] args)
    {
    }

    [CommandMethod]
    internal static string StaticMethod_WithReturnType()
    {
        return string.Empty;
    }

    [CommandMethod]
    [CommandMethodProperty("abc")]
    internal static void StaticMethod_WithNotFoundProperty()
    {
    }

    [CommandMethod]
    [CommandMethodStaticProperty("abc")]
    internal static void StaticMethod_WithNotFoundStaticClass()
    {
    }

    [CommandMethod]
    [CommandMethodStaticProperty(typeof(StaticClass), "abc")]
    internal static void StaticMethod_WithStaticClass_WithNotFoundProperty()
    {
    }

    [CommandMethod]
    [CommandMethodStaticProperty(typeof(StaticClassPropertyArray))]
    internal static void StaticMethod_WithStaticClass_WithStaticPropertyArray(params string[] args)
    {
    }

    [Theory]
    [InlineData(nameof(Method_WithReturnType))]
    [InlineData(nameof(Method_WithNotFoundProperty))]
    [InlineData(nameof(Method_WithStaticClass_WithNotFoundProperty))]
    [InlineData(nameof(Method_WithStaticClass_WithStaticPropertyArray))]
    public void InvalidMethod_ThrowTest(string methodName)
    {
        var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        var methodInfo = GetType().GetMethod(methodName, bindingFlags)!;
        var expectedValue = new CommandMemberInfo(methodInfo).ToString();
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            GetMemberDescriptors(this, methodName);
        });

        Assert.Equal(expectedValue, exception.Source);
    }

    [Theory]
    [InlineData(nameof(StaticMethod_WithReturnType))]
    [InlineData(nameof(StaticMethod_WithNotFoundProperty))]
    [InlineData(nameof(StaticMethod_WithNotFoundStaticClass))]
    [InlineData(nameof(StaticMethod_WithStaticClass_WithNotFoundProperty))]
    [InlineData(nameof(StaticMethod_WithStaticClass_WithStaticPropertyArray))]
    public void InvalidStaticMethod_ThrowTest(string methodName)
    {
        var bindingFlags = BindingFlags.Static | BindingFlags.NonPublic;
        var methodInfo = GetType().GetMethod(methodName, bindingFlags)!;
        var expectedValue = new CommandMemberInfo(methodInfo).ToString();
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            GetMemberDescriptors(this, methodName);
        });

        Assert.Equal(expectedValue, exception.Source);
    }
}
