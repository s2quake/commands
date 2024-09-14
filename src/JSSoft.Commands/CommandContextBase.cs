// <copyright file="CommandContextBase.cs" company="JSSoft">
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
        : this(GetDefaultAssembly(), commands, settings: CommandSettings.Default)
    {
    }

    protected CommandContextBase(IEnumerable<ICommand> commands, CommandSettings settings)
        : this(GetDefaultAssembly(), commands, settings)
    {
    }

    protected CommandContextBase(Assembly assembly, IEnumerable<ICommand> commands)
        : this(assembly, commands, settings: CommandSettings.Default)
    {
    }

    protected CommandContextBase(
        Assembly assembly, IEnumerable<ICommand> commands, CommandSettings settings)
    {
        Settings = settings;
        _fullName = assembly.GetAssemblyLocation();
        _filename = Path.GetFileName(_fullName);
        Name = Path.GetFileNameWithoutExtension(_fullName);
        Version = assembly.GetAssemblyVersion();
        Copyright = assembly.GetAssemblyCopyright();
        _commandNode = new CommandNode(this);
        Initialize(_commandNode, commands);
    }

    protected CommandContextBase(string name, IEnumerable<ICommand> commands)
        : this(name, commands, settings: CommandSettings.Default)
    {
    }

    protected CommandContextBase(
        string name, IEnumerable<ICommand> commands, CommandSettings settings)
    {
        if (name == string.Empty)
        {
            throw new ArgumentException("Empty string is not allowed.", nameof(name));
        }

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

    public ICommand Node => _commandNode;

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

    internal static ICommand? GetCommand(ICommand parent, IList<string> argList)
    {
        if (argList.FirstOrDefault() is { } commandName
            && parent.TryGetCommand(commandName, out var command) is true)
        {
            argList.RemoveAt(0);
            if (argList.Count > 0 && command.Commands.Count is not 0)
            {
                return GetCommand(command, argList);
            }

            return command;
        }

        return null;
    }

    internal void ThrowIfNotVerifyCommandName(string commandName)
    {
        if (CheckCommandName(commandName) is false)
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
        var items = args.Where(item => Settings.IsHelpArg(item) is false).ToArray();
        var helpCommand = HelpCommand;
        var invoker = new InternalCommandInvoker(helpCommand);
        invoker.Invoke(items);
    }

    protected virtual void OnVersionExecute(string[] args)
    {
        var items = args.Where(item => Settings.IsVersionArg(item) is false).ToArray();
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

    private static void AttachContext(ICommand command, ICommandContext commandContext)
    {
        if (command.Context is not null)
        {
            throw new CommandDefinitionException(
                message: "The command context is already attached.",
                memberInfo: command.GetType());
        }

        var parent = command.Parent;
        if (parent?.Commands.Contains(command) is false)
        {
            throw new CommandDefinitionException(
                message: $"Command '{command}' does not contain the commands of parent '{parent}'.",
                memberInfo: command.GetType());
        }

        command.Context = commandContext;
        foreach (var item in command.Commands)
        {
            AttachContext(item, commandContext);
        }
    }

    private string[] GetCompletion(
        ICommand parent, IList<string> itemList, string find)
    {
        if (itemList.Count is 0)
        {
            var query = from child in parent.Commands
                        where child.IsEnabled is true
                        from name in new string[] { child.Name }.Concat(child.Aliases)
                        where name.StartsWith(find)
                        orderby name
                        select name;
            return query.ToArray();
        }
        else
        {
            var commandName = itemList[0];
            if (parent.TryGetCommand(commandName, out var command) is true)
            {
                if (command.IsEnabled is true && command.Commands.Any() is true)
                {
                    itemList.RemoveAt(0);
                    return GetCompletion(command, itemList, find);
                }
                else
                {
                    var args = itemList.Skip(1).ToArray();
                    if (GetCompletion(command, args, find) is string[] completions)
                    {
                        return completions;
                    }
                }
            }

            return [];
        }
    }

    private string[] GetCompletion(ICommand command, string[] args, string find)
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(command);
        var settings = Settings;
        var parseContext = new ParseContext(memberDescriptors, settings, args);
        var context = CommandCompletionContext.Create(command, parseContext, find);
        if (context is CommandCompletionContext completionContext)
        {
            return command.GetCompletions(completionContext);
        }
        else if (context is string[] completions)
        {
            return completions;
        }

        return [];
    }

    private void Initialize(CommandNode commandNode, IEnumerable<ICommand> commands)
    {
        var query = from command in commands.Concat([HelpCommand, VersionCommand]).Distinct()
                    where command.Parent is null
                    orderby command.Name
                    select command;

        foreach (var command in query)
        {
            command.SetParent(commandNode);
        }

        foreach (var command in commandNode.Commands)
        {
            AttachContext(command, this);
        }
    }

    private void ExecuteInternal(string[] args)
    {
        var argList = new List<string>(args);
        if (CommandUtility.IsEmptyArgs(args) is true)
        {
            OnEmptyExecute();
        }
        else if (Settings.ContainsHelpOption(args) is true)
        {
            OnHelpExecute(args);
        }
        else if (Settings.ContainsVersionOption(args) is true)
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
        if (CommandUtility.IsEmptyArgs(args) is true)
        {
            OnEmptyExecute();
        }
        else if (Settings.ContainsHelpOption(args) is true)
        {
            OnHelpExecute(args);
        }
        else if (Settings.ContainsVersionOption(args) is true)
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
