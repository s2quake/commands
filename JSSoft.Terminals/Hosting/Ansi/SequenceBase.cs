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

namespace JSSoft.Terminals.Hosting.Ansi;

abstract class SequenceBase(SequenceType type, char character)
    : ISequence
{
    public SequenceType Type { get; } = type;

    public virtual string Prefix => string.Empty;

    public virtual string Suffix => string.Empty;

    public char Character { get; } = character;

    public virtual string DisplayName => string.Empty;

    public override string? ToString() => DisplayName == string.Empty ? base.ToString() : DisplayName;

    protected abstract void OnProcess(SequenceContext context);

    protected virtual bool Match(string text, Range parameterRange, out Range actualParameterRange)
    {
        var parameter = text[parameterRange];
        if (Prefix != string.Empty && parameter.StartsWith(Prefix) != true)
        {
            actualParameterRange = parameterRange;
            return false;
        }
        if (Suffix != string.Empty && parameter.EndsWith(Suffix) != true)
        {
            actualParameterRange = parameterRange;
            return false;
        }

        actualParameterRange = new Range(parameterRange.Start.Value + Prefix.Length, parameterRange.End.Value - Suffix.Length);
        return true;
    }

    #region ISequence

    void ISequence.Process(SequenceContext context)
        => OnProcess(context);

    bool ISequence.Match(string text, Range parameterRange, out Range actualParameterRange)
        => Match(text, parameterRange, out actualParameterRange);

    #endregion

    #region IComparable

    int IComparable<ISequence>.CompareTo(ISequence? other)
    {
        if (other is SequenceBase sequence)
        {
            if (Type != sequence.Type)
                return Type.CompareTo(sequence.Type);
            if (Character != sequence.Character)
                return Character.CompareTo(sequence.Character);
            if (sequence.Prefix != Prefix)
                return StringComparer.Ordinal.Compare(sequence.Prefix, Prefix);
            return StringComparer.Ordinal.Compare(sequence.Suffix, Suffix);
        }
        else if (other is not null)
        {
            return Character.CompareTo(other.Character);
        }
        return 1;
    }

    #endregion

    #region IEquatable 

    bool IEquatable<ISequence>.Equals(ISequence? other)
    {
        if (other is SequenceBase sequence)
        {
            return Type == sequence.Type && Prefix == sequence.Prefix && Suffix == sequence.Suffix;
        }

        return false;
    }

    #endregion
}
