// <copyright file="CommandCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace JSSoft.Commands;

public sealed class CommandCollection : IEnumerable<ICommand>
{
    private readonly Dictionary<string, ICommand> _commandByName = [];
    private readonly List<ICommand> _commandList = [];

    public CommandCollection()
    {
    }

    public CommandCollection(IEnumerable<ICommand> commands)
    {
        var capacity = commands.Count() + commands.SelectMany(item => item.Aliases).Count();
        _commandByName = new(capacity);
        foreach (var command in commands)
        {
            Add(command);
        }

        _commandList = new List<ICommand>(commands);
    }

    public static CommandCollection Empty { get; } = new() { IsLocked = true };

    public int Count => _commandList.Count;

    internal bool IsLocked { get; set; }

    public ICommand this[int index] => _commandList[index];

    public ICommand this[string name] => _commandByName[name];

    public void Add(ICommand command)
    {
        if (IsLocked is true)
        {
            throw new InvalidOperationException("Collection is locked.");
        }

        if (command.AllowsSubCommands is false
            && command is not IExecutable
            && command is not IAsyncExecutable)
        {
            throw new CommandDefinitionException(
                $"Command '{command.Name}' must implement IExecutable or " +
                $"IAsyncExecutable if it does not allow subcommands.");
        }

        if (CommandUtility.IsName(command.Name) is false)
        {
            throw new CommandDefinitionException("Invalid command name.");
        }

        _commandByName.Add(command.Name, command);
        foreach (var alias in command.Aliases)
        {
            _commandByName.Add(alias, command);
        }

        _commandList.Add(command);
    }

    public void Remove(ICommand command)
    {
        if (IsLocked is true)
        {
            throw new InvalidOperationException("Collection is locked.");
        }

        _commandByName.Remove(command.Name);
        foreach (var alias in command.Aliases)
        {
            _commandByName.Remove(alias);
        }

        _commandList.Remove(command);
    }

    public bool Contains(string name) => _commandByName.ContainsKey(name);

    public bool Contains(ICommand command) => _commandList.Contains(command);

    public bool TryGetValue(string name, [MaybeNullWhen(false)] out ICommand value)
        => _commandByName.TryGetValue(name, out value);

    public IEnumerator<ICommand> GetEnumerator() => _commandList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _commandList.GetEnumerator();
}
