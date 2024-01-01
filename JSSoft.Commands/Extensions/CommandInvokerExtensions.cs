// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Extensions;

public static class CommandInvokerExtensions
{
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
    {
        return @this.InvokeAsync(args, CancellationToken.None, new Progress<ProgressInfo>());
    }

    public static Task InvokeAsync(this CommandInvoker @this, string argumentLine)
    {
        return @this.InvokeAsync(argumentLine, CancellationToken.None);
    }

    public static Task InvokeAsync(this CommandInvoker @this, string argumentLine, CancellationToken cancellationToken)
    {
        return InvokeAsync(@this, argumentLine, cancellationToken, new Progress<ProgressInfo>());
    }

    public static Task InvokeAsync(this CommandInvoker @this, string argumentLine, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        var args = CommandUtility.Split(argumentLine);
        return @this.InvokeAsync(args, cancellationToken, progress);
    }

    public static Task InvokeCommandLineAsync(this CommandInvoker @this, string commandLine)
    {
        return @this.InvokeCommandLineAsync(commandLine, CancellationToken.None);
    }

    public static Task InvokeCommandLineAsync(this CommandInvoker @this, string commandLine, CancellationToken cancellationToken)
    {
        return InvokeCommandLineAsync(@this, commandLine, cancellationToken, new Progress<ProgressInfo>());
    }

    public static Task InvokeCommandLineAsync(this CommandInvoker @this, string commandLine, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
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
    {
        return @this.TryInvokeAsync(args, CancellationToken.None);
    }

    public static Task<bool> TryInvokeAsync(this CommandInvoker @this, string[] args, CancellationToken cancellationToken)
    {
        return TryInvokeAsync(@this, args, cancellationToken, new Progress<ProgressInfo>());
    }

    public static async Task<bool> TryInvokeAsync(this CommandInvoker @this, string[] args, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
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

    public static Task<bool> TryInvokeCommandLineAsync(this CommandInvoker @this, string commandLine)
    {
        return @this.TryInvokeCommandLineAsync(commandLine, CancellationToken.None);
    }

    public static async Task<bool> TryInvokeCommandLineAsync(this CommandInvoker @this, string commandLine, CancellationToken cancellationToken)
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
        // var (commandName, commandArguments) = CommandUtility.SplitCommandLine(commandLine);
        // ThrowIfNotVerifyCommandName(commandName);
        // return @this.TryInvokeAsync(commandArguments, cancellationToken);
    }

    public static Task<bool> TryInvokeAsync(this CommandInvoker @this, string argumentLine)
    {
        return @this.TryInvokeAsync(argumentLine, CancellationToken.None);
    }

    public static Task<bool> TryInvokeAsync(this CommandInvoker @this, string argumentLine, CancellationToken cancellationToken)
    {
        var args = CommandUtility.Split(argumentLine);
        return @this.TryInvokeAsync(args, cancellationToken);
    }
}
