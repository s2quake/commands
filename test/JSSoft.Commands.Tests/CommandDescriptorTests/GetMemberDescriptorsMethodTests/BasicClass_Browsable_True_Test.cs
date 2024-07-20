// <copyright file="BasicClass_Browsable_True_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable CA1822

using System.ComponentModel;
using static JSSoft.Commands.CommandDescriptor;

namespace JSSoft.Commands.Tests.CommandDescriptorTests.GetMemberDescriptorsMethodTests;

[Browsable(true)]
public sealed class BasicClass_Browsable_True_Test
{
    private static class BrowsableTrue_StaticClass
    {
        [CommandProperty]
        public static int StaticValue1 { get; set; }

        [CommandProperty]
        public static int StaticValue2 { get; set; }
    }

    private static class BrowsableFalse_StaticClass
    {
        [CommandProperty]
        public static int StaticValue1 { get; set; }

        [CommandProperty]
        public static int StaticValue2 { get; set; }
    }

    [CommandMethod]
    internal void Method(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_Method_Test()
    {
        var memberDescriptors = GetMemberDescriptors(this, nameof(Method));

        Assert.Equal(1, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[0].MemberName);
    }

    [CommandMethod]
    [Browsable(true)]
    [CommandMethodStaticProperty(typeof(BrowsableFalse_StaticClass))]
    internal void BrowsableTrue_Method(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BrowsableTrue_Method_Test()
    {
        var memberDescriptors = GetMemberDescriptors(this, nameof(BrowsableTrue_Method));
        var index = 0;

        Assert.Equal(3, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[index].MemberName);
        index++;
        Assert.Equal(
            expected: nameof(BrowsableTrue_StaticClass.StaticValue1),
            actual: memberDescriptors[index].MemberName);
        index++;
        Assert.Equal(
            expected: nameof(BrowsableTrue_StaticClass.StaticValue2),
            actual: memberDescriptors[index].MemberName);
    }

    [CommandMethod]
    [Browsable(false)]
    [CommandMethodStaticProperty(typeof(BrowsableTrue_StaticClass))]
    internal void BrowsableFalse_Method(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BrowsableFalse_Method_Test()
    {
        var memberDescriptors = GetMemberDescriptors(this, nameof(BrowsableFalse_Method));

        Assert.Equal(0, memberDescriptors.Count);
    }

    [CommandMethod]
    internal static void StaticMethod(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticMethod_Test()
    {
        var memberDescriptors = GetMemberDescriptors(this, nameof(StaticMethod));

        Assert.Equal(1, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[0].MemberName);
    }

    [CommandMethod]
    [Browsable(true)]
    [CommandMethodStaticProperty(typeof(BrowsableFalse_StaticClass))]
    internal static void BrowsableTrue_StaticMethod(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BrowsableTrue_StaticMethod_Test()
    {
        var memberDescriptors = GetMemberDescriptors(this, nameof(BrowsableTrue_StaticMethod));
        var index = 0;

        Assert.Equal(3, memberDescriptors.Count);
        Assert.Equal("int", memberDescriptors[index].MemberName);
        index++;
        Assert.Equal(
            expected: nameof(BrowsableTrue_StaticClass.StaticValue1),
            actual: memberDescriptors[index].MemberName);
        index++;
        Assert.Equal(
            expected: nameof(BrowsableTrue_StaticClass.StaticValue2),
            actual: memberDescriptors[index].MemberName);
    }

    [CommandMethod]
    [Browsable(false)]
    [CommandMethodStaticProperty(typeof(BrowsableTrue_StaticClass))]
    internal static void BrowsableFalse_StaticMethod(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BrowsableFalse_StaticMethod_Test()
    {
        var memberDescriptors = GetMemberDescriptors(this, nameof(BrowsableFalse_StaticMethod));

        Assert.Equal(0, memberDescriptors.Count);
    }
}
