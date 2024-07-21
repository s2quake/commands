// <copyright file="BasicClass_Browable_False_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands.Tests.CommandDescriptorTests.GetMemberDescriptorsTypeTests;

public sealed class BasicClass_Browable_False_Test
{
    [Browsable(false)]
    private sealed class BasicClass
    {
        [CommandProperty]
        public int Int { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_Test()
    {
        var obj = new BasicClass();
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(obj.GetType());

        Assert.Empty(memberDescriptors);
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClassType_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(BasicClass));

        Assert.Empty(memberDescriptors);
    }
}
