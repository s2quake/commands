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

using System.ComponentModel;

namespace JSSoft.Commands.Test.CommandDescriptorTests.GetMemberDescriptorsTypeTests;

public sealed class StaticClass_MultiplePropertyArray_FailTest
{
    static class StaticClass
    {
        [CommandPropertyArray]
        public static string[] Arguments1 { get; set; } = [];

        [CommandPropertyArray]
        public static string[] Arguments2 { get; set; } = [];
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticClassType_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(typeof(StaticClass));
        });
        Assert.Equal(typeof(StaticClass).AssemblyQualifiedName, exception.Source);
    }
}
