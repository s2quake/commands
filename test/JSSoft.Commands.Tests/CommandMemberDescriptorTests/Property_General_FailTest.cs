// <copyright file="Property_General_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections;
using System.Threading;

namespace JSSoft.Commands.Tests.CommandMemberDescriptorTests;

public class Property_General_FailTest
{
    private sealed class CancellationTokenClass
    {
        [CommandProperty]
        public CancellationToken Member1 { get; set; }
    }

    [Fact]
    public void CancellationTokenClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(CancellationTokenClass)));
    }

    private sealed class EnumerableClass
    {
        [CommandProperty]
        public IEnumerable Member1 { get; set; } = Array.Empty<object>();
    }

    [Fact]
    public void EnumerableClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(EnumerableClass)));
    }

    private sealed class ListClass
    {
        [CommandProperty]
        public List<string> Member1 { get; set; } = [];
    }

    [Fact]
    public void ListClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(ListClass)));
    }

    private sealed class DictionaryClass
    {
        [CommandProperty]
        public Dictionary<string, string> Member1 { get; set; } = [];
    }

    [Fact]
    public void DictionaryClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(DictionaryClass)));
    }
}
