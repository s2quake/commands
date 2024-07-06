// <copyright file="TestCommandExtension.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading.Tasks;

namespace JSSoft.Commands.Applications.Commands;

static class TestCommandExtension
{
    public static bool CanStart => true;

    public static Task<string[]> CompleteStartAsync(CommandMemberDescriptor memberDescriptor, string find)
    {
        return Task.Run(() => new string[] { "a", "b", "c" });
    }

    public static string[] CompleteStart(CommandMemberDescriptor memberDescriptor, string find)
    {
        return ["a", "b", "c"];
    }
}
