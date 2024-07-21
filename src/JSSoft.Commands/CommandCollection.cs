// <copyright file="CommandCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace JSSoft.Commands;

internal sealed class CommandCollection : ICommandCollection
{
    private readonly OrderedDictionary _dictionary;

    public CommandCollection()
    {
        _dictionary = [];
    }

    public CommandCollection(IEnumerable<ICommand> commands)
    {
        _dictionary = new OrderedDictionary(commands.Count());
        foreach (var command in commands)
        {
            Add(command);
        }
    }

    public static CommandCollection Empty { get; } = [];

    public int Count => _dictionary.Count;

    public ICommand this[int index] => (ICommand)_dictionary[index]!;

    public ICommand this[string name] => (ICommand)_dictionary[name]!;

    public void Add(ICommand command) => _dictionary.Add(command.Name, command);

    public bool Contains(string name) => _dictionary.Contains(name);

    public bool TryGetValue(string name, [MaybeNullWhen(false)] out ICommand value)
    {
        if (_dictionary.Contains(name) == true)
        {
            value = (ICommand)_dictionary[name]!;
            return true;
        }

        value = default!;
        return false;
    }

    public IEnumerator<ICommand> GetEnumerator()
    {
        foreach (DictionaryEntry item in _dictionary)
        {
            yield return (ICommand)item.Value!;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => _dictionary.Values.GetEnumerator();
}
