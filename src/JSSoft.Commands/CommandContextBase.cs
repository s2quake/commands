// <copyright file="CommandContextBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands;

public abstract class CommandContextBase : ICommandContext
{
    private readonly CommandNode _commandNode;
    private readonly string _fullName;
    private readonly string _filename;

    protected CommandContextBase(IEnumerable<ICommand> commands)
        : this(Assembly.GetEntryAssembly() ?? typeof(CommandParser).Assembly, commands, CommandSettings.Default)
    {
    }

    protected CommandContextBase(IEnumerable<ICommand> commands, CommandSettings settings)
        : this(Assembly.GetEntryAssembly() ?? typeof(CommandParser).Assembly, commands, settings)
    {
    }

    protected CommandContextBase(Assembly assembly, IEnumerable<ICommand> commands)
        : this(assembly, commands, CommandSettings.Default)
    {
    }

    protected CommandContextBase(Assembly assembly, IEnumerable<ICommand> commands, CommandSettings settings)
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
        : this(name, commands, CommandSettings.Default)
    {
    }

    protected CommandContextBase(string name, IEnumerable<ICommand> commands, CommandSettings settings)
    {
        ThrowUtility.ThrowIfEmpty(name, nameof(name));
        Settings = settings;
        _fullName = name;
        _filename = name;
        Name = name;
        _commandNode = new CommandNode(this);
        Initialize(_commandNode, commands);
    }

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

    public async Task ExecuteAsync(string[] args, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        await ExecuteInternalAsync(args, cancellationToken, progress);
        OnExecuted(EventArgs.Empty);
    }

    public string[] GetCompletion(string[] items, string find)
    {
        return GetCompletion(_commandNode, new List<string>(items), find);
    }

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

    public event EventHandler? Executed;

    internal static ICommand? GetCommand(ICommandNode parentNode, IList<string> argList)
    {
        if (argList.FirstOrDefault() is { } commandName)
        {
            if (parentNode.Children.ContainsKey(commandName) == true)
            {
                var commandNode = parentNode.Children[commandName];
                if (commandNode.IsEnabled == true)
                {
                    argList.RemoveAt(0);
                    if (argList.Count > 0 && commandNode.Children.Any())
                    {
                        return GetCommand(commandNode, argList);
                    }
                    return commandNode.Command;
                }
            }
            else if (parentNode.ChildByAlias.ContainsKey(commandName) == true)
            {
                var commandNode = parentNode.ChildByAlias[commandName];
                if (commandNode.IsEnabled == true)
                {
                    argList.RemoveAt(0);
                    if (argList.Count > 0 && commandNode.Children.Any())
                    {
                        return GetCommand(commandNode, argList);
                    }
                    return commandNode.Command;
                }
            }
        }
        return null;
    }

    internal void ThrowIfNotVerifyCommandName(string commandName)
    {
        if (VerifyCommandName(commandName) == false)
        {
            throw new ArgumentException($"Command name '{commandName}' is not available.", nameof(commandName));
        }
    }

    internal bool VerifyCommandName(string commandName)
    {
        if (Name == commandName)
            return true;
        if (_fullName == commandName)
            return true;
        if (_filename == commandName)
            return true;
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
                itemList.Add($"{CommandUtility.Delimiter}{Settings.HelpName}");
            if (Settings.HelpShortName != char.MinValue)
                itemList.Add($"{CommandUtility.ShortDelimiter}{Settings.HelpShortName}");
            if (ExecutionName != string.Empty)
                return $"{ExecutionName} {string.Join(" | ", itemList)}";
            return string.Join(" | ", itemList);
        }

        string GetVersionNames()
        {
            var itemList = new List<string>(2);
            if (Settings.VersionName != string.Empty)
                itemList.Add($"{CommandUtility.Delimiter}{Settings.VersionName}");
            if (Settings.VersionShortName != char.MinValue)
                itemList.Add($"{CommandUtility.ShortDelimiter}{Settings.VersionShortName}");
            if (ExecutionName != string.Empty)
                return $"{ExecutionName} {string.Join(" | ", itemList)}";
            return string.Join(" | ", itemList);
        }
    }

    protected virtual void OnHelpExecute(string[] args)
    {
        var items = args.Where(item => Settings.IsHelpArg(item) == false).ToArray();
        var helpCommand = HelpCommand;
        var invoker = new InternalCommandInvoker(helpCommand);
        invoker.Invoke(items);
    }

    protected virtual void OnVersionExecute(string[] args)
    {
        var items = args.Where(item => Settings.IsVersionArg(item) == false).ToArray();
        var versionCommand = VersionCommand;
        var invoker = new InternalCommandInvoker(versionCommand);
        invoker.Invoke(items);
    }

    protected virtual void OnExecuted(EventArgs e)
    {
        Executed?.Invoke(this, e);
    }

    protected virtual ICommand HelpCommand { get; } = new InternalHelpCommand();

    protected virtual ICommand VersionCommand { get; } = new InternalVersionCommand();

    private void Initialize(CommandNode commandNode, IEnumerable<ICommand> commands)
    {
        var query = from item in commands
                    orderby item.Name
                    orderby item.GetType().GetCustomAttribute<PartialCommandAttribute>() != null
                    select item;

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
        var partialCommandAttribute = command.GetType().GetCustomAttribute<PartialCommandAttribute>();
        if (parentNode.Children.ContainsKey(commandName) == true && partialCommandAttribute == null)
            throw new CommandDefinitionException($"Command '{commandName}' is already registered.");
        if (parentNode.Children.ContainsKey(commandName) == false && partialCommandAttribute != null)
            throw new CommandDefinitionException($"Partial command cannot be registered because command '{commandName}' does not exist.");
        if (partialCommandAttribute != null && command.Aliases.Length != 0)
            throw new CommandDefinitionException($"Partial command '{commandName}' cannot have alias");

        if (parentNode.Children.ContainsKey(commandName) == false)
        {
            var commandNode = new CommandNode(this, command)
            {
                Parent = parentNode,
            };
            parentNode.Children.Add(commandNode);
            foreach (var item in command.Aliases)
            {
                parentNode.ChildByAlias.Add(new CommandAliasNode(commandNode, item));
            }
        }
        {
            var commandNode = parentNode.Children[commandName];
            commandNode.CommandList.Add(command);
            if (command is ICommandHost commandHost)
            {
                commandHost.Node = commandNode;
            }
            if (command is ICommandHierarchy commandHierarchy)
            {
                CollectCommands(commandNode, commandHierarchy.Commands.Values);
            }
        }
    }

    private string[] GetCompletion(ICommandNode parentNode, IList<string> itemList, string find)
    {
        if (itemList.Count == 0)
        {
            var query = from item in parentNode.Children.Values
                        where item.IsEnabled
                        from name in new string[] { item.Name }.Concat(item.Aliases)
                        where name.StartsWith(find)
                        orderby name
                        select name;
            return query.ToArray();
        }
        else
        {
            var commandName = itemList.First();
            if (parentNode.Children.TryGetValue(commandName, out var commandNode) == true || parentNode.ChildByAlias.TryGetValue(commandName, out commandNode) == true)
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

    private string[] GetCompletion(ICommand command, string[] args, string find)
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

    private async Task ExecuteInternalAsync(string[] args, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
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

    #region InternalCommandInvoker

    sealed class InternalCommandInvoker(ICommand command)
        : CommandInvoker(command.Name, command)
    {
        protected override void OnValidate(string[] args)
        {
        }
    }

    #endregion

    #region InternalHelpCommand

    sealed class InternalHelpCommand : HelpCommandBase
    {
    }

    #endregion

    #region InternalVersionCommand

    sealed class InternalVersionCommand : VersionCommandBase
    {
    }

    #endregion
}
