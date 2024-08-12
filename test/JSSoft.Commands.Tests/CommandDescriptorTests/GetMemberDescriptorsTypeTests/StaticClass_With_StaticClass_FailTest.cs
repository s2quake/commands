// <copyright file="StaticClass_With_StaticClass_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandDescriptorTests.GetMemberDescriptorsTypeTests;

public sealed class StaticClass_With_StaticClass_FailTest
{
    [CommandStaticProperty(typeof(StaticClass2))]
    private static class StaticClass1
    {
        [CommandProperty]
        public static int Int1 { get; set; }
    }

    private static class StaticClass2
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
        var expectedValue = new CommandMemberInfo(typeof(StaticClass1)).ToString();
        Assert.Equal(expectedValue, exception.Source);
    }
}
