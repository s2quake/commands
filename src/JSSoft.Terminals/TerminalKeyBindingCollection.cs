// <copyright file="TerminalKeyBindingCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections;

namespace JSSoft.Terminals;

public class TerminalKeyBindingCollection : ITerminalKeyBindingCollection
{
    private readonly Dictionary<string, ITerminalKeyBinding> _keyBindingByName = [];

    public TerminalKeyBindingCollection(ITerminalKeyBindingCollection parent)
    {
        Parent = parent;
    }

    public TerminalKeyBindingCollection()
    {
    }

    public void Add(ITerminalKeyBinding item)
    {
        var name = $"{item.Modifiers}+{item.Key}";
        _keyBindingByName.Add(name, item);
    }

    public bool Process(object obj, TerminalModifiers modifiers, TerminalKey key)
    {
        var name = $"{modifiers}+{key}";
        if (_keyBindingByName.ContainsKey(name) == true)
        {
            var binding = _keyBindingByName[name];
            if (binding.CanInvoke(obj) == true)
            {
                binding.Invoke(obj);
                return true;
            }
        }

        if (Parent?.Process(obj, modifiers, key) == true)
        {
            return true;
        }

        return false;
    }

    public int Count => _keyBindingByName.Count;

    public ITerminalKeyBindingCollection? Parent { get; }

    IEnumerator<ITerminalKeyBinding> IEnumerable<ITerminalKeyBinding>.GetEnumerator()
    {
        foreach (var item in _keyBindingByName)
        {
            yield return item.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var item in _keyBindingByName)
        {
            yield return item.Value;
        }
    }
}
