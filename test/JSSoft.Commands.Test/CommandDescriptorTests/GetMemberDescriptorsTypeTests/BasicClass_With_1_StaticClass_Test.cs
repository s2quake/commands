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

public sealed class BasicClass_With_1_StaticClass_Test
{
    [CommandStaticProperty(typeof(StaticClass))]
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

    static class StaticClass
    {
        [CommandPropertyRequired]
        public static int StaticInt { get; set; }

        [CommandPropertySwitch]
        public static bool StaticBool { get; set; }

        [CommandProperty]
        [Browsable(true)]
        public static string StaticString { get; set; } = string.Empty;

        [CommandProperty]
        [Browsable(false)]
        public static float StaticFloat { get; set; }

        public static double StaticDouble { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClassType_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(BasicClass));

        Assert.Equal(7, memberDescriptors.Count);
        Assert.Equal(nameof(BasicClass.Int), memberDescriptors[0].MemberName);
        Assert.Equal(nameof(StaticClass.StaticInt), memberDescriptors[1].MemberName);
        Assert.Equal(nameof(BasicClass.Arguments), memberDescriptors[2].MemberName);
        Assert.Equal(nameof(BasicClass.String), memberDescriptors[3].MemberName);
        Assert.Equal(nameof(StaticClass.StaticString), memberDescriptors[4].MemberName);
        Assert.Equal(nameof(BasicClass.Bool), memberDescriptors[5].MemberName);
        Assert.Equal(nameof(StaticClass.StaticBool), memberDescriptors[6].MemberName);
    }

    [CommandStaticProperty(typeof(StaticClass), nameof(StaticClass.StaticInt))]
    sealed class BasicClass_With_1_StaticProperty
    {
        [CommandPropertyRequired]
        public int Int { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_BasicClass_With_1_StaticProperty_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(BasicClass_With_1_StaticProperty));

        Assert.Equal(2, memberDescriptors.Count);
        Assert.Equal(nameof(BasicClass_With_1_StaticProperty.Int), memberDescriptors[0].MemberName);
        Assert.Equal(nameof(StaticClass.StaticInt), memberDescriptors[1].MemberName);
    }

    [CommandStaticProperty(typeof(StaticClass),
        nameof(StaticClass.StaticInt),
        nameof(StaticClass.StaticBool)
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

        Assert.Equal(3, memberDescriptors.Count);
        Assert.Equal(nameof(BasicClass_With_2_StaticProperty.Int), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass.StaticInt), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass.StaticBool), memberDescriptors[index++].MemberName);
    }
}
