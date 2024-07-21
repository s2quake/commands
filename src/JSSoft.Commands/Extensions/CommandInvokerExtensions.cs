// <copyright file="CommandInvokerExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Extensions;

public static class CommandInvokerExtensions
{
    private static readonly Progress<ProgressInfo> EmptyProgress = new();

    public static void Invoke(this CommandInvoker @this, string argumentLine)
    {
        var args = CommandUtility.Split(argumentLine);
        @this.Invoke(args);
    }

    public static void InvokeCommandLine(this CommandInvoker @this, string commandLine)
    {
        var (commandName, commandArguments) = CommandUtility.SplitCommandLine(commandLine);
        @this.ThrowIfNotVerifyCommandName(commandName);
        @this.Invoke(commandArguments);
    }

    public static Task InvokeAsync(this CommandInvoker @this, string[] args)
        => @this.InvokeAsync(args, CancellationToken.None, EmptyProgress);

    public static Task InvokeAsync(this CommandInvoker @this, string argumentLine)
        => @this.InvokeAsync(argumentLine, CancellationToken.None);

    public static Task InvokeAsync(
        this CommandInvoker @this,
        string argumentLine,
        CancellationToken cancellationToken)
        => InvokeAsync(@this, argumentLine, cancellationToken, EmptyProgress);

    public static Task InvokeAsync(
        this CommandInvoker @this,
        string argumentLine,
        CancellationToken cancellationToken,
        IProgress<ProgressInfo> progress)
    {
        var args = CommandUtility.Split(argumentLine);
        return @this.InvokeAsync(args, cancellationToken, progress);
    }

    public static Task InvokeCommandLineAsync(this CommandInvoker @this, string commandLine)
        => @this.InvokeCommandLineAsync(commandLine, CancellationToken.None);

    public static Task InvokeCommandLineAsync(
        this CommandInvoker @this,
        string commandLine,
        CancellationToken cancellationToken)
        => InvokeCommandLineAsync(@this, commandLine, cancellationToken, EmptyProgress);

    public static Task InvokeCommandLineAsync(
        this CommandInvoker @this,
        string commandLine,
        CancellationToken cancellationToken,
        IProgress<ProgressInfo> progress)
    {
        var (commandName, commandArguments) = CommandUtility.SplitCommandLine(commandLine);
        @this.ThrowIfNotVerifyCommandName(commandName);
        return @this.InvokeAsync(commandArguments, cancellationToken, progress);
    }

    public static bool TryInvoke(this CommandInvoker @this, string[] args)
    {
        try
        {
            @this.Invoke(args);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool TryInvoke(this CommandInvoker @this, string argumentLine)
    {
        var args = CommandUtility.Split(argumentLine);
        return @this.TryInvoke(args);
    }

    public static bool TryInvokeCommandLine(this CommandInvoker @this, string commandLine)
    {
        var (commandName, commandArguments) = CommandUtility.SplitCommandLine(commandLine);
        @this.ThrowIfNotVerifyCommandName(commandName);
        return @this.TryInvoke(commandArguments);
    }

    public static Task<bool> TryInvokeAsync(this CommandInvoker @this, string[] args)
        => @this.TryInvokeAsync(args, CancellationToken.None);

    public static Task<bool> TryInvokeAsync(
        this CommandInvoker @this,
        string[] args,
        CancellationToken cancellationToken)
        => TryInvokeAsync(@this, args, cancellationToken, EmptyProgress);

    public static async Task<bool> TryInvokeAsync(
        this CommandInvoker @this,
        string[] args,
        CancellationToken cancellationToken,
        IProgress<ProgressInfo> progress)
    {
        try
        {
            await @this.InvokeAsync(args, cancellationToken, progress);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static Task<bool> TryInvokeAsync(this CommandInvoker @this, string argumentLine)
        => @this.TryInvokeAsync(argumentLine, CancellationToken.None);

    public static Task<bool> TryInvokeAsync(
        this CommandInvoker @this, string argumentLine, CancellationToken cancellationToken)
    {
        var args = CommandUtility.Split(argumentLine);
        return @this.TryInvokeAsync(args, cancellationToken);
    }

    public static Task<bool> TryInvokeCommandLineAsync(
        this CommandInvoker @this,
        string commandLine)
        => @this.TryInvokeCommandLineAsync(commandLine, CancellationToken.None);

    public static async Task<bool> TryInvokeCommandLineAsync(
        this CommandInvoker @this,
        string commandLine,
        CancellationToken cancellationToken)
    {
        try
        {
            await @this.InvokeCommandLineAsync(commandLine, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
