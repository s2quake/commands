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

public sealed class BasicClass_With_2_StaticClass1_Test
{
    [CommandStaticProperty(typeof(StaticClass1))]
    [CommandStaticProperty(typeof(StaticClass2))]
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

    static class StaticClass1
    {
        [CommandPropertyRequired]
        public static int StaticInt1 { get; set; }

        [CommandPropertySwitch]
        public static bool StaticBool1 { get; set; }
    }

    static class StaticClass2
    {
        [CommandPropertyRequired]
        public static int StaticInt2 { get; set; }

        [CommandPropertySwitch]
        public static bool StaticBool2 { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClassType_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(BasicClass));
        var index = 0;

        Assert.Equal(8, memberDescriptors.Count);
        Assert.Equal(nameof(BasicClass.Int), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass1.StaticInt1), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass2.StaticInt2), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.Arguments), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.String), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.Bool), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass1.StaticBool1), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass2.StaticBool2), memberDescriptors[index++].MemberName);
    }

    [CommandStaticProperty(typeof(StaticClass1), nameof(StaticClass1.StaticInt1))]
    [CommandStaticProperty(typeof(StaticClass2), nameof(StaticClass2.StaticInt2))]
    sealed class BasicClass_With_1_StaticProperty
    {
        [CommandPropertyRequired]
        public int Int { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_BasicClass_With_1_StaticProperty_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(BasicClass_With_1_StaticProperty));
        var index = 0;

        Assert.Equal(3, memberDescriptors.Count);
        Assert.Equal(nameof(BasicClass_With_1_StaticProperty.Int), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass1.StaticInt1), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass2.StaticInt2), memberDescriptors[index++].MemberName);
    }

    [CommandStaticProperty(typeof(StaticClass1),
        nameof(StaticClass1.StaticInt1),
        nameof(StaticClass1.StaticBool1)
    )]
    [CommandStaticProperty(typeof(StaticClass2),
        nameof(StaticClass2.StaticInt2),
        nameof(StaticClass2.StaticBool2)
    )]
    sealed class BasicClass_With_2_StaticProperty
    {
        [CommandPropertyRequired]
        public int Int { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_With_2_StaticProperty_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(BasicClass_With_2_StaticProperty));
        var index = 0;

        Assert.Equal(5, memberDescriptors.Count);
        Assert.Equal(nameof(BasicClass_With_2_StaticProperty.Int), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass1.StaticInt1), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass2.StaticInt2), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass1.StaticBool1), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass2.StaticBool2), memberDescriptors[index++].MemberName);
    }
}
