// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

#pragma warning disable CA1822

using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Test.CommandMethodDescriptorTests;

public class AsyncMethod_Test
{
    [CommandMethod]
    internal Task Method1Async()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Base_Method1Async_Test()
    {
        var methodDescriptor = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method1Async)];
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
    public void Base_Method2Async_Test()
    {
        var methodDescriptor = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method2Async)];
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
    public void Base_Method3Async_Test()
    {
        var methodDescriptor = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method3Async)];
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
    public void Base_Method4Async_Test()
    {
        var methodDescriptor = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method4Async)];
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
    public void Base_Method5Async_Test()
    {
        var methodDescriptor = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method5Async)];
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
    public void Base_Method6_Test()
    {
        var methodDescriptor = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method6Async)];
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
    public void Base_Method7_Test()
    {
        var methodDescriptor = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method7Async)];
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
    public void Base_Method8_Test()
    {
        var methodDescriptor = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method8Async)];
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
    public void Base_Method9_Test()
    {
        var methodDescriptor = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method9Async)];
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
    internal Task Method10Async(CancellationToken cancellationToken, IProgress<sbyte> progress) => Task.CompletedTask;
    [CommandMethod]
    internal Task Method11Async(CancellationToken cancellationToken, IProgress<byte> progress) => Task.CompletedTask;
    [CommandMethod]
    internal Task Method12Async(CancellationToken cancellationToken, IProgress<short> progress) => Task.CompletedTask;
    [CommandMethod]
    internal Task Method13Async(CancellationToken cancellationToken, IProgress<ushort> progress) => Task.CompletedTask;
    [CommandMethod]
    internal Task Method14Async(CancellationToken cancellationToken, IProgress<int> progress) => Task.CompletedTask;
    [CommandMethod]
    internal Task Method15Async(CancellationToken cancellationToken, IProgress<uint> progress) => Task.CompletedTask;
    [CommandMethod]
    internal Task Method16Async(CancellationToken cancellationToken, IProgress<long> progress) => Task.CompletedTask;
    [CommandMethod]
    internal Task Method17Async(CancellationToken cancellationToken, IProgress<ulong> progress) => Task.CompletedTask;
    [CommandMethod]
    internal Task Method18Async(CancellationToken cancellationToken, IProgress<float> progress) => Task.CompletedTask;
    [CommandMethod]
    internal Task Method19Async(CancellationToken cancellationToken, IProgress<double> progress) => Task.CompletedTask;
    [CommandMethod]
    internal Task Method20Async(CancellationToken cancellationToken, IProgress<decimal> progress) => Task.CompletedTask;
    [CommandMethod]
    internal Task Method21Async(CancellationToken cancellationToken, IProgress<ProgressInfo> progress) => Task.CompletedTask;

    [Fact]
    public void Base_Method_10_20_Test()
    {
        var methodDescriptor10 = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method10Async)];
        var methodDescriptor11 = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method11Async)];
        var methodDescriptor12 = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method12Async)];
        var methodDescriptor13 = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method13Async)];
        var methodDescriptor14 = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method14Async)];
        var methodDescriptor15 = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method15Async)];
        var methodDescriptor16 = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method16Async)];
        var methodDescriptor17 = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method17Async)];
        var methodDescriptor18 = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method18Async)];
        var methodDescriptor19 = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method19Async)];
        var methodDescriptor20 = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method20Async)];
        var methodDescriptor21 = CommandDescriptor.GetMethodDescriptors(typeof(AsyncMethod_Test))[nameof(Method21Async)];
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
    }
}
