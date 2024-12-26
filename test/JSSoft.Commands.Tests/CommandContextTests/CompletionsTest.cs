// <copyright file="CompletionsTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Tests.CommandContextTests;

public class CompletionsTest
{
    private static readonly ICommand[] _commands =
    [
        new Test2Command(),
        new Test1Command(),
        new AddCommand(),
        new NodeCommand(),
    ];

    private static readonly TestCommandContext _commandContext = new(_commands);

    [Fact]
    public void CommandCompletion1_Test()
    {
        var completions = _commandContext.GetCompletions([], string.Empty);
        var i = 0;
        Assert.Equal("test2", completions[i++]);
        Assert.Equal("test1", completions[i++]);
        Assert.Equal("add", completions[i++]);
        Assert.Equal("node", completions[i++]);
        Assert.Equal("help", completions[i++]);
        Assert.Equal("version", completions[i++]);
        Assert.Equal(i, completions.Length);
    }

    [Fact]
    public void CommandCompletion2_Test()
    {
        var completions = _commandContext.GetCompletions([], "h");
        var i = 0;
        Assert.Equal("help", completions[i++]);
        Assert.Equal(i, completions.Length);
    }

    [Fact]
    public void CommandCompletion3_Test()
    {
        var completions = _commandContext.GetCompletions([], "t");
        var i = 0;
        Assert.Equal("test1", completions[i++]);
        Assert.Equal("test2", completions[i++]);
        Assert.Equal(i, completions.Length);
    }

    private sealed class Test1Command : CommandBase
    {
        [CommandPropertyRequired]
        [CommandPropertyCompletion(nameof(GetTextCompletions))]
        public string Text { get; set; } = string.Empty;

        [CommandPropertyExplicitRequired]
        [CommandPropertyCompletion(nameof(GetMessageCompletions))]
        public string Message { get; set; } = string.Empty;

        protected override void OnExecute()
        {
            // do nothing
        }

        private static string[] GetTextCompletions()
            => ["text2", "wow1", "text1", string.Empty, "wow2"];

        private static string[] GetMessageCompletions()
            => ["text2_e", "wow1_e", "text1_e", "wow2_e"];
    }

    [Fact]
    public void Test1CommandCompletion1_Test()
    {
        var completions = _commandContext.GetCompletions(["test1"], string.Empty);
        var i = 0;
        Assert.Equal("text2", completions[i++]);
        Assert.Equal("wow1", completions[i++]);
        Assert.Equal("text1", completions[i++]);
        Assert.Equal("wow2", completions[i++]);
        Assert.Equal(i, completions.Length);
    }

    [Fact]
    public void Test1CommandCompletion2_Test()
    {
        var completions = _commandContext.GetCompletions(["test1", "wow"], string.Empty);
        var i = 0;
        Assert.Equal(i, completions.Length);
    }

    [Fact]
    public void Test1CommandCompletion3_Test()
    {
        var completions = _commandContext.GetCompletions(
            ["test1", "wow", "--message"], string.Empty);
        var i = 0;
        Assert.Equal("text2_e", completions[i++]);
        Assert.Equal("wow1_e", completions[i++]);
        Assert.Equal("text1_e", completions[i++]);
        Assert.Equal("wow2_e", completions[i++]);
        Assert.Equal(i, completions.Length);
    }

    private sealed class Test2Command : CommandAsyncBase
    {
        [CommandPropertyArray]
        [CommandPropertyCompletion(nameof(GetItemCompletions))]
        public string[] Items { get; set; } = [];

        protected override Task OnExecuteAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        private static string[] GetItemCompletions()
            => ["none", "type_a", "unknown", "all"];
    }

    public static readonly IEnumerable<object[]> Test2CommandCompletion1TestMembers =
    [
        [Array.Empty<string>()],
        [new string[] { "--" }],
        [new string[] { "none" }],
        [new string[] { "all", "unknown" }],
    ];

    [Theory]
    [MemberData(nameof(Test2CommandCompletion1TestMembers))]
    public void Test2CommandCompletion1_Test(string[] items)
    {
        var completions = _commandContext.GetCompletions(
            ["test2", .. items], string.Empty);
        var i = 0;
        Assert.Equal("none", completions[i++]);
        Assert.Equal("type_a", completions[i++]);
        Assert.Equal("unknown", completions[i++]);
        Assert.Equal("all", completions[i++]);
        Assert.Equal(i, completions.Length);
    }

    public static readonly IEnumerable<object[]> Test2CommandCompletion2TestMembers =
    [
        [Array.Empty<string>(), "n", new string[] { "none" }],
        [new string[] { "--" }, "u", new string[] { "unknown" }],
        [new string[] { "none" }, "a", new string[] { "all" }],
        [new string[] { "all", "unknown" }, "a", new string[] { "all" }],
    ];

    [Theory]
    [MemberData(nameof(Test2CommandCompletion2TestMembers))]
    public void Test2CommandCompletion2_Test(string[] items, string find, string[] expectedItems)
    {
        var completions = _commandContext.GetCompletions(
            ["test2", .. items], find);
        Assert.Equal(expectedItems, completions);
    }

    private abstract class AddCommandBase : CommandBase
    {
        [CommandProperty]
        [CommandPropertyCompletion(nameof(GetBasePropertyCompletions))]
        public string BaseProperty { get; set; } = string.Empty;

        private static string[] GetBasePropertyCompletions() => ["prop2", "prop1"];
    }

    private sealed class AddCommand : AddCommandBase
    {
        [CommandProperty]
        public string Text { get; set; } = string.Empty;

        protected override void OnExecute()
        {
            // do nothing
        }

        protected override string[] GetCompletions(CommandCompletionContext completionContext)
        {
            if (completionContext.MemberDescriptor.MemberName == nameof(Text))
            {
                return ["text1", "text2"];
            }

            return base.GetCompletions(completionContext);
        }
    }

    [Fact]
    public void AddCommandCompletion1_Test()
    {
        var completions = _commandContext.GetCompletions(["add", "--text"], string.Empty);
        var i = 0;
        Assert.Equal("text1", completions[i++]);
        Assert.Equal("text2", completions[i++]);
        Assert.Equal(i, completions.Length);
    }

    [Fact]
    public void AddCommandCompletion2_Test()
    {
        var completions = _commandContext.GetCompletions(["add", "--base-property"], string.Empty);
        var i = 0;
        Assert.Equal("prop2", completions[i++]);
        Assert.Equal("prop1", completions[i++]);
        Assert.Equal(i, completions.Length);
    }

    [Fact]
    public void AddCommandCompletion3_Test()
    {
        var completions = _commandContext.GetCompletions(
            ["add", "--base-property"], "p");
        var i = 0;
        Assert.Equal("prop1", completions[i++]);
        Assert.Equal("prop2", completions[i++]);
        Assert.Equal(i, completions.Length);
    }

    private sealed class NodeCommand : CommandMethodBase
    {
        [CommandMethod]
        public void Start(
            [CommandParameterCompletion(nameof(GetPortCompletions))] int port)
        {
            // do nothing
        }

        private string[] GetPortCompletions() => ["8080", "8081", "8082"];
    }

    [Fact]
    public void NodeCommandCompletion1_Test()
    {
        var completions = _commandContext.GetCompletions(["node", "start"], string.Empty);
        var i = 0;
        Assert.Equal("8080", completions[i++]);
        Assert.Equal("8081", completions[i++]);
        Assert.Equal("8082", completions[i++]);
        Assert.Equal(i, completions.Length);
    }
}
