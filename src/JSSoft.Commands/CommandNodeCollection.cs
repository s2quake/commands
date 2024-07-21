// <copyright file="CommandNodeCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace JSSoft.Commands;

internal sealed class CommandNodeCollection : ICommandNodeCollection
{
    private readonly OrderedDictionary _dictionary = [];

    public int Count => _dictionary.Count;

    public CommandNode this[int index] => (CommandNode)_dictionary[index]!;

    public CommandNode this[string name] => (CommandNode)_dictionary[name]!;

    ICommandNode ICommandNodeCollection.this[int index] => this[index];

    ICommandNode ICommandNodeCollection.this[string name] => this[name];

    public bool Contains(string name) => _dictionary.Contains(name);

    public void Add(CommandNode node) => _dictionary.Add(node.Name, node);

    public bool TryGetValue(string name, [MaybeNullWhen(false)] out CommandNode value)
    {
        if (_dictionary.Contains(name) == true)
        {
            value = (CommandNode)_dictionary[name]!;
            return true;
        }

        value = default!;
        return false;
    }

    public IEnumerator<ICommandNode> GetEnumerator()
    {
        foreach (DictionaryEntry item in _dictionary)
        {
            yield return (ICommandNode)item.Value!;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => _dictionary.Values.GetEnumerator();

    bool ICommandNodeCollection.TryGetValue(
        string name, [MaybeNullWhen(false)] out ICommandNode value)
    {
        if (TryGetValue(name, out var v) == true)
        {
            value = v;
            return true;
        }

        value = default!;
        return false;
    }
}
