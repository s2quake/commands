// <copyright file="ParameterArray_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable CA1822

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Tests.CommandMethodDescriptorTests;

public class ParameterArray_Test
{
    [CommandMethod]
    internal void Method1([CommandParameterArray] string[] values)
    {
    }

    [Fact]
    public void Method1_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(ParameterArray_Test));
        var methodDescriptor = methodDescriptors[nameof(Method1)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method1", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("method1", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method1), methodDescriptor.MethodName);
        Assert.False(methodDescriptor.IsAsync);
        Assert.Single(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
        Assert.True(methodDescriptor.Members[0].IsVariables);
    }

    [CommandMethod]
    internal void Method2(params string[] values)
    {
    }

    [Fact]
    public void Method2_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(ParameterArray_Test));
        var methodDescriptor = methodDescriptors[nameof(Method2)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method2", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("method2", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method2), methodDescriptor.MethodName);
        Assert.False(methodDescriptor.IsAsync);
        Assert.Single(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
        Assert.True(methodDescriptor.Members[0].IsVariables);
    }

    [CommandMethod]
    internal Task Method3Async(
        [CommandParameterArray] string[] values)
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Method3_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(ParameterArray_Test));
        var methodDescriptor = methodDescriptors[nameof(Method3Async)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method3", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("method3", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method3Async), methodDescriptor.MethodName);
        Assert.True(methodDescriptor.IsAsync);
        Assert.Single(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
        Assert.True(methodDescriptor.Members[0].IsVariables);
    }

    [CommandMethod]
    internal Task Method4Async(
        [CommandParameterArray] string[] values, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Method4_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(ParameterArray_Test));
        var methodDescriptor = methodDescriptors[nameof(Method4Async)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method4", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("method4", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method4Async), methodDescriptor.MethodName);
        Assert.True(methodDescriptor.IsAsync);
        Assert.Single(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
        Assert.True(methodDescriptor.Members[0].IsVariables);
    }

    [CommandMethod]
    internal Task Method5Async(
        params string[] values)
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Method5_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(ParameterArray_Test));
        var methodDescriptor = methodDescriptors[nameof(Method5Async)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method5", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("method5", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method5Async), methodDescriptor.MethodName);
        Assert.True(methodDescriptor.IsAsync);
        Assert.Single(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
        Assert.True(methodDescriptor.Members[0].IsVariables);
    }
}
