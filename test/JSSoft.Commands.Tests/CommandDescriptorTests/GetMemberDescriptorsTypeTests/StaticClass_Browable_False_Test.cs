// <copyright file="StaticClass_Browable_False_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands.Tests.CommandDescriptorTests.GetMemberDescriptorsTypeTests;

public sealed class StaticClass_Browable_False_Test
{
    [Browsable(false)]
    private static class BrowsableFalseStaticClass
    {
        [CommandProperty]
        public static int Int { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BrowsableFalseStaticClass_Test()
    {
        var type = typeof(BrowsableFalseStaticClass);
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(type);

        Assert.Empty(memberDescriptors);
    }
}
