// <copyright file="AsyncMethod_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable CA1822

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Tests.CommandMethodDescriptorTests;

public class AsyncMethod_FailTest
{
    private sealed class Method1Class
    {
        [CommandMethod]
        internal int MethodAsync()
        {
            throw new NotSupportedException();
        }
    }

    [Fact]
    public void Base_Method1_Test()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(typeof(Method1Class)));
    }

    private sealed class Method2Class
    {
        [CommandMethod]
        internal Task MethodAsync(CancellationToken cancellationToken, int value)
        {
            throw new NotSupportedException();
        }
    }

    [Fact]
    public void Base_Method2_Test()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(typeof(Method2Class)));
    }

    private sealed class Method3Class
    {
        [CommandMethod]
        internal Task<int> MethodAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }

    [Fact]
    public void Base_Method3_Test()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(typeof(Method3Class)));
    }

    private sealed class Method4Class
    {
        [CommandMethod]
        internal Task MethodAsync(IList<int> list, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }

    [Fact]
    public void Base_Method4_Test()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(typeof(Method4Class)));
    }

    private sealed class Method5Class
    {
        [CommandMethod]
        internal Task MethodAsync(IProgress<float> progress, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }

    [Fact]
    public void Base_Method5_Test()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(typeof(Method5Class)));
    }

    private sealed class Method6Class
    {
        [CommandMethod]
        internal Task MethodAsync(IProgress<float> progress, int value)
        {
            throw new NotSupportedException();
        }
    }

    [Fact]
    public void Base_Method6_Test()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(typeof(Method6Class)));
    }

    private sealed class Method7Class
    {
        [CommandMethod]
        internal Task MethodAsync(IProgress<bool> progress)
        {
            throw new NotSupportedException();
        }
    }

    [Fact]
    public void Base_Method7_Test()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(typeof(Method7Class)));
    }

    private sealed class Method8Class
    {
        [CommandMethod]
        internal Task MethodAsync(IProgress<IntPtr> progress)
        {
            throw new NotSupportedException();
        }
    }

    [Fact]
    public void Base_Method8_Test()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(typeof(Method8Class)));
    }

    private sealed class Method9Class
    {
        [CommandMethod]
        internal Task MethodAsync(IProgress<string> progress)
        {
            throw new NotSupportedException();
        }
    }

    [Fact]
    public void Base_Method9_Test()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(typeof(Method9Class)));
    }
}
