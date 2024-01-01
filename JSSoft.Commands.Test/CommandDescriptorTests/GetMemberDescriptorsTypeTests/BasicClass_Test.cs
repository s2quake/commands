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

public sealed class BasicClass_Test
{
    sealed class BasicClass
    {
        [CommandPropertyRequired]
        public int Int { get; set; }

        [CommandPropertySwitch]
        public bool Bool { get; set; }

        [CommandProperty]
        [Browsable(true)]
        public string String { get; set; } = string.Empty;

        [CommandProperty]
        [Browsable(false)]
        public float Float { get; set; }

        public double Double { get; set; }

        [CommandPropertyArray]
        public string[] Arguments { get; set; } = [];
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_Test()
    {
        var obj = new BasicClass();
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(obj.GetType());
        var index = 0;

        Assert.Equal(4, memberDescriptors.Count);
        Assert.Equal(nameof(BasicClass.Int), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.Arguments), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.String), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.Bool), memberDescriptors[index++].MemberName);
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClassType_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(BasicClass));
        var index = 0;

        Assert.Equal(4, memberDescriptors.Count);
        Assert.Equal(nameof(BasicClass.Int), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.Arguments), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.String), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.Bool), memberDescriptors[index++].MemberName);
    }

    sealed class MultiplePropertyArrayInvalidClass
    {
        [CommandPropertyArray]
        public string[] Arguments1 { get; set; } = [];

        [CommandPropertyArray]
        public string[] Arguments2 { get; set; } = [];
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_MultiplePropertyArrayInvalidClass_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(typeof(MultiplePropertyArrayInvalidClass));
        });
        Assert.Equal(typeof(MultiplePropertyArrayInvalidClass).AssemblyQualifiedName, exception.Source);
    }
}
