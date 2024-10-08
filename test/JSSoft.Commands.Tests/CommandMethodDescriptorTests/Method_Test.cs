// <copyright file="Method_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable CA1822

using System.ComponentModel;

namespace JSSoft.Commands.Tests.CommandMethodDescriptorTests;

public class Method_Test
{
    [CommandMethod]
    internal void Method1()
    {
    }

    [Fact]
    public void Method1_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(Method_Test));
        var methodDescriptor = methodDescriptors[nameof(Method1)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method1", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("method1", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method1), methodDescriptor.MethodName);
        Assert.False(methodDescriptor.IsAsync);
        Assert.Empty(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
    }

    [CommandMethod(Aliases = ["t1", "t2"])]
    internal void Method2()
    {
    }

    [Fact]
    public void Method2_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(Method_Test));
        var methodDescriptor = methodDescriptors[nameof(Method2)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method2", methodDescriptor.Name);
        Assert.Equivalent(new string[] { "t1", "t2" }, methodDescriptor.Aliases);
        Assert.Equal("method2, t1, t2", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method2), methodDescriptor.MethodName);
        Assert.False(methodDescriptor.IsAsync);
        Assert.Empty(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
    }

    [CommandMethod("test-method")]
    internal void Method3()
    {
    }

    [Fact]
    public void Method3_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(Method_Test));
        var methodDescriptor = methodDescriptors[nameof(Method3)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("test-method", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("test-method", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method3), methodDescriptor.MethodName);
        Assert.False(methodDescriptor.IsAsync);
        Assert.Empty(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
    }

    [CommandMethod]
    [DisplayName("display-method")]
    internal void Method4()
    {
    }

    [Fact]
    public void Method4_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(Method_Test));
        var methodDescriptor = methodDescriptors[nameof(Method4)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method4", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("display-method", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method4), methodDescriptor.MethodName);
        Assert.False(methodDescriptor.IsAsync);
        Assert.Empty(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
    }

    [CommandMethod]
    [Category("etc")]
    internal void Method5()
    {
    }

    [Fact]
    public void Method5_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(Method_Test));
        var methodDescriptor = methodDescriptors[nameof(Method5)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method5", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("method5", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method5), methodDescriptor.MethodName);
        Assert.False(methodDescriptor.IsAsync);
        Assert.Empty(methodDescriptor.Members);
        Assert.Equal("etc", methodDescriptor.Category);
    }
}
