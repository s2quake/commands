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

namespace JSSoft.Commands;

public class CommandInvoker : CommandAnalyzer
{
    public CommandInvoker(object instance)
       : base(Assembly.GetEntryAssembly() ?? typeof(CommandParser).Assembly, instance)
    {
    }

    public CommandInvoker(object instance, CommandSettings settings)
        : base(Assembly.GetEntryAssembly() ?? typeof(CommandParser).Assembly, instance, settings)
    {
    }

    public CommandInvoker(string name, object instance)
        : base(name, instance)
    {
    }

    public CommandInvoker(string name, object instance, CommandSettings settings)
        : base(name, instance, settings)
    {
    }

    public CommandInvoker(Assembly assembly, object instance)
        : base(assembly, instance)
    {
    }

    public CommandInvoker(Assembly assembly, object instance, CommandSettings settings)
        : base(assembly, instance, settings)
    {
    }

    public void Invoke(string[] args)
    {
        OnValidate(args);

        var commandName = args.Length > 0 ? args[0] : string.Empty;
        var commandArguments = args.Length > 1 ? args.Skip(1).ToArray() : Array.Empty<string>();
        var instance = Instance;

        if (instance is ICommandHierarchy hierarchy && hierarchy.Commands.ContainsKey(commandName) == true)
        {
            var command = hierarchy.Commands[commandName];
            var parser = new CommandParser(commandName, command);
            parser.Parse(commandArguments);
            if (command is IExecutable commandExecutable)
                commandExecutable.Execute();
            else if (command is IAsyncExecutable commandAsyncExecutable)
                throw new InvalidOperationException("Use InvokeAsync instead.");
        }
        else if (instance is IExecutable executable)
        {
            Parse(args);
            executable.Execute();
        }
        else if (instance is IAsyncExecutable asyncExecutable)
        {
            throw new InvalidOperationException("Use InvokeAsync instead.");
        }
        else if (CommandDescriptor.GetMethodDescriptors(instance.GetType()).FindByName(commandName) is { } methodDescriptor)
        {
            if (methodDescriptor.IsAsync == true)
            {
                throw new InvalidOperationException("Use InvokeAsync instead.");
            }
            else
            {
                Invoke(methodDescriptor, instance, commandArguments);
            }
        }
    }

    public async Task InvokeAsync(string[] args, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        OnValidate(args);

        var commandName = args.Length > 0 ? args[0] : string.Empty;
        var commandArguments = args.Length > 0 ? args.Skip(1).ToArray() : Array.Empty<string>();
        var instance = Instance;
        if (instance is ICommandHierarchy hierarchy && hierarchy.Commands.ContainsKey(commandName) == true)
        {
            var command = hierarchy.Commands[commandName];
            var parser = new CommandParser(commandName, command);
            parser.Parse(commandArguments);
            if (command is IExecutable commandExecutable)
                commandExecutable.Execute();
            else if (command is IAsyncExecutable commandAsyncExecutable1)
                await commandAsyncExecutable1.ExecuteAsync(cancellationToken, progress);
        }
        else if (instance is IExecutable executable)
        {
            Parse(args);
            executable.Execute();
        }
        else if (instance is IAsyncExecutable asyncExecutable)
        {
            Parse(args);
            await asyncExecutable.ExecuteAsync(cancellationToken, progress);
        }
        else if (commandName != string.Empty && CommandDescriptor.GetMethodDescriptors(instance.GetType()).FindByName(commandName) is { } methodDescriptor)
        {
            if (methodDescriptor.IsAsync == true)
                await InvokeAsync(methodDescriptor, instance, commandArguments, cancellationToken, progress);
            else
                Invoke(methodDescriptor, instance, commandArguments);
        }
    }

    protected virtual void OnValidate(string[] args)
    {
        if (CommandUtility.IsEmptyArgs(args) == true)
            throw new CommandInvocationException(this, CommandInvocationError.Empty, args);
        if (Settings.ContainsHelpOption(args) == true)
            throw new CommandInvocationException(this, CommandInvocationError.Help, args);
        if (Settings.ContainsVersionOption(args) == true)
            throw new CommandInvocationException(this, CommandInvocationError.Version, args);
    }

    private static void Invoke(CommandMethodDescriptor methodDescriptor, object instance, string[] args)
    {
        methodDescriptor.Invoke(instance, args, methodDescriptor.Members);
    }

    private static Task InvokeAsync(CommandMethodDescriptor methodDescriptor, object instance, string[] args, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        return methodDescriptor.InvokeAsync(instance, args, methodDescriptor.Members, cancellationToken, progress);
    }

    private void Parse(string[] args)
    {
        var instance = Instance;
        var commandName = args.Length > 0 ? args[0] : string.Empty;
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(instance);
        var parserContext = new ParseContext(memberDescriptors, args);
        parserContext.SetValue(instance);
    }
}
