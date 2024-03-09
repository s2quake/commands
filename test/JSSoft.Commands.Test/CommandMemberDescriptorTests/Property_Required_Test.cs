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

public class Property_Required_Test
{
    sealed class InstanceClass
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
        var memberDescriptor = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass))[nameof(InstanceClass.Required1)];
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
        var memberDescriptor = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass))[nameof(InstanceClass.Required2)];
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
        var memberDescriptor = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass))[nameof(InstanceClass.Required3)];
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
