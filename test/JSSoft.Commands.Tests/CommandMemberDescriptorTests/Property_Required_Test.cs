// <copyright file="Property_Required_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandMemberDescriptorTests;

public class Property_Required_Test
{
    private sealed class InstanceClass
    {
        [CommandPropertyRequired]
        public string Required1 { get; set; } = string.Empty;

        [CommandPropertyRequired(DefaultValue = "3")]
        public string Required2 { get; set; } = string.Empty;

        [CommandPropertyRequired("required")]
        public string Required3 { get; set; } = string.Empty;
    }

    [Fact]
    public void Base_Required1_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass));
        var memberDescriptor = memberDescriptors[nameof(InstanceClass.Required1)];
        Assert.IsType<CommandPropertyDescriptor>(memberDescriptor);
        Assert.Equal("required1", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("required1", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal(DBNull.Value, memberDescriptor.DefaultValue);
        Assert.True(memberDescriptor.IsRequired);
        Assert.False(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass.Required1), memberDescriptor.MemberName);
        Assert.Equal(CommandType.Required, memberDescriptor.CommandType);
    }

    [Fact]
    public void Base_Required2_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass));
        var memberDescriptor = memberDescriptors[nameof(InstanceClass.Required2)];
        Assert.IsType<CommandPropertyDescriptor>(memberDescriptor);
        Assert.Equal("required2", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("required2", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal("3", memberDescriptor.DefaultValue);
        Assert.True(memberDescriptor.IsRequired);
        Assert.False(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass.Required2), memberDescriptor.MemberName);
        Assert.Equal(CommandType.Required, memberDescriptor.CommandType);
    }

    [Fact]
    public void Base_Required3_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass));
        var memberDescriptor = memberDescriptors[nameof(InstanceClass.Required3)];
        Assert.IsType<CommandPropertyDescriptor>(memberDescriptor);
        Assert.Equal("required", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("required", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal(DBNull.Value, memberDescriptor.DefaultValue);
        Assert.True(memberDescriptor.IsRequired);
        Assert.False(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass.Required3), memberDescriptor.MemberName);
        Assert.Equal(CommandType.Required, memberDescriptor.CommandType);
    }
}
