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

using System.Collections;

namespace JSSoft.Terminals.Hosting.Ansi;

sealed class SequenceCollection : IEnumerable<ISequence>
{
    private static readonly Comparer KeyComparer = new();

    private readonly Dictionary<char, SortedDictionary<SequenceKey, ISequence>> _sequencesByCharacter = [];

    public void Add(ISequence item)
    {
        if (_sequencesByCharacter.ContainsKey(item.Character) == false)
        {
            _sequencesByCharacter.Add(item.Character, new SortedDictionary<SequenceKey, ISequence>(KeyComparer));
        }
        _sequencesByCharacter[item.Character].Add(new(item.Prefix, item.Suffix), item);
    }

    public bool Contains(char character)
        => _sequencesByCharacter.ContainsKey(character) == true && _sequencesByCharacter.Count > 0;

    public ISequence this[char character, string etc]
    {
        get
        {
            var sequences = _sequencesByCharacter[character];
            foreach (var item in sequences.Keys)
            {
                if (item.Equals(etc) == true)
                {
                    return sequences[item];
                }
            }
            throw new NotImplementedException();
        }
    }

    #region Comparer

    sealed class Comparer : IComparer<SequenceKey>
    {
        public int Compare(SequenceKey x, SequenceKey y)
        {
            var r = StringComparer.Ordinal.Compare(y.Prefix, x.Prefix);
            if (r == 0)
            {
                return StringComparer.Ordinal.Compare(y.Suffix, x.Suffix);
            }
            return r;
        }
    }

    #endregion

    #region IEnumerable

    IEnumerator<ISequence> IEnumerable<ISequence>.GetEnumerator()
    {
        foreach (var values in _sequencesByCharacter.Values)
        {
            foreach (var value in values.Values)
            {
                yield return value;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var values in _sequencesByCharacter.Values)
        {
            foreach (var value in values.Values)
            {
                yield return value;
            }
        }
    }

    #endregion
}