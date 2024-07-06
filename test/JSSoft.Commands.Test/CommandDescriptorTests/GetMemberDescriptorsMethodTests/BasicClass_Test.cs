// <copyright file="BasicClass_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable CA1822

using System.Reflection;

namespace JSSoft.Commands.Test.CommandDescriptorTests.GetMemberDescriptorsMethodTests;

public sealed class BasicClass_Test
{
    static class StaticClass
    {
        [CommandProperty]
        public static int StaticValue1 { get; set; }

        [CommandProperty]
        public static int StaticValue2 { get; set; }
    }

    [CommandProperty]
    public int Value1 { get; set; }

    [CommandProperty]
    public static string Value2 { get; set; } = string.Empty;

    [CommandMethod]
    internal void Method()
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_Method_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(Method));

        Assert.Empty(memberDescriptors);
    }

    [CommandMethod]
    internal void Method_WithParam1(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_Method_WithParam1_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(Method_WithParam1));

        Assert.Equal(1, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[0].MemberName);
    }

    [CommandMethod]
    internal void Method_WithParam2(int @int, params int[] args)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_Method_WithParam2_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(Method_WithParam2));

        Assert.Equal(2, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[0].MemberName);
        Assert.Equal("args", memberDescriptors[1].MemberName);
    }

    [CommandMethod]
    [CommandMethodProperty(nameof(Value1))]
    internal void Method_WithParam1_WithProperty1(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_Method_WithParam1_WithProperty1_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(Method_WithParam1_WithProperty1));

        Assert.Equal(2, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[0].MemberName);
        Assert.Equal(nameof(Value1), memberDescriptors[1].MemberName);
    }

    [CommandMethod]
    [CommandMethodProperty(nameof(Value1), nameof(Value2))]
    internal void Method_WithParam1_WithProperty2(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_Method_WithParam1_WithProperty2_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(Method_WithParam1_WithProperty2));

        Assert.Equal(3, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[0].MemberName);
        Assert.Equal(nameof(Value1), memberDescriptors[1].MemberName);
        Assert.Equal(nameof(Value2), memberDescriptors[2].MemberName);
    }

    [CommandMethod]
    [CommandMethodProperty(nameof(Value1), nameof(Value2))]
    [CommandMethodStaticProperty(typeof(StaticClass))]
    internal void Method_WithParam1_WithProperty2_WithStaticProperty1(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_Method_WithParam1_WithProperty2_WithStaticProperty1_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(Method_WithParam1_WithProperty2_WithStaticProperty1));

        Assert.Equal(5, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[0].MemberName);
        Assert.Equal(nameof(Value1), memberDescriptors[1].MemberName);
        Assert.Equal(nameof(Value2), memberDescriptors[2].MemberName);
        Assert.Equal(nameof(StaticClass.StaticValue1), memberDescriptors[3].MemberName);
        Assert.Equal(nameof(StaticClass.StaticValue2), memberDescriptors[4].MemberName);
    }

    [CommandMethod]
    [CommandMethodProperty(nameof(Value1), nameof(Value2))]
    [CommandMethodStaticProperty(typeof(StaticClass), nameof(StaticClass.StaticValue1))]
    internal void Method_WithParam1_WithProperty2_WithStaticProperty2(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_Method_WithParam1_WithProperty2_WithStaticProperty2_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(Method_WithParam1_WithProperty2_WithStaticProperty2));

        Assert.Equal(4, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[0].MemberName);
        Assert.Equal(nameof(Value1), memberDescriptors[1].MemberName);
        Assert.Equal(nameof(Value2), memberDescriptors[2].MemberName);
        Assert.Equal(nameof(StaticClass.StaticValue1), memberDescriptors[3].MemberName);
    }

    [CommandMethod]
    internal static void StaticMethod()
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticMethod_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(StaticMethod));

        Assert.Empty(memberDescriptors);
    }

    [CommandMethod]
    internal static void StaticMethod_WithParam1(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticMethod_WithParam1_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(StaticMethod_WithParam1));

        Assert.Equal(1, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[0].MemberName);
    }

    [CommandMethod]
    internal static void StaticMethod_WithParam2(int @int, params int[] args)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticMethod_WithParam2_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(StaticMethod_WithParam2));

        Assert.Equal(2, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[0].MemberName);
        Assert.Equal("args", memberDescriptors[1].MemberName);
    }

    [CommandMethod]
    [CommandMethodProperty(nameof(Value1))]
    internal static void StaticMethod_WithParam1_WithProperty1(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticMethod_WithParam1_WithProperty1_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(StaticMethod_WithParam1_WithProperty1));

        Assert.Equal(2, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[0].MemberName);
        Assert.Equal(nameof(Value1), memberDescriptors[1].MemberName);
    }

    [CommandMethod]
    [CommandMethodProperty(nameof(Value1), nameof(Value2))]
    internal static void StaticMethod_WithParam1_WithProperty2(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticMethod_WithParam1_WithProperty2_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(StaticMethod_WithParam1_WithProperty2));

        Assert.Equal(3, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[0].MemberName);
        Assert.Equal(nameof(Value1), memberDescriptors[1].MemberName);
        Assert.Equal(nameof(Value2), memberDescriptors[2].MemberName);
    }

    [CommandMethod]
    [CommandMethodProperty(nameof(Value1), nameof(Value2))]
    [CommandMethodStaticProperty(typeof(StaticClass))]
    internal static void StaticMethod_WithParam1_WithProperty2_WithStaticProperty1(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticMethod_WithParam1_WithProperty2_WithStaticProperty1_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(StaticMethod_WithParam1_WithProperty2_WithStaticProperty1));

        Assert.Equal(5, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[0].MemberName);
        Assert.Equal(nameof(Value1), memberDescriptors[1].MemberName);
        Assert.Equal(nameof(Value2), memberDescriptors[2].MemberName);
        Assert.Equal(nameof(StaticClass.StaticValue1), memberDescriptors[3].MemberName);
        Assert.Equal(nameof(StaticClass.StaticValue2), memberDescriptors[4].MemberName);
    }

    [CommandMethod]
    [CommandMethodProperty(nameof(Value1), nameof(Value2))]
    [CommandMethodStaticProperty(typeof(StaticClass), nameof(StaticClass.StaticValue1))]
    internal static void StaticMethod_WithParam1_WithProperty2_WithStaticProperty2(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticMethod_WithParam1_WithProperty2_WithStaticProperty2_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(StaticMethod_WithParam1_WithProperty2_WithStaticProperty2));

        Assert.Equal(4, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[0].MemberName);
        Assert.Equal(nameof(Value1), memberDescriptors[1].MemberName);
        Assert.Equal(nameof(Value2), memberDescriptors[2].MemberName);
        Assert.Equal(nameof(StaticClass.StaticValue1), memberDescriptors[3].MemberName);
    }
}
