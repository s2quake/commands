// <copyright file="Property_General_WriteOnly_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandMemberDescriptorTests;

public class Property_General_WriteOnly_FailTest
{
    private sealed class InstanceClass
    {
        private int _member1;

        [CommandProperty]
        public int Member1
        {
            set => _member1 = value;
        }
    }

    [Fact]
    public void InstanceClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass)));
    }
}
