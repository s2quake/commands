// <copyright file="CommandInvoker.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using JSSoft.Commands.Extensions;

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
        OnVerify(args);

        var commandName = args.Length > 0 ? args[0] : string.Empty;
        var commandArguments = args.Length > 1 ? args.Skip(1).ToArray() : [];
        var instance = Instance;

        if (instance is ICommand hierarchy
            && hierarchy.TryGetCommand(commandName, out var command) == true)
        {
            var parser = new CommandParser(commandName, command);
            parser.Parse(commandArguments);
            if (command is IExecutable commandExecutable)
            {
                commandExecutable.Execute();
            }
            else if (command is IAsyncExecutable)
            {
                throw new InvalidOperationException("Use InvokeAsync instead.");
            }
        }
        else if (TryGetMethodDescriptor(commandName, out var methodDescriptor) == true)
        {
            if (methodDescriptor.IsAsync == true)
            {
                throw new InvalidOperationException("Use InvokeAsync instead.");
            }
            else
            {
                Invoke(methodDescriptor, commandArguments);
            }
        }
        else if (instance is IExecutable executable)
        {
            Parse(args);
            executable.Execute();
        }
        else if (instance is IAsyncExecutable)
        {
            throw new InvalidOperationException("Use InvokeAsync instead.");
        }
        else
        {
            throw new InvalidOperationException(
                "Instance '{instance.GetType().Name}' must implement IExecutable.");
        }
    }

    public async Task InvokeAsync(
        string[] args, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        OnVerify(args);

        var commandName = args.Length > 0 ? args[0] : string.Empty;
        var commandArguments = args.Length > 0 ? args.Skip(1).ToArray() : [];
        var instance = Instance;
        if (instance is ICommand hierarchy
            && hierarchy.TryGetCommand(commandName, out var command) == true)
        {
            var parser = new CommandParser(commandName, command);
            parser.Parse(commandArguments);
            if (command is IExecutable commandExecutable)
            {
                commandExecutable.Execute();
            }
            else if (command is IAsyncExecutable commandAsyncExecutable1)
            {
                await commandAsyncExecutable1.ExecuteAsync(cancellationToken, progress);
            }
        }
        else if (TryGetMethodDescriptor(commandName, out var methodDescriptor) == true)
        {
            if (methodDescriptor.IsAsync == true)
            {
                await InvokeAsync(
                    methodDescriptor, commandArguments, cancellationToken, progress);
            }
            else
            {
                Invoke(methodDescriptor, commandArguments);
            }
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
        else
        {
            throw new InvalidOperationException(
                $"Instance '{instance.GetType().Name}' must implement IExecutable or " +
                $"IAsyncExecutable.");
        }
    }

    protected virtual void OnVerify(string[] args)
    {
        if (CommandUtility.IsEmptyArgs(args) == true && Settings.AllowEmpty != true)
        {
            throw new CommandInvocationException(this, CommandInvocationError.Empty, args);
        }

        if (Settings.ContainsHelpOption(args) == true)
        {
            throw new CommandInvocationException(this, CommandInvocationError.Help, args);
        }

        if (Settings.ContainsVersionOption(args) == true)
        {
            throw new CommandInvocationException(this, CommandInvocationError.Version, args);
        }
    }

    private bool TryGetMethodDescriptor(
        string commandName,
        [MaybeNullWhen(false)] out CommandMethodDescriptor methodDescriptor)
    {
        var instanceType = InstanceType;
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(instanceType);
        methodDescriptor = methodDescriptors.FindByName(commandName)!;
        return methodDescriptor is not null;
    }

    private void Invoke(
        CommandMethodDescriptor methodDescriptor, string[] args)
    {
        var instance = Instance;
        var memberDescriptors = methodDescriptor.Members;
        var settings = Settings;
        var parseContext = new ParseContext(memberDescriptors, settings, args);
        var methodInstance = new CommandMethodInstance(methodDescriptor, instance);
        parseContext.SetValue(methodInstance);
        methodDescriptor.Invoke(instance, methodInstance);
    }

    private Task InvokeAsync(
        CommandMethodDescriptor methodDescriptor,
        string[] args,
        CancellationToken cancellationToken,
        IProgress<ProgressInfo> progress)
    {
        var instance = Instance;
        var memberDescriptors = methodDescriptor.Members;
        var settings = Settings;
        var parseContext = new ParseContext(memberDescriptors, settings, args);
        var methodInstance = new CommandMethodInstance(methodDescriptor, instance);
        parseContext.SetValue(methodInstance);
        return methodDescriptor.InvokeAsync(
            instance, methodInstance, cancellationToken, progress);
    }

    private void Parse(string[] args)
    {
        var instance = Instance;
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(instance);
        var settings = Settings;
        var parserContext = new ParseContext(memberDescriptors, settings, args);
        parserContext.SetValue(instance);
    }
}
