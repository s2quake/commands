// <copyright file="SequenceCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections;

namespace JSSoft.Terminals.Hosting.Ansi;

internal class SequenceCollection(string sequenceString) : ISequenceCollection
{
    private readonly Dictionary<char, SortedSet<ISequence>> _sequencesByCharacter = [];

    public string SequenceString { get; } = sequenceString;

    public override string ToString() => SequenceString;

    public void Add(ISequence item)
    {
        if (_sequencesByCharacter.ContainsKey(item.Character) != true)
        {
            _sequencesByCharacter.Add(item.Character, []);
        }

        _sequencesByCharacter[item.Character].Add(item);
    }

    public (ISequence value, string parameter, int endIndex) GetValue(string text)
    {
        if (text.StartsWith(SequenceString) != true)
        {
            throw new ArgumentException($"text does not start with: '{SequenceString}'", nameof(text));
        }

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
}
