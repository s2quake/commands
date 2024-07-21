// <copyright file="BasicClass_Browsable_False_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable CA1822

using System.ComponentModel;
using static JSSoft.Commands.CommandDescriptor;

namespace JSSoft.Commands.Tests.CommandDescriptorTests.GetMemberDescriptorsMethodTests;

[Browsable(false)]
public sealed class BasicClass_Browsable_False_Test
{
    private static class StaticClass
    {
        [CommandProperty]
        public static int StaticValue1 { get; set; }

        [CommandProperty]
        public static int StaticValue2 { get; set; }
    }

    [CommandMethod]
    [CommandMethodStaticProperty(typeof(StaticClass))]
    internal void Method(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_Method_Test()
    {
        var memberDescriptors = GetMemberDescriptors(this, nameof(Method));

        Assert.Equal(0, memberDescriptors.Count);
    }

    [CommandMethod]
    [Browsable(true)]
    [CommandMethodStaticProperty(typeof(StaticClass))]
    internal void BrowsableTrue_Method(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_BrowsableTrue_Method_Test()
    {
        var memberDescriptors = GetMemberDescriptors(this, nameof(BrowsableTrue_Method));

        Assert.Equal(0, memberDescriptors.Count);
    }

    [CommandMethod]
    [Browsable(false)]
    [CommandMethodStaticProperty(typeof(StaticClass))]
    internal void BrowsableFalse_Method(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_BrowsableFalse_Method_Test()
    {
        var memberDescriptors = GetMemberDescriptors(this, nameof(BrowsableFalse_Method));

        Assert.Equal(0, memberDescriptors.Count);
    }

    [CommandMethod]
    [CommandMethodStaticProperty(typeof(StaticClass))]
    internal static void StaticMethod(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_StaticMethod_Test()
    {
        var memberDescriptors = GetMemberDescriptors(this, nameof(StaticMethod));

        Assert.Equal(0, memberDescriptors.Count);
    }

    [CommandMethod]
    [Browsable(true)]
    [CommandMethodStaticProperty(typeof(StaticClass))]
    internal static void BrowsableTrue_StaticMethod(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_BrowsableTrue_StaticMethod_Test()
    {
        var memberDescriptors = GetMemberDescriptors(this, nameof(BrowsableTrue_StaticMethod));

        Assert.Equal(0, memberDescriptors.Count);
    }

    [CommandMethod]
    [Browsable(false)]
    [CommandMethodStaticProperty(typeof(StaticClass))]
    internal static void BrowsableFalse_StaticMethod(int @int)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_BrowsableFalse_StaticMethod_Test()
    {
        var memberDescriptors = GetMemberDescriptors(this, nameof(BrowsableFalse_StaticMethod));

        Assert.Equal(0, memberDescriptors.Count);
    }
}
