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

public class Parameter_Variables_Test
{
    sealed class InstanceClass
    {
        [CommandMethod]
        public void Test1(params string[] args)
        {
        }

        [CommandMethod]
        public void Test2(params string[] Args)
        {
        }
    }

    [Fact]
    public void Base_Variables1_Test()
    {
        var memberDescriptor = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass), nameof(InstanceClass.Test1))["args"];
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
        Assert.Equal(CommandType.Variables, memberDescriptor.CommandType);
    }

    [Fact]
    public void Base_Variables2_Test()
    {
        var memberDescriptor = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass), nameof(InstanceClass.Test2))["Args"];
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
        Assert.Equal(CommandType.Variables, memberDescriptor.CommandType);
    }
}
