// <copyright file="StaticClass_MultiplePropertyArray_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandDescriptorTests.GetMemberDescriptorsTypeTests;

public sealed class StaticClass_MultiplePropertyArray_FailTest
{
    private static class StaticClass
    {
        [CommandPropertyArray]
        public static string[] Arguments1 { get; set; } = [];

        [CommandPropertyArray]
        public static string[] Arguments2 { get; set; } = [];
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticClassType_FailTest()
    {
        var exception = Assert.Throws<CommandDefinitionException>(() =>
        {
            CommandDescriptor.GetMemberDescriptors(typeof(StaticClass));
        });
        Assert.Equal(typeof(StaticClass).AssemblyQualifiedName, exception.Source);
    }
}
