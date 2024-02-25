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
using System.Diagnostics.CodeAnalysis;

namespace JSSoft.Terminals.Hosting.Ansi;

sealed class SequenceCollection(string sequenceString) : IEnumerable<ISequence>
{
    private readonly Dictionary<char, SortedSet<ISequence>> _sequencesByCharacter = [];

    public string SequenceString { get; } = sequenceString;

    public override string ToString()
        => SequenceString;

    public void Add(ISequence item)
    {
        if (_sequencesByCharacter.ContainsKey(item.Character) == false)
        {
            _sequencesByCharacter.Add(item.Character, []);
        }
        _sequencesByCharacter[item.Character].Add(item);
    }

    public bool TryGetValue(string text, [MaybeNullWhen(false)] out ISequence value, out string parameter, out int endIndex)
    {
        if (text.StartsWith(SequenceString) == true)
        {
            return TryGetSequence(text, out value, out parameter, out endIndex);
        }
        value = default;
        parameter = string.Empty;
        endIndex = 0;
        return false;

    }

    private bool TryGetSequence(string text, [MaybeNullWhen(false)] out ISequence value, out string parameter, out int endIndex)
    {
        for (var i = SequenceString.Length; i < text.Length; i++)
        {
            var character = text[i];
            if (_sequencesByCharacter.TryGetValue(character, out var sequences) == true)
            {
                var range = new Range(SequenceString.Length, i);
                foreach (var sequence in sequences)
                {
                    if (sequence.Match(text, range, out var actualRange) == true)
                    {
                        value = sequence;
                        parameter = text[actualRange];
                        endIndex = Math.Max(actualRange.End.Value, i + 1);
                        return true;
                    }
                }
            }
        }
        value = default;
        parameter = string.Empty;
        endIndex = default;
        return false;
    }

    #region IEnumerable

    IEnumerator<ISequence> IEnumerable<ISequence>.GetEnumerator()
    {
        foreach (var values in _sequencesByCharacter.Values)
        {
            foreach (var value in values)
            {
                yield return value;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var values in _sequencesByCharacter.Values)
        {
            foreach (var value in values)
            {
                yield return value;
            }
        }
    }

    #endregion
}