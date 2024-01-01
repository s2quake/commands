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

namespace JSSoft.Commands.Test.CommandDescriptorTests.GetMemberDescriptorsMethodTests;

public sealed class BasicClass_Test_FailTest
{
    static class StaticClass
    {
        [CommandProperty]
        public static int StaticValue { get; set; }
    }

    static class StaticClassPropertyArray
    {
        [CommandPropertyArray]
        public static string[] Arguments { get; set; } = [];
    }

    [CommandMethod]
    internal int Method_WithReturnType()
    {
        return 0;
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_Method_WithReturnType_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(this, nameof(Method_WithReturnType));
        });

        Assert.Equal(GetType().AssemblyQualifiedName!, exception.Source);
    }

    [CommandMethod]
    [CommandMethodProperty("abc")]
    internal void Method_WithNotFoundProperty()
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_Method_WithNotFoundProperty_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(this, nameof(Method_WithNotFoundProperty));
        });

        Assert.Equal(GetType().AssemblyQualifiedName!, exception.Source);
    }

    [CommandMethod]
    [CommandMethodStaticProperty("abc")]
    internal void Method_WithNotFoundStaticClass()
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_Method_WithNotFoundStaticClass_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(this, nameof(Method_WithNotFoundStaticClass));
        });

        Assert.Equal(GetType().AssemblyQualifiedName!, exception.Source);
    }

    [CommandMethod]
    [CommandMethodStaticProperty(typeof(StaticClass), "abc")]
    internal void Method_WithStaticClass_WithNotFoundProperty()
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_Method_WithStaticClass_WithNotFoundProperty_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(this, nameof(Method_WithStaticClass_WithNotFoundProperty));
        });

        Assert.Equal(GetType().AssemblyQualifiedName!, exception.Source);
    }

    [CommandMethod]
    [CommandMethodStaticProperty(typeof(StaticClassPropertyArray))]
    internal void Method_WithStaticClass_WithStaticPropertyArray(params string[] args)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_Method_WithStaticClass_WithStaticPropertyArray_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(this, nameof(Method_WithStaticClass_WithStaticPropertyArray));
        });

        Assert.Equal(GetType().AssemblyQualifiedName!, exception.Source);
    }

    [CommandMethod]
    internal static string StaticMethod_WithReturnType()
    {
        return string.Empty;
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticMethod_WithReturnType_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(this, nameof(StaticMethod_WithReturnType));
        });

        Assert.Equal(GetType().AssemblyQualifiedName!, exception.Source);
    }

    [CommandMethod]
    [CommandMethodProperty("abc")]
    internal static void StaticMethod_WithNotFoundProperty()
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticMethod_WithNotFoundProperty_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(this, nameof(StaticMethod_WithNotFoundProperty));
        });

        Assert.Equal(GetType().AssemblyQualifiedName!, exception.Source);
    }

    [CommandMethod]
    [CommandMethodStaticProperty("abc")]
    internal static void StaticMethod_WithNotFoundStaticClass()
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticMethod_WithNotFoundStaticClass_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(this, nameof(StaticMethod_WithNotFoundStaticClass));
        });

        Assert.Equal(GetType().AssemblyQualifiedName!, exception.Source);
    }

    [CommandMethod]
    [CommandMethodStaticProperty(typeof(StaticClass), "abc")]
    internal static void StaticMethod_WithStaticClass_WithNotFoundProperty()
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticMethod_WithStaticClass_WithNotFoundProperty_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(this, nameof(StaticMethod_WithStaticClass_WithNotFoundProperty));
        });

        Assert.Equal(GetType().AssemblyQualifiedName!, exception.Source);
    }

    [CommandMethod]
    [CommandMethodStaticProperty(typeof(StaticClassPropertyArray))]
    internal static void StaticMethod_WithStaticClass_WithStaticPropertyArray(params string[] args)
    {
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticMethod_WithStaticClass_WithStaticPropertyArray_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(this, nameof(StaticMethod_WithStaticClass_WithStaticPropertyArray));
        });

        Assert.Equal(GetType().AssemblyQualifiedName!, exception.Source);
    }
}
