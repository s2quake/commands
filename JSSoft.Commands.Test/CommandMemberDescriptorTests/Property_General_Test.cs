// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

namespace JSSoft.Commands.Test.CommandMemberDescriptorTests;

public class Property_General_Test
{
    sealed class InstanceClass
    {
        [CommandProperty]
        public string Member1 { get; set; } = string.Empty;

        [CommandProperty(InitValue = "1", DefaultValue = "3")]
        public string Member2 { get; set; } = string.Empty;

        [CommandProperty("member")]
        public string Member3 { get; set; } = string.Empty;

        [CommandProperty("member4", 'a')]
        public string Member4 { get; set; } = string.Empty;

        [CommandProperty('b')]
        public string Member5 { get; set; } = string.Empty;

        [CommandProperty('c', useName: true)]
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
        Assert.False(memberDescriptor.IsRequired);
        Assert.True(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass.Member1), memberDescriptor.MemberName);
        Assert.Equal(CommandType.General, memberDescriptor.CommandType);
    }

    [Fact]
    public void Base_Member2_Test()
    {
        var memberDescriptor = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass))[nameof(InstanceClass.Member2)];
        Assert.IsType<CommandPropertyDescriptor>(memberDescriptor);
        Assert.Equal("member2", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("--member2", memberDescriptor.DisplayName);
        Assert.Equal("1", memberDescriptor.InitValue);
        Assert.Equal("3", memberDescriptor.DefaultValue);
        Assert.False(memberDescriptor.IsRequired);
        Assert.True(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass.Member2), memberDescriptor.MemberName);
        Assert.Equal(CommandType.General, memberDescriptor.CommandType);
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
        Assert.False(memberDescriptor.IsRequired);
        Assert.True(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass.Member3), memberDescriptor.MemberName);
        Assert.Equal(CommandType.General, memberDescriptor.CommandType);
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
        Assert.False(memberDescriptor.IsRequired);
        Assert.True(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass.Member4), memberDescriptor.MemberName);
        Assert.Equal(CommandType.General, memberDescriptor.CommandType);
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
        Assert.False(memberDescriptor.IsRequired);
        Assert.True(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass.Member5), memberDescriptor.MemberName);
        Assert.Equal(CommandType.General, memberDescriptor.CommandType);
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
        Assert.False(memberDescriptor.IsRequired);
        Assert.True(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(string), memberDescriptor.MemberType);
        Assert.Equal(nameof(InstanceClass.Member6), memberDescriptor.MemberName);
        Assert.Equal(CommandType.General, memberDescriptor.CommandType);
    }
}
