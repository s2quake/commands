// <copyright file="CustomCommandTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Tests.CommandContextTests;

public class CustomCommandTest
{
    [Fact]
    public void ExecutableNotImplemented_ThrowTest()
    {
        var customCommand = new CustomCommandWithoutExecutable();
        Assert.Throws<CommandDefinitionException>(
            () => new TestCommandContext(customCommand));
    }

    private sealed class CustomCommandWithoutExecutable : ICommand
    {
        public string Name => "test";

        public string[] Aliases => [];

        public bool IsEnabled => true;

        public bool AllowsSubCommands => false;

        public string Summary => nameof(Summary);

        public string Description => nameof(Description);

        public string Example => nameof(Example);

        public string Category => string.Empty;

        public ICommand? Parent { get; set; }

        public CommandCollection Commands { get; } = [];

        public ICommandContext? Context { get; set; }

        public string[] GetCompletions(CommandCompletionContext completionContext) => [];

        public string GetUsage(bool isDetail) => string.Empty;
    }

    [Fact]
    public void ExecutableImplemented_Test()
    {
        var customCommand = new CustomCommandWithExecutable();
        var commandContext = new TestCommandContext(customCommand);
        commandContext.Execute("test");

        Assert.Equal(int.MaxValue, customCommand.Value);
    }

    private sealed class CustomCommandWithExecutable : ICommand, IExecutable
    {
        public string Name => "test";

        public string[] Aliases => [];

        public bool IsEnabled => true;

        public bool AllowsSubCommands => false;

        public string Summary => nameof(Summary);

        public string Description => nameof(Description);

        public string Example => nameof(Example);

        public string Category => string.Empty;

        public ICommand? Parent { get; set; }

        public CommandCollection Commands { get; } = [];

        public ICommandContext? Context { get; set; }

        public int Value { get; set; }

        public void Execute()
        {
            Value = int.MaxValue;
        }

        public string[] GetCompletions(CommandCompletionContext completionContext) => [];

        public string GetUsage(bool isDetail) => string.Empty;
    }

    [Fact]
    public async Task AsyncExecutableImplemented_TestAsync()
    {
        var customCommand = new CustomCommandWithAsyncExecutable();
        var commandContext = new TestCommandContext(customCommand);
        await commandContext.ExecuteAsync("test");

        Assert.Equal(int.MaxValue, customCommand.Value);
    }

    private sealed class CustomCommandWithAsyncExecutable : ICommand, IAsyncExecutable
    {
        public string Name => "test";

        public string[] Aliases => [];

        public bool IsEnabled => true;

        public bool AllowsSubCommands => false;

        public string Summary => nameof(Summary);

        public string Description => nameof(Description);

        public string Example => nameof(Example);

        public string Category => string.Empty;

        public ICommand? Parent { get; set; }

        public CommandCollection Commands { get; } = [];

        public ICommandContext? Context { get; set; }

        public int Value { get; set; }

        public Task ExecuteAsync(
            CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
        {
            Value = int.MaxValue;
            return Task.CompletedTask;
        }

        public string[] GetCompletions(CommandCompletionContext completionContext) => [];

        public string GetUsage(bool isDetail) => string.Empty;
    }
}
