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

using System.Collections;
using System.Threading;

namespace JSSoft.Commands.Test.CommandMemberDescriptorTests;

public class Property_General_FailTest
{
    sealed class CancellationTokenClass
    {
        [CommandProperty]
        public CancellationToken Member1 { get; set; }
    }

    [Fact]
    public void CancellationTokenClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(CancellationTokenClass)));
    }

    sealed class EnumerableClass
    {
        [CommandProperty]
        public IEnumerable Member1 { get; set; } = Array.Empty<object>();
    }

    [Fact]
    public void EnumerableClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(EnumerableClass)));
    }

    sealed class ListClass
    {
        [CommandProperty]
        public List<string> Member1 { get; set; } = [];
    }

    [Fact]
    public void ListClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(ListClass)));
    }

    sealed class DictionaryClass
    {
        [CommandProperty]
        public Dictionary<string, string> Member1 { get; set; } = [];
    }

    [Fact]
    public void DictionaryClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(DictionaryClass)));
    }
}
