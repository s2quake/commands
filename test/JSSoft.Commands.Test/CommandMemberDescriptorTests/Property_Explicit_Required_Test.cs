// <copyright file="Property_Explicit_Required_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Test.CommandMemberDescriptorTests;

public class Property_Explicit_Required_Test
{
    sealed class InstanceClass
    {
        [CommandPropertyExplicitRequired]
        public string Member1 { get; set; } = string.Empty;

        [CommandPropertyExplicitRequired(DefaultValue = "3")]
        public string Member2 { get; set; } = string.Empty;

        [CommandPropertyExplicitRequired("member")]
        public string Member3 { get; set; } = string.Empty;

        [CommandPropertyExplicitRequired("member4", 'a')]
        public string Member4 { get; set; } = string.Empty;

        [CommandPropertyExplicitRequired('b')]
        public string Member5 { get; set; } = string.Empty;

        [CommandPropertyExplicitRequired('c', useName: true)]
        public string Member6 { get; set; } = string.Empty;
    }

    [Fact]
    public void Base_Member1_Test()
    {
        var memberDescriptor = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass))[nameof(InstanceClass.Member1)];
        Assert.IsType<CommandPropertyDescriptor>(memberDescriptor);
        Assert.Equal("member1", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("--member1", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal(DBNull.Value, memberDescriptor.DefaultValue);
        Assert.True(memberDescriptor.IsRequired);
        Assert.True(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass.Member1), memberDescriptor.MemberName);
        Assert.Equal(CommandType.ExplicitRequired, memberDescriptor.CommandType);
    }

    [Fact]
    public void Base_Member2_Test()
    {
        var memberDescriptor = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass))[nameof(InstanceClass.Member2)];
        Assert.IsType<CommandPropertyDescriptor>(memberDescriptor);
        Assert.Equal("member2", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("--member2", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal("3", memberDescriptor.DefaultValue);
        Assert.True(memberDescriptor.IsRequired);
        Assert.True(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass.Member2), memberDescriptor.MemberName);
        Assert.Equal(CommandType.ExplicitRequired, memberDescriptor.CommandType);
    }

    [Fact]
    public void Base_Member3_Test()
    {
        var memberDescriptor = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass))[nameof(InstanceClass.Member3)];
        Assert.IsType<CommandPropertyDescriptor>(memberDescriptor);
        Assert.Equal("member", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("--member", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal(DBNull.Value, memberDescriptor.DefaultValue);
        Assert.True(memberDescriptor.IsRequired);
        Assert.True(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass.Member3), memberDescriptor.MemberName);
        Assert.Equal(CommandType.ExplicitRequired, memberDescriptor.CommandType);
    }

    [Fact]
    public void Base_Member4_Test()
    {
        var memberDescriptor = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass))[nameof(InstanceClass.Member4)];
        Assert.IsType<CommandPropertyDescriptor>(memberDescriptor);
        Assert.Equal("member4", memberDescriptor.Name);
        Assert.Equal('a', memberDescriptor.ShortName);
        Assert.Equal("--member4 | -a", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal(DBNull.Value, memberDescriptor.DefaultValue);
        Assert.True(memberDescriptor.IsRequired);
        Assert.True(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass.Member4), memberDescriptor.MemberName);
        Assert.Equal(CommandType.ExplicitRequired, memberDescriptor.CommandType);
    }

    [Fact]
    public void Base_Member5_Test()
    {
        var memberDescriptor = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass))[nameof(InstanceClass.Member5)];
        Assert.IsType<CommandPropertyDescriptor>(memberDescriptor);
        Assert.Equal(string.Empty, memberDescriptor.Name);
        Assert.Equal('b', memberDescriptor.ShortName);
        Assert.Equal("-b", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal(DBNull.Value, memberDescriptor.DefaultValue);
        Assert.True(memberDescriptor.IsRequired);
        Assert.True(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass.Member5), memberDescriptor.MemberName);
        Assert.Equal(CommandType.ExplicitRequired, memberDescriptor.CommandType);
    }

    [Fact]
    public void Base_Member6_Test()
    {
        var memberDescriptor = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass))[nameof(InstanceClass.Member6)];
        Assert.IsType<CommandPropertyDescriptor>(memberDescriptor);
        Assert.Equal("member6", memberDescriptor.Name);
        Assert.Equal('c', memberDescriptor.ShortName);
        Assert.Equal("-c | --member6", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal(DBNull.Value, memberDescriptor.DefaultValue);
        Assert.True(memberDescriptor.IsRequired);
        Assert.True(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass.Member6), memberDescriptor.MemberName);
        Assert.Equal(CommandType.ExplicitRequired, memberDescriptor.CommandType);
    }
}
