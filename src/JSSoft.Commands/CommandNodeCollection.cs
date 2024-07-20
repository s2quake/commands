// <copyright file="CommandNodeCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using KeyValuePair = System.Collections.Generic.KeyValuePair<string, JSSoft.Commands.ICommandNode>;

namespace JSSoft.Commands;

internal sealed class CommandNodeCollection
    : Dictionary<string, CommandNode>, IReadOnlyDictionary<string, ICommandNode>
{
    public CommandNodeCollection()
        : base(StringComparer.CurrentCulture)
    {
    }

    IEnumerable<string> IReadOnlyDictionary<string, ICommandNode>.Keys => Keys;

    IEnumerable<ICommandNode> IReadOnlyDictionary<string, ICommandNode>.Values => Values;

    ICommandNode IReadOnlyDictionary<string, ICommandNode>.this[string key] => this[key];

    public void Add(CommandNode node) => Add(node.Name, node);

    bool IReadOnlyDictionary<string, ICommandNode>.TryGetValue(
        string key, [MaybeNullWhen(false)] out ICommandNode value)
    {
        if (TryGetValue(key, out var v) == true)
        {
            value = v;
            return true;
        }

        value = default!;
        return false;
    }

    IEnumerator<KeyValuePair> IEnumerable<KeyValuePair>.GetEnumerator()
    {
        foreach (var item in this)
        {
            yield return new KeyValuePair(item.Key, item.Value);
        }
    }
}
