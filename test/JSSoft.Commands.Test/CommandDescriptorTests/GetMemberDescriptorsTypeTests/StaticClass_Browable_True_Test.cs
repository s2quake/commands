// <copyright file="StaticClass_Browable_True_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands.Test.CommandDescriptorTests.GetMemberDescriptorsTypeTests;

public sealed class StaticClass_Browable_True_Test
{
    [Browsable(true)]
    static class StaticClass
    {
        [CommandProperty]
        public static int Int { get; set; }

        [CommandProperty]
        [Browsable(true)]
        public static string String { get; set; } = string.Empty;

        [CommandProperty]
        [Browsable(false)]
        public static float Float { get; set; }

        public static double Double { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticClass_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(StaticClass));
        var index = 0;

        Assert.Equal(2, memberDescriptors.Count);
        Assert.Equal(nameof(StaticClass.Int), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass.String), memberDescriptors[index++].MemberName);
    }
}
