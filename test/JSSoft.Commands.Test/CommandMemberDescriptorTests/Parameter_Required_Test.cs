// <copyright file="Parameter_Required_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Test.CommandMemberDescriptorTests;

public class Parameter_Required_Test
{
    sealed class InstanceClass
    {
        [CommandMethod]
        public void Test1(string arg0 = "")
        {
        }

        [CommandMethod]
        public void Test2(string Arg0 = "")
        {
        }

        [CommandMethod]
        public void Test3(string arg0)
        {
        }
    }

    [Fact]
    public void Base_Required1_Test()
    {
        var memberDescriptor = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass), nameof(InstanceClass.Test1))["arg0"];
        Assert.IsType<CommandParameterDescriptor>(memberDescriptor);
        Assert.Equal("arg0", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("arg0", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal("", memberDescriptor.DefaultValue);
        Assert.True(memberDescriptor.IsRequired);
        Assert.False(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal("arg0", memberDescriptor.MemberName);
        Assert.Equal(CommandType.Required, memberDescriptor.CommandType);
    }

    [Fact]
    public void Base_Required2_Test()
    {
        var memberDescriptor = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass), nameof(InstanceClass.Test2))["Arg0"];
        Assert.IsType<CommandParameterDescriptor>(memberDescriptor);
        Assert.Equal("arg0", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("arg0", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal("", memberDescriptor.DefaultValue);
        Assert.True(memberDescriptor.IsRequired);
        Assert.False(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal("Arg0", memberDescriptor.MemberName);
        Assert.Equal(CommandType.Required, memberDescriptor.CommandType);
    }

    [Fact]
    public void Base_Required3_Test()
    {
        var memberDescriptor = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass), nameof(InstanceClass.Test3))["arg0"];
        Assert.IsType<CommandParameterDescriptor>(memberDescriptor);
        Assert.Equal("arg0", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("arg0", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal(DBNull.Value, memberDescriptor.DefaultValue);
        Assert.True(memberDescriptor.IsRequired);
        Assert.False(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal("arg0", memberDescriptor.MemberName);
        Assert.Equal(CommandType.Required, memberDescriptor.CommandType);
    }
}
