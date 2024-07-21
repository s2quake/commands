// <copyright file="CommandMemberDescriptorCollectionTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests;

public class CommandMemberDescriptorCollectionTest
{
    private sealed class StyleCommand
    {
        [CommandPropertySwitch('i')]
        [CommandPropertyCondition(nameof(StyleName), "", IsNot = true)]
        public bool IsDetail { get; set; }

        [CommandPropertyRequired(DefaultValue = "")]
        public string StyleName { get; set; } = string.Empty;
    }

    [Fact]
    public void StyleCommand_PropertyOrder_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(StyleCommand));
        Assert.Equal(2, memberDescriptors.Count);
        Assert.Equal(nameof(StyleCommand.StyleName), memberDescriptors[0].MemberName);
        Assert.Equal(nameof(StyleCommand.IsDetail), memberDescriptors[1].MemberName);
    }
}
