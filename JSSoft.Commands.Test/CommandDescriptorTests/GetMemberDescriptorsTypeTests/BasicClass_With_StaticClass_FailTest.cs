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

using System.ComponentModel;

namespace JSSoft.Commands.Test.CommandDescriptorTests.GetMemberDescriptorsTypeTests;

public sealed class BasicClass_With_StaticClass_FailTest
{
    [CommandStaticProperty(typeof(StaticClass_PropertyArray))]
    sealed class BasicClass_PropertyArray
    {
        [CommandPropertyArray]
        public string[] Arguments { get; set; } = [];
    }

    static class StaticClass_PropertyArray
    {
        [CommandPropertyArray]
        public static string[] StaticArguments { get; set; } = [];
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_PropertyArray_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(typeof(BasicClass_PropertyArray));
        });

        Assert.Equal(typeof(BasicClass_PropertyArray).AssemblyQualifiedName!, exception.Source);
    }

    [CommandStaticProperty(typeof(StaticClass_SameProperty))]
    sealed class BasicClass_SameProperty
    {
        [CommandProperty]
        public int Int { get; set; }
    }

    static class StaticClass_SameProperty
    {
        [CommandProperty]
        public static int Int { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_SameProperty_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(typeof(BasicClass_SameProperty));
        });

        Assert.Equal(typeof(BasicClass_SameProperty).AssemblyQualifiedName!, exception.Source);
    }

    [CommandStaticProperty(typeof(StaticClass_SameProperty))]
    sealed class BasicClass_SameMemberName
    {
        [CommandProperty]
        public int Int { get; set; }
    }

    static class StaticClass_SameMemberName
    {
        [CommandProperty("int")]
        public static int StaticInt { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_SameMemberName_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(typeof(BasicClass_SameMemberName));
        });

        Assert.Equal(typeof(BasicClass_SameMemberName).AssemblyQualifiedName!, exception.Source);
    }

    [CommandStaticProperty(typeof(StaticClass_SameProperty))]
    sealed class BasicClass_SameShortName
    {
        [CommandProperty('i')]
        public int Int { get; set; }
    }

    static class StaticClass_SameShortName
    {
        [CommandProperty('i')]
        public static int StaticInt { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_SameShortName_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(typeof(BasicClass_SameShortName));
        });

        Assert.Equal(typeof(BasicClass_SameShortName).AssemblyQualifiedName!, exception.Source);
    }

    [CommandStaticProperty(typeof(StaticClass_NotFoundProperty),
        "Data"
    )]
    sealed class BasicClass_NotFoundProperty
    {
        [CommandProperty]
        public int Int { get; set; }
    }

    static class StaticClass_NotFoundProperty
    {
        [CommandProperty]
        public static int Int { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_NotFoundProperty_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(typeof(BasicClass_NotFoundProperty));
        });

        Assert.Equal(typeof(BasicClass_NotFoundProperty).AssemblyQualifiedName!, exception.Source);
    }

    [CommandStaticProperty("")]
    sealed class BasicClass_EmptyStaticClass
    {
        [CommandProperty]
        public int Int { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_EmptyStaticClass_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(typeof(BasicClass_EmptyStaticClass));
        });

        Assert.Equal(typeof(BasicClass_EmptyStaticClass).AssemblyQualifiedName!, exception.Source);
    }

    [CommandStaticProperty("abc")]
    sealed class BasicClass_NotFoundStaticClass
    {
        [CommandProperty]
        public int Int { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_NotFoundStaticClass_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(typeof(BasicClass_NotFoundStaticClass));
        });

        Assert.Equal(typeof(BasicClass_NotFoundStaticClass).AssemblyQualifiedName!, exception.Source);
    }
}
