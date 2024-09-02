// <copyright file="CommandContextBaseExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Extensions;

public static class CommandContextBaseExtensions
{
    private static readonly Progress<ProgressInfo> EmptyProgress = new();

    public static ICommand? GetCommandByCommandLine(
        this CommandContextBase @this, string commandLine)
    {
        if (CommandUtility.TrySplitCommandLine(
            commandLine, out var commandName, out var commandArguments) is false)
        {
            return null;
        }

        if (@this.CheckCommandName(commandName) is false)
        {
            return null;
        }

        return @this.GetCommand(commandArguments);
    }

    public static void ExecuteCommandLine(this CommandContextBase @this, string commandLine)
    {
        var (commandName, commandArguments) = CommandUtility.SplitCommandLine(commandLine);
        @this.ThrowIfNotVerifyCommandName(commandName);
        @this.Execute(commandArguments);
    }

    public static void Execute(this CommandContextBase @this, string argumentLine)
    {
        var args = CommandUtility.Split(argumentLine);
        @this.Execute(args);
    }

    public static Task ExecuteCommandLineAsync(this CommandContextBase @this, string commandLine)
        => @this.ExecuteCommandLineAsync(commandLine, CancellationToken.None);

    public static Task ExecuteCommandLineAsync(
        this CommandContextBase @this, string commandLine, CancellationToken cancellationToken)
        => ExecuteCommandLineAsync(@this, commandLine, cancellationToken, EmptyProgress);

    public static Task ExecuteCommandLineAsync(
        this CommandContextBase @this,
        string commandLine,
        CancellationToken cancellationToken,
        IProgress<ProgressInfo> progress)
    {
        var (commandName, commandArguments) = CommandUtility.SplitCommandLine(commandLine);
        @this.ThrowIfNotVerifyCommandName(commandName);
        return @this.ExecuteAsync(commandArguments, cancellationToken, progress);
    }

    public static Task ExecuteAsync(this CommandContextBase @this, string[] args)
        => @this.ExecuteAsync(args, CancellationToken.None, EmptyProgress);

    public static Task ExecuteAsync(
        this CommandContextBase @this, string[] args, CancellationToken cancellationToken)
        => @this.ExecuteAsync(args, cancellationToken, EmptyProgress);

    public static Task ExecuteAsync(
        this CommandContextBase @this,
        string[] args,
        CancellationToken cancellationToken,
        IProgress<ProgressInfo> progress)
    {
        return @this.ExecuteAsync(args, cancellationToken, progress);
    }

    public static Task ExecuteAsync(this CommandContextBase @this, string argumentLine)
        => ExecuteAsync(@this, argumentLine, CancellationToken.None);

    public static Task ExecuteAsync(
        this CommandContextBase @this, string argumentLine, CancellationToken cancellationToken)
        => ExecuteAsync(@this, argumentLine, cancellationToken, EmptyProgress);

    public static async Task ExecuteAsync(
        this CommandContextBase @this,
        string argumentLine,
        CancellationToken cancellationToken,
        IProgress<ProgressInfo> progress)
    {
        var args = CommandUtility.Split(argumentLine);
        await @this.ExecuteAsync(args, cancellationToken, progress);
    }
}
