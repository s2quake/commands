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

public class Method_FailTest
{
    sealed class Method1Class
    {
        [CommandMethod]
        internal int Method()
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void Base_Method1_Test()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMethodDescriptors(typeof(Method1Class)));
    }

    sealed class Method5Class
    {
        [CommandMethod]
        internal void Method(List<int> list)
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void Base_Method5_Test()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMethodDescriptors(typeof(Method5Class)));
    }
}
