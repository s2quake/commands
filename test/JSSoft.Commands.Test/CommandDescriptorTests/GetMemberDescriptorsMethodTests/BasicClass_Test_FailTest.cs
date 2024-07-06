// <copyright file="BasicClass_Test_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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
