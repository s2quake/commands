// <copyright file="ParseDescriptorCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace JSSoft.Commands;

public sealed class ParseDescriptorCollection : IEnumerable<ParseDescriptor>
{
    private readonly OrderedDictionary _itemByMember;
    private readonly Dictionary<string, ParseDescriptor> _itemByMemberName;

    internal ParseDescriptorCollection(CommandMemberDescriptorCollection memberDescriptors)
    {
        _itemByMember = new(memberDescriptors.Count);
        _itemByMemberName = new(memberDescriptors.Count);
        foreach (var item in memberDescriptors)
        {
            var parseDescriptor = new ParseDescriptor(item);
            _itemByMember.Add(item, parseDescriptor);
            _itemByMemberName.Add(item.MemberName, parseDescriptor);
        }
    }

    public int Count => _itemByMember.Count;

    public ParseDescriptor this[CommandMemberDescriptor memberDescriptor]
        => (ParseDescriptor)_itemByMember[memberDescriptor]!;

    public ParseDescriptor this[int index]
        => (ParseDescriptor)_itemByMember[index]!;

    public ParseDescriptor this[string memberName]
        => _itemByMemberName[memberName]!;

    public bool Contains(CommandMemberDescriptor memberDescriptor)
        => _itemByMember.Contains(memberDescriptor);

    public bool Contains(string memberName)
        => _itemByMemberName.ContainsKey(memberName);

    public bool TryGetValue(
        CommandMemberDescriptor memberDescriptor, [MaybeNullWhen(false)] out ParseDescriptor value)
    {
        if (_itemByMember.Contains(memberDescriptor) is true)
        {
            value = (ParseDescriptor)_itemByMember[memberDescriptor]!;
            return true;
        }

        value = default!;
        return false;
    }

    public bool TryGetValue(string memberName, [MaybeNullWhen(false)] out ParseDescriptor value)
        => _itemByMemberName.TryGetValue(memberName, out value);

    IEnumerator<ParseDescriptor> IEnumerable<ParseDescriptor>.GetEnumerator()
        => _itemByMember.Values.OfType<ParseDescriptor>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _itemByMember.Values.GetEnumerator();

    internal Queue<ParseDescriptor> CreateQueue()
    {
        var query = from ParseDescriptor item in _itemByMember.Values
                    where item.IsRequired is true
                    where item.IsExplicit is false
                    where item.HasValue is false
                    select item;
        return new(query);
    }
}
