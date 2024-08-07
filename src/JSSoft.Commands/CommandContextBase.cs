﻿// <copyright file="CommandContextBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

public abstract class CommandContextBase : ICommandContext
{
    private readonly CommandNode _commandNode;
    private readonly string _fullName;
    private readonly string _filename;

    protected CommandContextBase(IEnumerable<ICommand> commands)
        : this(GetDefaultAssembly(), commands, settings: default)
    {
    }

    protected CommandContextBase(IEnumerable<ICommand> commands, CommandSettings settings)
        : this(GetDefaultAssembly(), commands, settings)
    {
    }

    protected CommandContextBase(Assembly assembly, IEnumerable<ICommand> commands)
        : this(assembly, commands, settings: default)
    {
    }

    protected CommandContextBase(
        Assembly assembly, IEnumerable<ICommand> commands, CommandSettings settings)
    {
        Settings = settings;
        _fullName = AssemblyUtility.GetAssemblyLocation(assembly);
        _filename = Path.GetFileName(_fullName);
        Name = Path.GetFileNameWithoutExtension(_fullName);
        Version = AssemblyUtility.GetAssemblyVersion(assembly);
        Copyright = AssemblyUtility.GetAssemblyCopyright(assembly);
        _commandNode = new CommandNode(this);
        Initialize(_commandNode, commands);
    }

    protected CommandContextBase(string name, IEnumerable<ICommand> commands)
        : this(name, commands, settings: default)
    {
    }

    protected CommandContextBase(
        string name, IEnumerable<ICommand> commands, CommandSettings settings)
    {
        ThrowUtility.ThrowIfEmpty(name, nameof(name));
        Settings = settings;
        _fullName = name;
        _filename = name;
        Name = name;
        _commandNode = new CommandNode(this);
        Initialize(_commandNode, commands);
    }

    public event EventHandler? Executed;

    public TextWriter Out { get; set; } = Console.Out;

    public TextWriter Error { get; set; } = Console.Error;

    public string Name { get; }

    public string Version { get; set; } = $"{new Version(1, 0)}";

    public string Copyright { get; set; } = string.Empty;

    public ICommandNode Node => _commandNode;

    public string ExecutionName
    {
        get
        {
            if (Owner is null)
            {
#if NETCOREAPP
                if (_filename != Name)
                {
                    return $"dotnet {_filename}";
                }
#elif NETFRAMEWORK
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    return $"mono {_filename}";
                }
#endif
                return Name;
            }

            return string.Empty;
        }
    }

    public object? Owner { get; set; }

    public CommandSettings Settings { get; }

    protected virtual ICommand HelpCommand { get; } = new InternalHelpCommand();

    protected virtual ICommand VersionCommand { get; } = new InternalVersionCommand();

    public ICommand? GetCommand(string[] args)
    {
        var argList = new List<string>(args);
        return GetCommand(_commandNode, argList);
    }

    public void Execute(string[] args)
    {
        ExecuteInternal(args);
        OnExecuted(EventArgs.Empty);
    }

    public async Task ExecuteAsync(
        string[] args, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        await ExecuteInternalAsync(args, cancellationToken, progress);
        OnExecuted(EventArgs.Empty);
    }

    public string[] GetCompletion(string[] items, string find)
        => GetCompletion(_commandNode, new List<string>(items), find);

    internal static ICommand? GetCommand(ICommandNode parentNode, IList<string> argList)
    {
        if (argList.FirstOrDefault() is { } commandName
            && parentNode.TryGetEnabledCommand(commandName, out var commandNode) == true)
        {
            argList.RemoveAt(0);
            if (argList.Count > 0 && commandNode.Children.Count != 0)
            {
                return GetCommand(commandNode, argList);
            }

            return commandNode.Commands.FirstOrDefault();
        }

        return null;
    }

    internal void ThrowIfNotVerifyCommandName(string commandName)
    {
        if (CheckCommandName(commandName) != true)
        {
            var message = $"Command name '{commandName}' is not available.";
            throw new ArgumentException(message, nameof(commandName));
        }
    }

    internal bool CheckCommandName(string commandName)
    {
        if (Name == commandName)
        {
            return true;
        }

        if (_fullName == commandName)
        {
            return true;
        }

        if (_filename == commandName)
        {
            return true;
        }

        return false;
    }

    protected virtual void OnEmptyExecute()
    {
        var helpNames = GetHelpNames();
        var versionNames = GetVersionNames();
        var sb = new StringBuilder();
        if (helpNames != string.Empty && HelpCommand is not null)
        {
            sb.AppendLine($"Type '{helpNames}' for usage.");
        }

        if (versionNames != string.Empty && VersionCommand is not null)
        {
            sb.AppendLine($"Type '{versionNames}' for version.");
        }

        Out.Write(sb.ToString());

        string GetHelpNames()
        {
            var itemList = new List<string>(2);
            if (Settings.HelpName != string.Empty)
            {
                itemList.Add($"{CommandUtility.Delimiter}{Settings.HelpName}");
            }

            if (Settings.HelpShortName != char.MinValue)
            {
                itemList.Add($"{CommandUtility.ShortDelimiter}{Settings.HelpShortName}");
            }

            if (ExecutionName != string.Empty)
            {
                return $"{ExecutionName} {string.Join(" | ", itemList)}";
            }

            return string.Join(" | ", itemList);
        }

        string GetVersionNames()
        {
            var itemList = new List<string>(2);
            if (Settings.VersionName != string.Empty)
            {
                itemList.Add($"{CommandUtility.Delimiter}{Settings.VersionName}");
            }

            if (Settings.VersionShortName != char.MinValue)
            {
                itemList.Add($"{CommandUtility.ShortDelimiter}{Settings.VersionShortName}");
            }

            if (ExecutionName != string.Empty)
            {
                return $"{ExecutionName} {string.Join(" | ", itemList)}";
            }

            return string.Join(" | ", itemList);
        }
    }

    protected virtual void OnHelpExecute(string[] args)
    {
        var items = args.Where(item => Settings.IsHelpArg(item) != true).ToArray();
        var helpCommand = HelpCommand;
        var invoker = new InternalCommandInvoker(helpCommand);
        invoker.Invoke(items);
    }

    protected virtual void OnVersionExecute(string[] args)
    {
        var items = args.Where(item => Settings.IsVersionArg(item) != true).ToArray();
        var versionCommand = VersionCommand;
        var invoker = new InternalCommandInvoker(versionCommand);
        invoker.Invoke(items);
    }

    protected virtual void OnExecuted(EventArgs e)
    {
        Executed?.Invoke(this, e);
    }

    private static Assembly GetDefaultAssembly()
        => Assembly.GetEntryAssembly() ?? typeof(CommandParser).Assembly;

    private static string[] GetCompletion(
        ICommandNode parentNode, IList<string> itemList, string find)
    {
        if (itemList.Count == 0)
        {
            var query = from child in parentNode.Children
                        where child.IsEnabled == true
                        from name in new string[] { child.Name }.Concat(child.Aliases)
                        where name.StartsWith(find)
                        orderby name
                        select name;
            return query.ToArray();
        }
        else
        {
            var commandName = itemList[0];
            if (parentNode.TryGetCommand(commandName, out var commandNode) == true)
            {
                if (commandNode.IsEnabled == true && commandNode.Children.Any() == true)
                {
                    itemList.RemoveAt(0);
                    return GetCompletion(commandNode, itemList, find);
                }
                else
                {
                    var args = itemList.Skip(1).ToArray();
                    foreach (var item in commandNode.Commands)
                    {
                        if (GetCompletion(item, args, find) is string[] completions)
                        {
                            return completions;
                        }
                    }
                }
            }

            return [];
        }
    }

    private static string[] GetCompletion(ICommand command, string[] args, string find)
    {
        if (command is ICommandCompleter completer)
        {
            var memberDescriptors = CommandDescriptor.GetMemberDescriptors(command);
            var context = CommandCompletionContext.Create(command, memberDescriptors, args, find);
            if (context is CommandCompletionContext completionContext)
            {
                return completer.GetCompletions(completionContext);
            }
            else if (context is string[] completions)
            {
                return completions;
            }
        }

        return [];
    }

    private void Initialize(CommandNode commandNode, IEnumerable<ICommand> commands)
    {
        var query = from command in commands
                    let commandType = command.GetType()
                    orderby command.Name
                    orderby AttributeUtility.IsDefined<PartialCommandAttribute>(commandType) == true
                    select command;

        CollectCommands(commandNode, query);
        if (HelpCommand is ICommandHost helpCommandHost)
        {
            helpCommandHost.Node ??= new CommandNode(this);
        }

        if (VersionCommand is ICommandHost versionCommandHost)
        {
            versionCommandHost.Node ??= new CommandNode(this);
        }
    }

    private void CollectCommands(CommandNode parentNode, IEnumerable<ICommand> commands)
    {
        foreach (var item in commands)
        {
            CollectCommands(parentNode, item);
        }
    }

    private void CollectCommands(CommandNode parentNode, ICommand command)
    {
        var commandName = command.Name;
        var commandType = command.GetType();
        var isPartialCommand = AttributeUtility.IsDefined<PartialCommandAttribute>(commandType);
        if (parentNode.Children.Contains(commandName) == true && isPartialCommand != true)
        {
            var message = $"Command '{commandName}' is already registered.";
            throw new CommandDefinitionException(message);
        }

        if (parentNode.Children.Contains(commandName) != true && isPartialCommand == true)
        {
            var message = $"""
                Partial command cannot be registered because command '{commandName}' does not exist.
                """;
            throw new CommandDefinitionException(message);
        }

        if (isPartialCommand == true && command.Aliases.Length != 0)
        {
            var message = $"Partial command '{commandName}' cannot have alias.";
            throw new CommandDefinitionException(message);
        }

        if (parentNode.Children.TryGetValue(commandName, out var commandNode) != true)
        {
            commandNode = new CommandNode(this, command)
            {
                Parent = parentNode,
            };
            parentNode.Children.Add(commandNode);
            foreach (var item in command.Aliases)
            {
                parentNode.ChildByAlias.Add(new CommandAliasNode(commandNode, item));
            }
        }

        commandNode.CommandList.Add(command);
        if (command is ICommandHost commandHost)
        {
            commandHost.Node = commandNode;
        }

        if (command is ICommandHierarchy commandHierarchy)
        {
            CollectCommands(commandNode, commandHierarchy.Commands);
        }
    }

    private void ExecuteInternal(string[] args)
    {
        var argList = new List<string>(args);
        if (CommandUtility.IsEmptyArgs(args) == true)
        {
            OnEmptyExecute();
        }
        else if (Settings.ContainsHelpOption(args) == true)
        {
            OnHelpExecute(args);
        }
        else if (Settings.ContainsVersionOption(args) == true)
        {
            OnVersionExecute(args);
        }
        else if (GetCommand(_commandNode, argList) is { } command)
        {
            var invoker = new InternalCommandInvoker(command);
            invoker.Invoke([.. argList]);
        }
        else
        {
            throw new CommandLineException("Command does not exist.");
        }
    }

    private async Task ExecuteInternalAsync(
        string[] args, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        var argList = new List<string>(args);
        if (CommandUtility.IsEmptyArgs(args) == true)
        {
            OnEmptyExecute();
        }
        else if (Settings.ContainsHelpOption(args) == true)
        {
            OnHelpExecute(args);
        }
        else if (Settings.ContainsVersionOption(args) == true)
        {
            OnVersionExecute(args);
        }
        else if (GetCommand(_commandNode, argList) is { } command)
        {
            var invoker = new InternalCommandInvoker(command);
            await invoker.InvokeAsync([.. argList], cancellationToken, progress);
        }
        else
        {
            throw new CommandLineException("Command does not exist.");
        }
    }

    private sealed class InternalCommandInvoker(ICommand command)
        : CommandInvoker(command.Name, command)
    {
        protected override void OnVerify(string[] args)
        {
            // do nothing
        }
    }

    private sealed class InternalHelpCommand : HelpCommandBase
    {
    }

    private sealed class InternalVersionCommand : VersionCommandBase
    {
    }
}
