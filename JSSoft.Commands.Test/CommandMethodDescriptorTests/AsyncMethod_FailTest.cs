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

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Test.CommandMethodDescriptorTests;

public class AsyncMethod_FailTest
{
    sealed class Method1Class
    {
        [CommandMethod]
        internal int MethodAsync()
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void Base_Method1_Test()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMethodDescriptors(typeof(Method1Class)));
    }

    sealed class Method2Class
    {
        [CommandMethod]
        internal Task MethodAsync(CancellationToken cancellationToken, int value)
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void Base_Method2_Test()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMethodDescriptors(typeof(Method2Class)));
    }

    sealed class Method3Class
    {
        [CommandMethod]
        internal Task<int> MethodAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void Base_Method3_Test()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMethodDescriptors(typeof(Method3Class)));
    }

    sealed class Method4Class
    {
        [CommandMethod]
        internal Task MethodAsync(IList<int> list, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void Base_Method4_Test()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMethodDescriptors(typeof(Method4Class)));
    }

    sealed class Method5Class
    {
        [CommandMethod]
        internal Task MethodAsync(IProgress<float> progress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void Base_Method5_Test()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMethodDescriptors(typeof(Method5Class)));
    }

    sealed class Method6Class
    {
        [CommandMethod]
        internal Task MethodAsync(IProgress<float> progress, int value)
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void Base_Method6_Test()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMethodDescriptors(typeof(Method6Class)));
    }

    sealed class Method7Class
    {
        [CommandMethod]
        internal Task MethodAsync(IProgress<bool> progress)
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void Base_Method7_Test()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMethodDescriptors(typeof(Method7Class)));
    }

    sealed class Method8Class
    {
        [CommandMethod]
        internal Task MethodAsync(IProgress<IntPtr> progress)
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void Base_Method8_Test()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMethodDescriptors(typeof(Method8Class)));
    }

    sealed class Method9Class
    {
        [CommandMethod]
        internal Task MethodAsync(IProgress<string> progress)
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void Base_Method9_Test()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMethodDescriptors(typeof(Method9Class)));
    }
}
