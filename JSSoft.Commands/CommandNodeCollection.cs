// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

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
        base.Add(node.Name, node);
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
        value = null;
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
