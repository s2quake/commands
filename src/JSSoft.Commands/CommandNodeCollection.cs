// <copyright file="CommandNodeCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace JSSoft.Commands;

sealed class CommandNodeCollection : Dictionary<string, CommandNode>, IReadOnlyDictionary<string, ICommandNode>
{
    public CommandNodeCollection()
        : base(StringComparer.CurrentCulture)
    {
    }

    public void Add(CommandNode node)
    {
        Add(node.Name, node);
    }

    #region IReadOnlyDictionary

    ICommandNode IReadOnlyDictionary<string, ICommandNode>.this[string key] => this[key];

    IEnumerable<string> IReadOnlyDictionary<string, ICommandNode>.Keys => Keys;

    IEnumerable<ICommandNode> IReadOnlyDictionary<string, ICommandNode>.Values => Values;

    bool IReadOnlyDictionary<string, ICommandNode>.TryGetValue(string key, [MaybeNullWhen(false)] out ICommandNode value)
    {
        if (TryGetValue(key, out var v) == true)
        {
            value = v;
            return true;
        }
        value = default!;
        return false;
    }

    #endregion

    #region IEnumerable

    IEnumerator<KeyValuePair<string, ICommandNode>> IEnumerable<KeyValuePair<string, ICommandNode>>.GetEnumerator()
    {
        foreach (var item in this)
        {
            yield return new KeyValuePair<string, ICommandNode>(item.Key, item.Value);
        }
    }

    #endregion
}
