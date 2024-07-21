// <copyright file="Parameter_Variables_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandMemberDescriptorTests;

public class Parameter_Variables_Test
{
    private sealed class InstanceClass
    {
        [CommandMethod]
        public void Test1(params string[] args)
        {
        }

#pragma warning disable SA1313
        /// <summary>
        /// Test for <see cref="Base_Required2_Test"/>.
        /// <!---->
        [CommandMethod]
        public void Test2(params string[] Args)
        {
        }
    }
#pragma warning restore SA1313

    [Fact]
    public void Base_Variables1_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(
            type: typeof(InstanceClass),
            methodName: nameof(InstanceClass.Test1));
        var memberDescriptor = memberDescriptors["args"];
        Assert.IsType<CommandParameterArrayDescriptor>(memberDescriptor);
        Assert.Equal("args", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("args", memberDescriptor.DisplayName);
        Assert.Equivalent(Array.Empty<string>(), memberDescriptor.InitValue);
        Assert.Equal(DBNull.Value, memberDescriptor.DefaultValue);
        Assert.False(memberDescriptor.IsRequired);
        Assert.False(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.True(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string[]), memberDescriptor.MemberType);
        Assert.Equal("args", memberDescriptor.MemberName);
        Assert.True(memberDescriptor.IsVariables);
    }

    [Fact]
    public void Base_Variables2_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(
            type: typeof(InstanceClass),
            methodName: nameof(InstanceClass.Test2));
        var memberDescriptor = memberDescriptors["Args"];
        Assert.IsType<CommandParameterArrayDescriptor>(memberDescriptor);
        Assert.Equal("args", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("args", memberDescriptor.DisplayName);
        Assert.Equivalent(Array.Empty<string>(), memberDescriptor.InitValue);
        Assert.Equal(DBNull.Value, memberDescriptor.DefaultValue);
        Assert.False(memberDescriptor.IsRequired);
        Assert.False(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.True(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string[]), memberDescriptor.MemberType);
        Assert.Equal("Args", memberDescriptor.MemberName);
        Assert.True(memberDescriptor.IsVariables);
    }
}
