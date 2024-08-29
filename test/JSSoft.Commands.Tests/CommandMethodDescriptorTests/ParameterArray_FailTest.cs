// <copyright file="ParameterArray_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable CA1822

using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Tests.CommandMethodDescriptorTests;

public class ParameterArray_FailTest
{
    [CommandMethod]
    internal void Method1([CommandParameterArray] string[] values1, params string[] values2)
    {
    }

    [Fact]
    public void Method1_FailTest()
    {
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        var declaringType = typeof(ParameterArray_FailTest);
        var methodInfo = declaringType.GetMethod(nameof(Method1), bindingFlags)!;
        var exception = Assert.Throws<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(declaringType, nameof(Method1)));
        var expectedValue = new CommandMemberInfo(methodInfo).ToString();

        Assert.Equal(expectedValue, exception.Source);
    }

    [CommandMethod]
    internal Task Method2Async([CommandParameterArray] string[] values1, params string[] values2)
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Method2_FailTest()
    {
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        var declaringType = typeof(ParameterArray_FailTest);
        var methodInfo = declaringType.GetMethod(nameof(Method2Async), bindingFlags)!;
        var exception = Assert.Throws<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(declaringType, nameof(Method2Async)));
        var expectedValue = new CommandMemberInfo(methodInfo).ToString();

        Assert.Equal(expectedValue, exception.Source);
    }

    [CommandMethod]
    internal void Method3(
        [CommandParameterArray] string[] values1, [CommandParameterArray] string[] values2)
    {
    }

    [Fact]
    public void Method3_FailTest()
    {
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        var declaringType = typeof(ParameterArray_FailTest);
        var methodInfo = declaringType.GetMethod(nameof(Method3), bindingFlags)!;
        var exception = Assert.Throws<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(declaringType, nameof(Method3)));
        var expectedValue = new CommandMemberInfo(methodInfo).ToString();

        Assert.Equal(expectedValue, exception.Source);
    }

    [CommandMethod]
    internal Task Method4Async(
        [CommandParameterArray] string[] values1, [CommandParameterArray] string[] values2)
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Method4_FailTest()
    {
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        var declaringType = typeof(ParameterArray_FailTest);
        var methodInfo = declaringType.GetMethod(nameof(Method4Async), bindingFlags)!;
        var exception = Assert.Throws<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(declaringType, nameof(Method4Async)));
        var expectedValue = new CommandMemberInfo(methodInfo).ToString();

        Assert.Equal(expectedValue, exception.Source);
    }

    [CommandMethod]
    internal Task Method5Async(
        [CommandParameterArray] string[] values1,
        [CommandParameterArray] string[] values2,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Method5_FailTest()
    {
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        var declaringType = typeof(ParameterArray_FailTest);
        var methodInfo = declaringType.GetMethod(nameof(Method5Async), bindingFlags)!;
        var exception = Assert.Throws<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(declaringType, nameof(Method5Async)));
        var expectedValue = new CommandMemberInfo(methodInfo).ToString();

        Assert.Equal(expectedValue, exception.Source);
    }
}
