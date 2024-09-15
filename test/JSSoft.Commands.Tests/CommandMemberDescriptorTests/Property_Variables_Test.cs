// <copyright file="Property_Variables_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands.Tests.CommandMemberDescriptorTests;

public class Property_Variables_Test
{
    private sealed class InstanceClass1
    {
        [CommandPropertyArray]
        [Category("")]
        public string[] Member1 { get; set; } = [];
    }

    [Fact]
    public void Base_InstanceClass1_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass1));
        var memberDescriptor = memberDescriptors[nameof(InstanceClass1.Member1)];
        Assert.IsType<CommandPropertyDescriptor>(memberDescriptor);
        Assert.Equal("member1", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("member1...", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal(DBNull.Value, memberDescriptor.DefaultValue);
        Assert.False(memberDescriptor.IsRequired);
        Assert.False(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.True(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string[]), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass1.Member1), memberDescriptor.MemberName);
        Assert.Empty(memberDescriptor.Category);
    }

    private sealed class InstanceClass2
    {
        [CommandPropertyArray]
        public string[] Member2 { get; set; } = [];
    }

    [Fact]
    public void Base_InstanceClass2_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass2));
        var memberDescriptor = memberDescriptors[nameof(InstanceClass2.Member2)];
        Assert.IsType<CommandPropertyDescriptor>(memberDescriptor);
        Assert.Equal("member2", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("member2...", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal(DBNull.Value, memberDescriptor.DefaultValue);
        Assert.False(memberDescriptor.IsRequired);
        Assert.False(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.True(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string[]), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass2.Member2), memberDescriptor.MemberName);
        Assert.Empty(memberDescriptor.Category);
    }

    private sealed class InstanceClass3
    {
        [CommandPropertyArray]
        [Category("Test")]
        public string[] Member3 { get; set; } = [];
    }

    [Fact]
    public void Base_InstanceClass3_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass3));
        var memberDescriptor = memberDescriptors[nameof(InstanceClass3.Member3)];
        Assert.IsType<CommandPropertyDescriptor>(memberDescriptor);
        Assert.Equal("member3", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("member3...", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal(DBNull.Value, memberDescriptor.DefaultValue);
        Assert.False(memberDescriptor.IsRequired);
        Assert.False(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.True(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string[]), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass3.Member3), memberDescriptor.MemberName);
        Assert.Equal("Test", memberDescriptor.Category);
    }
}
