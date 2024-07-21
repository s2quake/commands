// <copyright file="CommandMemberDescriptorCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using static JSSoft.Commands.CommandUtility;

namespace JSSoft.Commands;

public sealed class CommandMemberDescriptorCollection : IEnumerable<CommandMemberDescriptor>
{
    private readonly OrderedDictionary _itemByMemberName;
    private readonly Dictionary<string, CommandMemberDescriptor> _itemByName;
    private readonly Dictionary<char, CommandMemberDescriptor> _itemByShortName;

    public CommandMemberDescriptorCollection(
        Type type, IEnumerable<CommandMemberDescriptor> memberDescriptors)
    {
        if (memberDescriptors.Count(item => item.IsVariables) > 1)
        {
            var message = $"""
                Attribute '{nameof(CommandPropertyArrayAttribute)}' can be defined in only one 
                property.
                """;
            throw new CommandDefinitionException(message, type);
        }

        var query = from item in memberDescriptors
                    orderby item.DefaultValue != DBNull.Value
                    orderby item.IsGeneral descending
                    orderby item.IsVariables descending
                    orderby item.IsRequired && item.IsExplicit descending
                    orderby item.IsRequired descending
                    select item;
        var items = query.ToArray();
        _itemByMemberName = new(items.Length);
        _itemByName = new(items.Length);
        _itemByShortName = new(items.Length);
        foreach (var item in items)
        {
            if (_itemByMemberName.Contains(item.MemberName) == true)
            {
                var message = $"""
                    {nameof(CommandMemberDescriptor)} '{item.MemberName}' cannot be added because 
                    it already exists.
                    """;
                throw new CommandDefinitionException(message, type);
            }

            _itemByMemberName.Add(item.MemberName, item);
        }

        foreach (var item in items)
        {
            if (item.Name == string.Empty)
            {
                continue;
            }

            if (_itemByName.ContainsKey(item.Name) == true)
            {
                var message = $"""
                    {nameof(CommandMemberDescriptor)} '{item.Name}' cannot be added because it 
                    already exists.
                    """;
                throw new CommandDefinitionException(message, type);
            }

            _itemByName.Add(item.Name, item);
        }

        foreach (var item in items)
        {
            if (item.ShortName == char.MinValue)
            {
                continue;
            }

            if (_itemByShortName.ContainsKey(item.ShortName) == true)
            {
                var message = $"""
                    {nameof(CommandMemberDescriptor)} '{item.ShortName}' cannot be added because it 
                    already exists.
                    """;
                throw new CommandDefinitionException(message, type);
            }

            _itemByShortName.Add(item.ShortName, item);
        }

        Type = type;
        RequirementDescriptors = [.. Enumerable.Where(this, IsRequiredDescriptor)];
        VariablesDescriptor = memberDescriptors.SingleOrDefault(IsVariableDescriptor);
        OptionDescriptors = [.. Enumerable.Where(this, IsOptionDescriptor)];
    }

    public int Count => _itemByMemberName.Count;

    public Type Type { get; }

    public CommandMemberDescriptor? VariablesDescriptor { get; }

    public bool HasOptions => OptionDescriptors.Length > 0;

    public CommandMemberDescriptor[] OptionDescriptors { get; }

    public bool HasRequirements => RequirementDescriptors.Length > 0;

    public CommandMemberDescriptor[] RequirementDescriptors { get; }

    public CommandMemberDescriptor this[string memberName]
    {
        get
        {
            if (_itemByMemberName.Contains(memberName) == true)
            {
                return (CommandMemberDescriptor)_itemByMemberName[memberName]!;
            }

            throw new CommandMemberNotFoundException(memberName);
        }
    }

    public CommandMemberDescriptor this[int index]
        => (CommandMemberDescriptor)_itemByMemberName[index]!;

    public CommandMemberDescriptor? FindByOptionName(string optionName)
    {
        if (optionName.StartsWith(Delimiter) == true && optionName.Length > 3)
        {
            var name = optionName[Delimiter.Length..];
            return _itemByName.TryGetValue(name, out var value) == true ? value : null;
        }
        else if (optionName.StartsWith(ShortDelimiter) == true && optionName.Length == 2)
        {
            var name = optionName[1];
            return _itemByShortName.TryGetValue(name, out var value) == true ? value : null;
        }
        else if (optionName.Length == 1)
        {
            var name = optionName[0];
            return _itemByShortName.TryGetValue(name, out var value) == true ? value : null;
        }
        else
        {
            return _itemByName.TryGetValue(optionName, out var value) == true ? value : null;
        }
    }

    public bool Contains(string memberName) => _itemByMemberName.Contains(memberName);

    IEnumerator<CommandMemberDescriptor> IEnumerable<CommandMemberDescriptor>.GetEnumerator()
    {
        foreach (var item in _itemByMemberName.Values)
        {
            if (item is CommandMemberDescriptor memberDescriptor)
            {
                yield return memberDescriptor;
            }
            else
            {
                throw new UnreachableException();
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var item in _itemByMemberName.Values)
        {
            yield return item;
        }
    }

    internal IEnumerable<CommandMemberDescriptor> Filter(Type requestType, string[] memberNames)
    {
        if (memberNames.Length == 0)
        {
            foreach (var item in this)
            {
                yield return item;
            }
        }
        else
        {
            var type = Type;
            foreach (var item in memberNames)
            {
                if (Contains(item) != true)
                {
                    var message = $"Type '{type}' does not have property '{item}'.";
                    throw new CommandDefinitionException(message, requestType);
                }

                yield return this[item];
            }
        }
    }

    private static bool IsRequiredDescriptor(CommandMemberDescriptor memberDescriptor)
        => memberDescriptor.IsRequired == true;

    private static bool IsOptionDescriptor(CommandMemberDescriptor memberDescriptor)
        => memberDescriptor.IsGeneral == true || memberDescriptor.IsSwitch == true;

    private static bool IsVariableDescriptor(CommandMemberDescriptor memberDescriptor)
        => memberDescriptor.IsVariables;
}
