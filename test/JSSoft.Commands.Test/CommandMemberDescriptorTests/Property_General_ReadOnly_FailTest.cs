// <copyright file="Property_General_ReadOnly_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Test.CommandMemberDescriptorTests;

public class Property_General_ReadOnly_FailTest
{
    sealed class InstanceClass
    {
        [CommandProperty]
        public int Member1 { get; }
    }

    [Fact]
    public void InstanceClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass)));
    }
}
