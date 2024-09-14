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
        CommandMemberInfo owner, IEnumerable<CommandMemberDescriptor> memberDescriptors)
    {
        if (memberDescriptors.Count(item => item.IsVariables) > 1)
        {
            var message = $"Attribute '{nameof(CommandPropertyArrayAttribute)}' can be defined " +
                          $"in only one property.";
            throw new CommandDefinitionException(message, owner);
        }

        var query = from memberDescriptor in memberDescriptors
                    orderby memberDescriptor.DefaultValue != DBNull.Value
                    orderby memberDescriptor.IsGeneral descending
                    orderby memberDescriptor.IsVariables descending
                    orderby memberDescriptor.IsRequired && memberDescriptor.IsExplicit descending
                    orderby memberDescriptor.IsRequired descending
                    select memberDescriptor;
        var items = query.ToArray();
        _itemByMemberName = new(items.Length);
        _itemByName = new(items.Length);
        _itemByShortName = new(items.Length);
        foreach (var item in items)
        {
            if (_itemByMemberName.Contains(item.MemberName) is true)
            {
                var message = $"{nameof(CommandMemberDescriptor)} '{item.MemberName}' cannot be " +
                              $"added because it already exists.";
                throw new CommandDefinitionException(message, owner);
            }

            _itemByMemberName.Add(item.MemberName, item);
        }

        foreach (var item in items)
        {
            if (item.Name == string.Empty)
            {
                continue;
            }

            if (_itemByName.ContainsKey(item.Name) is true)
            {
                var message = $"{nameof(CommandMemberDescriptor)} '{item.Name}' cannot be added " +
                              $"because it already exists.";
                throw new CommandDefinitionException(message, owner);
            }

            _itemByName.Add(item.Name, item);
        }

        foreach (var item in items)
        {
            if (item.ShortName == char.MinValue)
            {
                continue;
            }

            if (_itemByShortName.ContainsKey(item.ShortName) is true)
            {
                var message = $"{nameof(CommandMemberDescriptor)} '{item.ShortName}' cannot be " +
                              $"added because it already exists.";
                throw new CommandDefinitionException(message, owner);
            }

            _itemByShortName.Add(item.ShortName, item);
        }

        Owner = owner;
        RequirementDescriptors = [.. Enumerable.Where(this, IsRequiredDescriptor)];
        VariablesDescriptor = memberDescriptors.SingleOrDefault(IsVariablesDescriptor);
        OptionDescriptors = [.. Enumerable.Where(this, IsOptionDescriptor)];
    }

    public int Count => _itemByMemberName.Count;

    public CommandMemberInfo Owner { get; }

    public CommandMemberDescriptor? VariablesDescriptor { get; }

    public bool HasOptions => OptionDescriptors.Length > 0;

    public CommandMemberDescriptor[] OptionDescriptors { get; }

    public bool HasRequirements => RequirementDescriptors.Length > 0;

    public CommandMemberDescriptor[] RequirementDescriptors { get; }

    public CommandMemberDescriptor this[string memberName]
    {
        get
        {
            if (_itemByMemberName.Contains(memberName) is true)
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
        if (optionName.StartsWith(Delimiter) is true && optionName.Length > 3)
        {
            var name = optionName[Delimiter.Length..];
            return _itemByName.TryGetValue(name, out var value) is true ? value : null;
        }
        else if (optionName.StartsWith(ShortDelimiter) is true && optionName.Length == 2)
        {
            var name = optionName[1];
            return _itemByShortName.TryGetValue(name, out var value) is true ? value : null;
        }
        else if (optionName.Length == 1)
        {
            var name = optionName[0];
            return _itemByShortName.TryGetValue(name, out var value) is true ? value : null;
        }
        else
        {
            return null;
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

    internal IEnumerable<CommandMemberDescriptor> Filter(
        CommandMemberInfo memberInfo, string[] memberNames)
    {
        if (memberNames.Length is 0)
        {
            foreach (var item in this)
            {
                yield return item;
            }
        }
        else
        {
            var type = Owner;
            foreach (var item in memberNames)
            {
                if (Contains(item) is false)
                {
                    var message = $"Type '{type}' does not have property '{item}'.";
                    throw new CommandDefinitionException(message, memberInfo);
                }

                yield return this[item];
            }
        }
    }

    private static bool IsRequiredDescriptor(CommandMemberDescriptor memberDescriptor)
        => memberDescriptor.IsRequired is true;

    private static bool IsOptionDescriptor(CommandMemberDescriptor memberDescriptor)
        => memberDescriptor.IsGeneral is true || memberDescriptor.IsSwitch is true;

    private static bool IsVariablesDescriptor(CommandMemberDescriptor memberDescriptor)
        => memberDescriptor.IsVariables;
}
