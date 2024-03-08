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

public static class CommandContextBaseExtensions
{
    public static ICommand? GetCommandByCommandLine(this CommandContextBase @this, string commandLine)
    {
        if (CommandUtility.TrySplitCommandLine(commandLine, out var commandName, out var commandArguments) == false)
            return null;
        if (@this.VerifyCommandName(commandName) == false)
            return null;
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
    {
        return @this.ExecuteCommandLineAsync(commandLine, CancellationToken.None);
    }

    public static Task ExecuteCommandLineAsync(this CommandContextBase @this, string commandLine, CancellationToken cancellationToken)
    {
        return ExecuteCommandLineAsync(@this, commandLine, cancellationToken, new Progress<ProgressInfo>());
    }

    public static Task ExecuteCommandLineAsync(this CommandContextBase @this, string commandLine, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        var (commandName, commandArguments) = CommandUtility.SplitCommandLine(commandLine);
        @this.ThrowIfNotVerifyCommandName(commandName);
        return @this.ExecuteAsync(commandArguments, cancellationToken, progress);
    }

    public static Task ExecuteAsync(this CommandContextBase @this, string[] args)
    {
        return @this.ExecuteAsync(args, CancellationToken.None, new Progress<ProgressInfo>());
    }

    public static Task ExecuteAsync(this CommandContextBase @this, string[] args, CancellationToken cancellationToken)
    {
        return @this.ExecuteAsync(args, cancellationToken, new Progress<ProgressInfo>());
    }

    public static Task ExecuteAsync(this CommandContextBase @this, string[] args, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        return @this.ExecuteAsync(args, cancellationToken, progress);
    }

    public static Task ExecuteAsync(this CommandContextBase @this, string argumentLine)
    {
        return ExecuteAsync(@this, argumentLine, CancellationToken.None);
    }

    public static Task ExecuteAsync(this CommandContextBase @this, string argumentLine, CancellationToken cancellationToken)
    {
        return ExecuteAsync(@this, argumentLine, cancellationToken, new Progress<ProgressInfo>());
    }

    public static async Task ExecuteAsync(this CommandContextBase @this, string argumentLine, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        var args = CommandUtility.Split(argumentLine);
        await @this.ExecuteAsync(args, cancellationToken, progress);
    }
}
