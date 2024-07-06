// <copyright file="StaticClass_With_StaticClass_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands.Test.CommandDescriptorTests.GetMemberDescriptorsTypeTests;

public sealed class StaticClass_With_StaticClass_FailTest
{
    [CommandStaticProperty(typeof(StaticClass2))]
    static class StaticClass1
    {
        [CommandProperty]
        public static int Int1 { get; set; }
    }

    static class StaticClass2
    {
        [CommandProperty]
        public static int Int2 { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticClass1_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() => 
        {
            CommandDescriptor.GetMemberDescriptors(typeof(StaticClass1));
        });
        Assert.Equal(typeof(StaticClass1).AssemblyQualifiedName!, exception.Source);
    }
}
