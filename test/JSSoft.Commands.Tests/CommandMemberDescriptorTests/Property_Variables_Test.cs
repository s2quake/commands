// <copyright file="Property_Variables_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandMemberDescriptorTests;

public class Property_Variables_Test
{
    private sealed class InstanceClass
    {
        [CommandPropertyArray]
        public string[] Member1 { get; set; } = [];
    }

    [Fact]
    public void Base_Member1_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass));
        var memberDescriptor = memberDescriptors[nameof(InstanceClass.Member1)];
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
        Assert.Equal(nameof(InstanceClass.Member1), memberDescriptor.MemberName);
        Assert.Equal(CommandType.Variables, memberDescriptor.CommandType);
    }
}
