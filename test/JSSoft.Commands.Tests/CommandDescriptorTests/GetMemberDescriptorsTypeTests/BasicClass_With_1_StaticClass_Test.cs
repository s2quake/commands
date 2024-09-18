// <copyright file="BasicClass_With_1_StaticClass_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using static JSSoft.Commands.CommandDescriptor;

namespace JSSoft.Commands.Tests.CommandDescriptorTests.GetMemberDescriptorsTypeTests;

public sealed class BasicClass_With_1_StaticClass_Test
{
    [CommandStaticProperty(typeof(StaticClass))]
    private sealed class BasicClass
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

    private static class StaticClass
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
        var memberDescriptors = GetMemberDescriptors(typeof(BasicClass));
        var index = 0;
        Assert.Equal(7, memberDescriptors.Count);
        Assert.Equal(nameof(BasicClass.Int), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass.StaticInt), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.Arguments), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.Bool), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.String), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass.StaticBool), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass.StaticString), memberDescriptors[index++].MemberName);
        Assert.Equal(7, index);
    }

    [CommandStaticProperty(typeof(StaticClass), nameof(StaticClass.StaticInt))]
    private sealed class BasicClass_With_1_StaticProperty
    {
        [CommandPropertyRequired]
        public int Int { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_BasicClass_With_1_StaticProperty_Test()
    {
        var memberDescriptors = GetMemberDescriptors(typeof(BasicClass_With_1_StaticProperty));

        Assert.Equal(2, memberDescriptors.Count);
        Assert.Equal(nameof(BasicClass_With_1_StaticProperty.Int), memberDescriptors[0].MemberName);
        Assert.Equal(nameof(StaticClass.StaticInt), memberDescriptors[1].MemberName);
    }

    [CommandStaticProperty(
        typeof(StaticClass),
        nameof(StaticClass.StaticInt),
        nameof(StaticClass.StaticBool))]
    private sealed class BasicClass_With_2_StaticProperty
    {
        [CommandPropertyRequired]
        public int Int { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_With_2_StaticProperty_Test()
    {
        var memberDescriptors = GetMemberDescriptors(typeof(BasicClass_With_2_StaticProperty));
        var index = 0;

        Assert.Equal(3, memberDescriptors.Count);
        Assert.Equal(
            nameof(BasicClass_With_2_StaticProperty.Int), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass.StaticInt), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass.StaticBool), memberDescriptors[index++].MemberName);

        Assert.Equal(3, index);
    }
}
