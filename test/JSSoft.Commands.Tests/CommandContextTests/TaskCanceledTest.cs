// <copyright file="TaskCanceledTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Tests.CommandContextTests;

public class TaskCanceledTest
{
    [Fact]
    public async Task InvokeAsync_Canceled_ThrowTestAsync()
    {
        var commands = new ICommand[]
        {
            new TestCommand(),
        };
        var commandContext = new TestCommandContext(commands);
        using var cancellationTokenSource = new CancellationTokenSource(10);
        await Assert.ThrowsAsync<TaskCanceledException>(
            async () => await commandContext.ExecuteAsync(
                "test method", cancellationTokenSource.Token));
    }

    private sealed class TestCommandContext(params ICommand[] commands)
        : CommandContextBase(commands)
    {
    }

    private sealed class TestCommand : CommandMethodBase
    {
        [CommandMethod]
        public async Task MethodAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken);
        }
    }
}
