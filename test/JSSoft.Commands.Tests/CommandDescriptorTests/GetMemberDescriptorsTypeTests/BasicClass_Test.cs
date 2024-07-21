// <copyright file="BasicClass_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands.Tests.CommandDescriptorTests.GetMemberDescriptorsTypeTests;

public sealed class BasicClass_Test
{
    private sealed class BasicClass
    {
        [CommandPropertyRequired]
        public int Int { get; set; }

        [CommandPropertySwitch]
        public bool Bool { get; set; }

        [CommandProperty]
        [Browsable(true)]
        public string String { get; set; } = string.Empty;

        [CommandProperty]
        [Browsable(false)]
        public float Float { get; set; }

        public double Double { get; set; }

        [CommandPropertyArray]
        public string[] Arguments { get; set; } = [];
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_Test()
    {
        var obj = new BasicClass();
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(obj.GetType());
        var index = 0;

        Assert.Equal(4, memberDescriptors.Count);
        Assert.Equal(nameof(BasicClass.Int), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.Arguments), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.String), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.Bool), memberDescriptors[index++].MemberName);

        Assert.Equal(4, index);
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClassType_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(BasicClass));
        var index = 0;

        Assert.Equal(4, memberDescriptors.Count);
        Assert.Equal(nameof(BasicClass.Int), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.Arguments), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.String), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(BasicClass.Bool), memberDescriptors[index++].MemberName);

        Assert.Equal(4, index);
    }

    private sealed class MultiplePropertyArrayInvalidClass
    {
        [CommandPropertyArray]
        public string[] Arguments1 { get; set; } = [];

        [CommandPropertyArray]
        public string[] Arguments2 { get; set; } = [];
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_MultiplePropertyArrayInvalidClass_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(typeof(MultiplePropertyArrayInvalidClass));
        });
        Assert.Equal(
            typeof(MultiplePropertyArrayInvalidClass).AssemblyQualifiedName, exception.Source);
    }
}
