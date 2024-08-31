// <copyright file="AsyncMethod_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable CA1822

using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Tests.CommandMethodDescriptorTests;

public class AsyncMethod_Test
{
    [CommandMethod]
    internal Task Method1Async()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Method1Async_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test));
        var methodDescriptor = methodDescriptors[nameof(Method1Async)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method1", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("method1", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method1Async), methodDescriptor.MethodName);
        Assert.True(methodDescriptor.IsAsync);
        Assert.Empty(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
    }

    [CommandMethod(Aliases = ["t1", "t2"])]
    internal Task Method2Async()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Method2Async_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test));
        var methodDescriptor = methodDescriptors[nameof(Method2Async)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method2", methodDescriptor.Name);
        Assert.Equivalent(new string[] { "t1", "t2" }, methodDescriptor.Aliases);
        Assert.Equal("method2, t1, t2", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method2Async), methodDescriptor.MethodName);
        Assert.True(methodDescriptor.IsAsync);
        Assert.Empty(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
    }

    [CommandMethod("test-method")]
    internal Task Method3Async()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Method3Async_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test));
        var methodDescriptor = methodDescriptors[nameof(Method3Async)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("test-method", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("test-method", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method3Async), methodDescriptor.MethodName);
        Assert.True(methodDescriptor.IsAsync);
        Assert.Empty(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
    }

    [CommandMethod]
    [DisplayName("display-method")]
    internal Task Method4Async()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Method4Async_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test));
        var methodDescriptor = methodDescriptors[nameof(Method4Async)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method4", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("display-method", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method4Async), methodDescriptor.MethodName);
        Assert.True(methodDescriptor.IsAsync);
        Assert.Empty(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
    }

    [CommandMethod]
    [Category("etc")]
    internal Task Method5Async()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Method5Async_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test));
        var methodDescriptor = methodDescriptors[nameof(Method5Async)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method5", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("method5", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method5Async), methodDescriptor.MethodName);
        Assert.True(methodDescriptor.IsAsync);
        Assert.Empty(methodDescriptor.Members);
        Assert.Equal("etc", methodDescriptor.Category);
    }

    [CommandMethod]
    internal Task Method6Async(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Method6_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test));
        var methodDescriptor = methodDescriptors[nameof(Method6Async)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method6", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("method6", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method6Async), methodDescriptor.MethodName);
        Assert.True(methodDescriptor.IsAsync);
        Assert.Empty(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
    }

    [CommandMethod]
    internal Task Method7Async(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Method7_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test));
        var methodDescriptor = methodDescriptors[nameof(Method7Async)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method7", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("method7", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method7Async), methodDescriptor.MethodName);
        Assert.True(methodDescriptor.IsAsync);
        Assert.Empty(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
    }

    [CommandMethod]
    internal Task Method8Async(IProgress<float> progress)
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Method8_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test));
        var methodDescriptor = methodDescriptors[nameof(Method8Async)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method8", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("method8", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method8Async), methodDescriptor.MethodName);
        Assert.True(methodDescriptor.IsAsync);
        Assert.Empty(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
    }

    [CommandMethod]
    internal Task Method9Async(CancellationToken cancellationToken, IProgress<float> progress)
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Method9_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test));
        var methodDescriptor = methodDescriptors[nameof(Method9Async)];
        Assert.IsAssignableFrom<CommandMethodDescriptor>(methodDescriptor);
        Assert.Equal("method9", methodDescriptor.Name);
        Assert.Empty(methodDescriptor.Aliases);
        Assert.Equal("method9", methodDescriptor.DisplayName);
        Assert.Equal(nameof(Method9Async), methodDescriptor.MethodName);
        Assert.True(methodDescriptor.IsAsync);
        Assert.Empty(methodDescriptor.Members);
        Assert.Empty(methodDescriptor.Category);
    }

    [CommandMethod]
    internal Task Method10Async(CancellationToken cancellationToken, IProgress<sbyte> progress)
        => Task.CompletedTask;

    [CommandMethod]
    internal Task Method11Async(CancellationToken cancellationToken, IProgress<byte> progress)
        => Task.CompletedTask;

    [CommandMethod]
    internal Task Method12Async(CancellationToken cancellationToken, IProgress<short> progress)
        => Task.CompletedTask;

    [CommandMethod]
    internal Task Method13Async(CancellationToken cancellationToken, IProgress<ushort> progress)
        => Task.CompletedTask;

    [CommandMethod]
    internal Task Method14Async(CancellationToken cancellationToken, IProgress<int> progress)
        => Task.CompletedTask;

    [CommandMethod]
    internal Task Method15Async(CancellationToken cancellationToken, IProgress<uint> progress)
        => Task.CompletedTask;

    [CommandMethod]
    internal Task Method16Async(CancellationToken cancellationToken, IProgress<long> progress)
        => Task.CompletedTask;

    [CommandMethod]
    internal Task Method17Async(CancellationToken cancellationToken, IProgress<ulong> progress)
        => Task.CompletedTask;

    [CommandMethod]
    internal Task Method18Async(CancellationToken cancellationToken, IProgress<float> progress)
        => Task.CompletedTask;

    [CommandMethod]
    internal Task Method19Async(CancellationToken cancellationToken, IProgress<double> progress)
        => Task.CompletedTask;

    [CommandMethod]
    internal Task Method20Async(CancellationToken cancellationToken, IProgress<decimal> progress)
        => Task.CompletedTask;

    [CommandMethod]
    internal Task Method21Async(
        CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
        => Task.CompletedTask;

    [Fact]
    public void Method_10_20_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test));
        var methodDescriptor10 = methodDescriptors[nameof(Method10Async)];
        var methodDescriptor11 = methodDescriptors[nameof(Method11Async)];
        var methodDescriptor12 = methodDescriptors[nameof(Method12Async)];
        var methodDescriptor13 = methodDescriptors[nameof(Method13Async)];
        var methodDescriptor14 = methodDescriptors[nameof(Method14Async)];
        var methodDescriptor15 = methodDescriptors[nameof(Method15Async)];
        var methodDescriptor16 = methodDescriptors[nameof(Method16Async)];
        var methodDescriptor17 = methodDescriptors[nameof(Method17Async)];
        var methodDescriptor18 = methodDescriptors[nameof(Method18Async)];
        var methodDescriptor19 = methodDescriptors[nameof(Method19Async)];
        var methodDescriptor20 = methodDescriptors[nameof(Method20Async)];
        var methodDescriptor21 = methodDescriptors[nameof(Method21Async)];
        Assert.Equal("method10", methodDescriptor10.Name);
        Assert.Equal("method11", methodDescriptor11.Name);
        Assert.Equal("method12", methodDescriptor12.Name);
        Assert.Equal("method13", methodDescriptor13.Name);
        Assert.Equal("method14", methodDescriptor14.Name);
        Assert.Equal("method15", methodDescriptor15.Name);
        Assert.Equal("method16", methodDescriptor16.Name);
        Assert.Equal("method17", methodDescriptor17.Name);
        Assert.Equal("method18", methodDescriptor18.Name);
        Assert.Equal("method19", methodDescriptor19.Name);
        Assert.Equal("method20", methodDescriptor20.Name);
        Assert.Equal("method21", methodDescriptor21.Name);
    }

    [Fact]
    public async Task InvokeAsync_Test()
    {
        var commandInvoker = new CommandInvoker(this);
        await commandInvoker.InvokeAsync("method10");
        await commandInvoker.InvokeAsync("method11");
        await commandInvoker.InvokeAsync("method12");
        await commandInvoker.InvokeAsync("method13");
        await commandInvoker.InvokeAsync("method14");
        await commandInvoker.InvokeAsync("method15");
        await commandInvoker.InvokeAsync("method16");
        await commandInvoker.InvokeAsync("method17");
        await commandInvoker.InvokeAsync("method18");
        await commandInvoker.InvokeAsync("method19");
        await commandInvoker.InvokeAsync("method20");
        await commandInvoker.InvokeAsync("method21");
        Assert.True(true);
    }
}
