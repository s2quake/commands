// <copyright file="BasicClass_With_StaticClass_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandDescriptorTests.GetMemberDescriptorsTypeTests;

public sealed class BasicClass_With_StaticClass_FailTest
{
    [CommandStaticProperty(typeof(StaticClass_PropertyArray))]
    private sealed class BasicClass_PropertyArray
    {
        [CommandPropertyArray]
        public string[] Arguments { get; set; } = [];
    }

    private static class StaticClass_PropertyArray
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
        var expectedValue = new CommandMemberInfo(typeof(BasicClass_PropertyArray)).ToString();

        Assert.Equal(expectedValue, exception.Source);
    }

    [CommandStaticProperty(typeof(StaticClass_SameProperty))]
    private sealed class BasicClass_SameProperty
    {
        [CommandProperty]
        public int Int { get; set; }
    }

    private static class StaticClass_SameProperty
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
        var expectedValue = new CommandMemberInfo(typeof(BasicClass_SameProperty)).ToString();

        Assert.Equal(expectedValue, exception.Source);
    }

    [CommandStaticProperty(typeof(StaticClass_SameProperty))]
    private sealed class BasicClass_SameMemberName
    {
        [CommandProperty]
        public int Int { get; set; }
    }

    private static class StaticClass_SameMemberName
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
        var expectedValue = new CommandMemberInfo(typeof(BasicClass_SameMemberName)).ToString();

        Assert.Equal(expectedValue, exception.Source);
    }

    [CommandStaticProperty(typeof(StaticClass_SameProperty))]
    private sealed class BasicClass_SameShortName
    {
        [CommandProperty('i')]
        public int Int { get; set; }
    }

    private static class StaticClass_SameShortName
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
        var expectedValue = new CommandMemberInfo(typeof(BasicClass_SameShortName)).ToString();

        Assert.Equal(expectedValue, exception.Source);
    }

    [CommandStaticProperty(typeof(StaticClass_NotFoundProperty), "Data")]
    private sealed class BasicClass_NotFoundProperty
    {
        [CommandProperty]
        public int Int { get; set; }
    }

    private static class StaticClass_NotFoundProperty
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
        var expectedValue = new CommandMemberInfo(typeof(BasicClass_NotFoundProperty)).ToString();

        Assert.Equal(expectedValue, exception.Source);
    }

    [CommandStaticProperty("")]
    private sealed class BasicClass_EmptyStaticClass
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
        var expectedValue = new CommandMemberInfo(typeof(BasicClass_EmptyStaticClass)).ToString();

        Assert.Equal(expectedValue, exception.Source);
    }

    [CommandStaticProperty("abc")]
    private sealed class BasicClass_NotFoundStaticClass
    {
        [CommandProperty]
        public int Int { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_NotFoundStaticClass_FailTest()
    {
        var type = typeof(BasicClass_NotFoundStaticClass);
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(type);
        });
        var expectedValue = new CommandMemberInfo(type).ToString();

        Assert.Equal(expectedValue, exception.Source);
    }
}
