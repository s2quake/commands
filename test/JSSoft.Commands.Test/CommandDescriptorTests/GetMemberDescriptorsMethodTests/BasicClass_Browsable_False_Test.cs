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

#pragma warning disable CA1822

using System.ComponentModel;

namespace JSSoft.Commands.Test.CommandDescriptorTests.GetMemberDescriptorsMethodTests;

[Browsable(false)]
public sealed class BasicClass_Browsable_False_Test
{
    static class StaticClass
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
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(Method));

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
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(BrowsableTrue_Method));

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
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(BrowsableFalse_Method));

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
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(StaticMethod));

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
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(BrowsableTrue_StaticMethod));

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
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(this, nameof(BrowsableFalse_StaticMethod));

        Assert.Equal(0, memberDescriptors.Count);
    }
}
