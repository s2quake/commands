// <copyright file="ParseDescriptorCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections;
using System.Collections.Specialized;

namespace JSSoft.Commands;

public sealed class ParseDescriptorCollection : IEnumerable<ParseDescriptor>
{
    private readonly OrderedDictionary _parseDescriptorByMember;

    internal ParseDescriptorCollection(CommandMemberDescriptorCollection memberDescriptors)
    {
        _parseDescriptorByMember = new(memberDescriptors.Count);
        foreach (var item in memberDescriptors)
        {
            _parseDescriptorByMember.Add(item, new ParseDescriptor(item));
        }
    }

    public int Count => _parseDescriptorByMember.Count;

    public ParseDescriptor this[CommandMemberDescriptor memberDescriptor] => (ParseDescriptor)_parseDescriptorByMember[memberDescriptor]!;

    internal Queue<ParseDescriptor> CreateQueue()
    {
        var query = from ParseDescriptor item in _parseDescriptorByMember.Values
                    where item.IsRequired == true && item.IsExplicit == false && item.HasValue == false
                    select item;
        return new(query);
    }

    #region ParseDescriptorCollection

    IEnumerator<ParseDescriptor> IEnumerable<ParseDescriptor>.GetEnumerator()
    {
        foreach (var item in _parseDescriptorByMember.Values)
        {
            yield return (ParseDescriptor)item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var item in _parseDescriptorByMember.Values)
        {
            yield return item;
        }
    }

    #endregion
}
