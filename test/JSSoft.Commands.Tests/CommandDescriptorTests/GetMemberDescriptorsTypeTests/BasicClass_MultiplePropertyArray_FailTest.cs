// <copyright file="BasicClass_MultiplePropertyArray_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandDescriptorTests.GetMemberDescriptorsTypeTests;

public sealed class BasicClass_MultiplePropertyArray_FailTest
{
    private sealed class BasicClass
    {
        [CommandPropertyArray]
        public string[] Arguments1 { get; set; } = [];

        [CommandPropertyArray]
        public string[] Arguments2 { get; set; } = [];
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_FailTest()
    {
        var obj = new BasicClass();
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(obj.GetType());
        });
        Assert.Equal(typeof(BasicClass).AssemblyQualifiedName, exception.Source);
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClassType_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(typeof(BasicClass));
        });
        Assert.Equal(typeof(BasicClass).AssemblyQualifiedName, exception.Source);
    }
}
