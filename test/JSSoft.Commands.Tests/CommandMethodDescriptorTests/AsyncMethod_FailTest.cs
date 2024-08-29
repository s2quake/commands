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
    public void Method1_Test()
    {
        var declaringType = typeof(Method1Class);
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(declaringType));
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
    public void Method2_Test()
    {
        var declaringType = typeof(Method2Class);
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(declaringType));
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
    public void Method3_Test()
    {
        var declaringType = typeof(Method3Class);
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(declaringType));
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
    public void Method4_Test()
    {
        var declaringType = typeof(Method4Class);
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(declaringType));
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
    public void Method5_Test()
    {
        var declaringType = typeof(Method5Class);
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(declaringType));
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
    public void Method6_Test()
    {
        var declaringType = typeof(Method6Class);
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(declaringType));
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
    public void Method7_Test()
    {
        var declaringType = typeof(Method7Class);
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(declaringType));
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
    public void Method8_Test()
    {
        var declaringType = typeof(Method8Class);
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(declaringType));
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
    public void Method9_Test()
    {
        var declaringType = typeof(Method9Class);
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(declaringType));
    }
}
