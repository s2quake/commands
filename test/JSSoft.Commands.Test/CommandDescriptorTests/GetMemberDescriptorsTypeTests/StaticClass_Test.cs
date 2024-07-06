// <copyright file="StaticClass_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands.Test.CommandDescriptorTests.GetMemberDescriptorsTypeTests;

public sealed class StaticClass_Test
{
    static class StaticClass
    {
        [CommandPropertyRequired]
        public static int Int { get; set; }

        [CommandPropertySwitch]
        public static bool Bool { get; set; }

        [CommandProperty]
        [Browsable(true)]
        public static string String { get; set; } = string.Empty;

        [CommandProperty]
        [Browsable(false)]
        public static float Float { get; set; }

        public static double Double { get; set; }

        [CommandPropertyArray]
        public static string[] Arguments { get; set; } = [];
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_StaticClassType_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(StaticClass));
        var index = 0;

        Assert.Equal(4, memberDescriptors.Count);
        Assert.Equal(nameof(StaticClass.Int), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass.Arguments), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass.String), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(StaticClass.Bool), memberDescriptors[index++].MemberName);
    }
}
