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

class SequenceCollection(string sequenceString) : ISequenceCollection
{
    private readonly Dictionary<char, SortedSet<ISequence>> _sequencesByCharacter = [];

    public string SequenceString { get; } = sequenceString;

    public override string ToString() => SequenceString;

    public void Add(ISequence item)
    {
        if (_sequencesByCharacter.ContainsKey(item.Character) == false)
        {
            _sequencesByCharacter.Add(item.Character, []);
        }
        _sequencesByCharacter[item.Character].Add(item);
    }

    public (ISequence value, string parameter, int endIndex) GetValue(string text)
    {
        if (text.StartsWith(SequenceString) != true)
            throw new ArgumentException($"text does not start with: '{SequenceString}'", nameof(text));

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
                        var parameter = text[actualRange];
                        var endIndex = Math.Max(actualRange.End.Value, i + 1);
                        return (sequence, parameter, endIndex);
                    }
                }
                throw new NotFoundSequenceException(text[0..i]);
            }
            else if (Test(character) != true)
            {
                throw new NotFoundSequenceException(text[0..i]);
            }
        }
        throw new NotSupportedException();
    }

    protected virtual bool Test(char character) => character switch
    {
        '\a' => false,
        '\b' => false,
        '\t' => false,
        '\n' => false,
        '\v' => false,
        '\f' => false,
        '\r' => false,
        '\x1b' => false,
        _ => true,
    };

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
