// <copyright file="CommandMethodDescriptorCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections.Specialized;
using System.Diagnostics;

namespace JSSoft.Commands;

public sealed class CommandMethodDescriptorCollection : IEnumerable<CommandMethodDescriptor>
{
    private readonly OrderedDictionary _methodDescriptorByMethodName = [];
    private readonly Dictionary<string, CommandMethodDescriptor> _methodDescriptorByName;

    internal CommandMethodDescriptorCollection(Type type, IEnumerable<CommandMethodDescriptor> methodDescriptors)
    {
        var query = from methodDescriptor in methodDescriptors
                    from alias in methodDescriptor.Aliases
                    select (alias, methodDescriptor);
        var aliases = query.ToArray();
        _methodDescriptorByMethodName = new(methodDescriptors.Count());
        _methodDescriptorByName = new(aliases.Length + methodDescriptors.Count());
        foreach (var item in methodDescriptors)
        {
            if (_methodDescriptorByMethodName.Contains(item.MethodName) == true)
                throw new CommandDefinitionException($"{nameof(CommandMethodDescriptor)} '{item.MethodName}' cannot be added because it already exists.", type);
            _methodDescriptorByMethodName.Add(item.MethodName, item);
            _methodDescriptorByName.Add(item.Name, item);
        }
        foreach (var (alias, methodDescriptor) in aliases)
        {
            if (_methodDescriptorByMethodName.Contains(alias) == true)
                throw new CommandDefinitionException($"{nameof(CommandMethodDescriptor)} '{alias}' cannot be added because it already exists.", type);
            if (_methodDescriptorByName.ContainsKey(alias) == true)
                throw new CommandDefinitionException($"{nameof(CommandMethodDescriptor)} '{alias}' cannot be added because it already exists.", type);
            _methodDescriptorByName.Add(alias, methodDescriptor);
        }
        Type = type;
    }

    public CommandMethodDescriptor? FindByName(string name)
    {
        return _methodDescriptorByName.TryGetValue(name, out var value) == true ? value : null;
    }

    public bool Contains(string methodName) => _methodDescriptorByMethodName.Contains(methodName);

    public bool TryGetValue(string methodName, out CommandMethodDescriptor? value)
    {
        if (_methodDescriptorByMethodName.Contains(methodName) == true)
        {
            value = (CommandMethodDescriptor)_methodDescriptorByMethodName[methodName]!;
            return true;
        }
        value = null;
        return false;
    }

    public CommandMethodDescriptor this[string methodName]
    {
        get
        {
            if (_methodDescriptorByMethodName.Contains(methodName) == true)
            {
                return (CommandMethodDescriptor)_methodDescriptorByMethodName[methodName]!;
            }
            throw new CommandMethodNotFoundException(methodName);
        }
    }

    public CommandMethodDescriptor this[int index] => (CommandMethodDescriptor)_methodDescriptorByMethodName[index]!;

    public int Count => _methodDescriptorByMethodName.Count;

    public Type Type { get; }

    internal IEnumerable<CommandMethodDescriptor> Filter(Type requestType, string[] methodNames)
    {
        if (methodNames.Length == 0)
        {
            foreach (var item in this)
            {
                yield return item;
            }
        }
        else
        {
            var type = Type;
            foreach (var item in methodNames)
            {
                if (Contains(item) == false)
                    throw new CommandDefinitionException($"Type '{type}' does not have method '{item}'.", requestType);
                yield return this[item];
            }
        }
    }

    #region IEnumerable

    IEnumerator<CommandMethodDescriptor> IEnumerable<CommandMethodDescriptor>.GetEnumerator()
    {
        foreach (var item in _methodDescriptorByMethodName.Values)
        {
            if (item is CommandMethodDescriptor methodDescriptor)
            {
                yield return methodDescriptor;
            }
            else
            {
                throw new UnreachableException();
            }
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        foreach (var item in _methodDescriptorByMethodName.Values)
        {
            yield return item;
        }
    }

    #endregion
}
