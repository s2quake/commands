// <copyright file="BasicClass_Browable_True_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands.Test.CommandDescriptorTests.GetMemberDescriptorsTypeTests;

public sealed class BasicClass_Browable_True_Test
{
    [Browsable(true)]
    sealed class BasicClass
    {
        [CommandProperty]
        public int Int { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClass_Test()
    {
        var obj = new BasicClass();
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(obj.GetType());

        Assert.Equal(1, memberDescriptors.Count);
        Assert.Equal(nameof(BasicClass.Int), memberDescriptors[0].MemberName);
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BasicClassType_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(BasicClass));

        
        Assert.Equal(1, memberDescriptors.Count);
        Assert.Equal(nameof(BasicClass.Int), memberDescriptors[0].MemberName);
    }
}
