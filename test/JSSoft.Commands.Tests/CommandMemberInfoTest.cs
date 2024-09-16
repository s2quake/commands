// <copyright file="CommandMemberInfoTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Reflection;

namespace JSSoft.Commands.Tests;

[CommandSummary("CommandMemberInfoTest: Summary")]
[CommandDescription("CommandMemberInfoTest: Description")]
[CommandExample("CommandMemberInfoTest: Example")]
public class CommandMemberInfoTest
{
    [Fact]
    public void Type_Test()
    {
        var memberInfo = new CommandMemberInfo(typeof(CommandMemberInfoTest));
        var usage = CommandDescriptor.GetUsage(memberInfo);

        Assert.Equal(typeof(CommandMemberInfoTest), memberInfo.Value);
        Assert.Equal(CommandMemberType.Type, memberInfo.Type);
        Assert.Equal(nameof(CommandMemberInfoTest), memberInfo.Name);
        Assert.Equal(nameof(CommandMemberInfoTest), memberInfo.Identifier);
        Assert.Equal(
            expected: $"JSSoft.Commands.Tests.{nameof(CommandMemberInfoTest)}",
            actual: memberInfo.FullName);
        Assert.Equal($"JSSoft.Commands.Tests", memberInfo.Namespace);
        Assert.Equal(typeof(CommandMemberInfoTest), memberInfo.DeclaringType);
        Assert.Equal(
            expected: "CommandMemberInfoTest: Summary",
            actual: usage.Summary);
        Assert.Equal(
            expected: "CommandMemberInfoTest: Description",
            actual: usage.Description);
        Assert.Equal(
            expected: "CommandMemberInfoTest: Example",
            actual: usage.Example);
    }

    [Fact]
    public void Property_Test()
    {
        var type = typeof(CommandMemberInfoTest);
        var propertyInfo = type.GetProperty(nameof(Property))!;
        var memberInfo = new CommandMemberInfo(propertyInfo);
        var usage = CommandDescriptor.GetUsage(memberInfo);

        Assert.Equal(propertyInfo, memberInfo.Value);
        Assert.Equal(CommandMemberType.Property, memberInfo.Type);
        Assert.Equal(nameof(Property), memberInfo.Name);
        Assert.Equal(
            expected: $"{nameof(CommandMemberInfoTest)}.{nameof(Property)}",
            actual: memberInfo.Identifier);
        Assert.Equal(
            expected: $"JSSoft.Commands.Tests.{nameof(CommandMemberInfoTest)}.{nameof(Property)}",
            actual: memberInfo.FullName);
        Assert.Equal($"JSSoft.Commands.Tests", memberInfo.Namespace);
        Assert.Equal(typeof(CommandMemberInfoTest), memberInfo.DeclaringType);
        Assert.Equal(
            expected: "Property: Summary",
            actual: usage.Summary);
        Assert.Equal(
            expected: "Property: Description",
            actual: usage.Description);
        Assert.Equal(
            expected: "Property: Example",
            actual: usage.Example);
    }

    [Fact]
    public void Method1_Test()
    {
        var type = typeof(CommandMemberInfoTest);
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        var methodInfo = type.GetMethod(nameof(Method1), bindingFlags)!;
        var memberInfo = new CommandMemberInfo(methodInfo);
        var usage = CommandDescriptor.GetUsage(memberInfo);

        Assert.Equal(methodInfo, memberInfo.Value);
        Assert.Equal(CommandMemberType.Method, memberInfo.Type);
        Assert.Equal(nameof(Method1), memberInfo.Name);
        Assert.Equal(
            expected: $"{nameof(CommandMemberInfoTest)}.{nameof(Method1)}",
            actual: memberInfo.Identifier);
        Assert.Equal(
            expected: $"JSSoft.Commands.Tests.{nameof(CommandMemberInfoTest)}.{nameof(Method1)}",
            actual: memberInfo.FullName);
        Assert.Equal($"JSSoft.Commands.Tests", memberInfo.Namespace);
        Assert.Equal(typeof(CommandMemberInfoTest), memberInfo.DeclaringType);
        Assert.Equal(
            expected: "Method1: Summary",
            actual: usage.Summary);
        Assert.Equal(
            expected: "Method1: Description",
            actual: usage.Description);
        Assert.Equal(
            expected: "Method1: Example",
            actual: usage.Example);
    }

    [Fact]
    public void Method1_Parameter_Test()
    {
        var type = typeof(CommandMemberInfoTest);
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        var methodInfo = type.GetMethod(nameof(Method1), bindingFlags)!;
        var parameterInfo = methodInfo.GetParameters()[0];
        var memberInfo = new CommandMemberInfo(parameterInfo);
        var usage = CommandDescriptor.GetUsage(memberInfo);

        Assert.Equal(parameterInfo, memberInfo.Value);
        Assert.Equal(CommandMemberType.Parameter, memberInfo.Type);
        Assert.Equal("parameter", memberInfo.Name);
        Assert.Equal(
            expected: $"{type.Name}.{nameof(Method1)}.parameter",
            actual: memberInfo.Identifier);
        Assert.Equal(
            expected: $"JSSoft.Commands.Tests.{type.Name}.{nameof(Method1)}.parameter",
            actual: memberInfo.FullName);
        Assert.Equal($"JSSoft.Commands.Tests", memberInfo.Namespace);
        Assert.Equal(typeof(CommandMemberInfoTest), memberInfo.DeclaringType);
        Assert.Equal(
            expected: "parameter: Summary",
            actual: usage.Summary);
        Assert.Equal(
            expected: "parameter: Description",
            actual: usage.Description);
        Assert.Equal(
            expected: "parameter: Example",
            actual: usage.Example);
    }

    [Fact]
    public void Method2Async_Test()
    {
        var type = typeof(CommandMemberInfoTest);
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        var methodInfo = type.GetMethod(nameof(Method2Async), bindingFlags)!;
        var memberInfo = new CommandMemberInfo(methodInfo);
        var usage = CommandDescriptor.GetUsage(memberInfo);

        Assert.Equal(methodInfo, memberInfo.Value);
        Assert.Equal(CommandMemberType.Method, memberInfo.Type);
        Assert.Equal(nameof(Method2Async), memberInfo.Name);
        Assert.Equal(
            expected: $"{nameof(CommandMemberInfoTest)}.{nameof(Method2Async)}",
            actual: memberInfo.Identifier);
        Assert.Equal(
            expected: $"JSSoft.Commands.Tests.{nameof(CommandMemberInfoTest)}.{nameof(Method2Async)}",
            actual: memberInfo.FullName);
        Assert.Equal($"JSSoft.Commands.Tests", memberInfo.Namespace);
        Assert.Equal(typeof(CommandMemberInfoTest), memberInfo.DeclaringType);
        Assert.Equal(
            expected: "Method2Async: Summary",
            actual: usage.Summary);
        Assert.Equal(
            expected: "Method2Async: Description",
            actual: usage.Description);
        Assert.Equal(
            expected: "Method2Async: Example",
            actual: usage.Example);
    }

    [CommandProperty]
    [CommandSummary("Property: Summary")]
    [CommandDescription("Property: Description")]
    [CommandExample("Property: Example")]
    public int Property { get; set; }

    [CommandMethod]
    [CommandSummary("Method1: Summary")]
    [CommandDescription("Method1: Description")]
    [CommandExample("Method1: Example")]
    internal void Method1(
        [CommandSummary("parameter: Summary")]
        [CommandDescription("parameter: Description")]
        [CommandExample("parameter: Example")]
        int parameter)
    {
        // do nothing
    }

    [CommandMethod]
    [CommandSummary("Method2Async: Summary")]
    [CommandDescription("Method2Async: Description")]
    [CommandExample("Method2Async: Example")]
    internal void Method2Async()
    {
        // do nothing
    }
}
