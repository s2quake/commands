// <copyright file="StaticClass_Browable_False_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands.Test.CommandDescriptorTests.GetMemberDescriptorsTypeTests;

public sealed class StaticClass_Browable_False_Test
{
    [Browsable(false)]
    static class BrowsableFalseStaticClass
    {
        [CommandProperty]
        public static int Int { get; set; }
    }

    [Fact]
    public void GetMemberDescriptors_Arg0_BrowsableFalseStaticClass_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(BrowsableFalseStaticClass));

        Assert.Empty(memberDescriptors);
    }
}
