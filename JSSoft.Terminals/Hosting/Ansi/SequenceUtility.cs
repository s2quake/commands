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

using System.Text.RegularExpressions;
using JSSoft.Terminals.Hosting.Ansi.Sequences.ESC;

namespace JSSoft.Terminals.Hosting.Ansi;

static class SequenceUtility
{
    private static readonly Dictionary<SequenceType, SequenceCollection> SequencesByType = new()
    {
        { SequenceType.ESC, new SequenceCollection() },
        { SequenceType.CSI, new SequenceCollection() },
        { SequenceType.DCS, new SequenceCollection() },
        { SequenceType.OSC, new SequenceCollection() },
    };
    private static readonly SequenceCollection CSISequenceByCharacter = SequencesByType[SequenceType.CSI];
    private static readonly SequenceCollection ESCSequenceByCharacter = SequencesByType[SequenceType.ESC];
    private static readonly SequenceCollection OSCSequenceByCharacter = SequencesByType[SequenceType.OSC];

    static SequenceUtility()
    {
        var types = typeof(EscapeCharacter).Assembly.GetTypes();
        var query = from type in types
                    where typeof(ISequence).IsAssignableFrom(type) == true
                    where type.IsAbstract != true
                    select type;
        var items = query.ToArray();
        foreach (var item in items)
        {
            var obj = (ISequence)Activator.CreateInstance(item)!;
            SequencesByType[obj.Type].Add(obj);
        }
    }

    public static void Process(TerminalLineCollection lines, AsciiCodeContext context)
    {
        var c = context.Text[context.TextIndex + 1];
        if (c == '[')
        {
            var s1 = context.TextIndex + 2;
            for (var i = s1; i < context.Text.Length; i++)
            {
                var character = context.Text[i];
                if (CSISequenceByCharacter.Contains(character) == true)
                {
                    var etc = context.Text[s1..i];
                    var sequence = CSISequenceByCharacter[character, etc];
                    var option = etc.Substring(sequence.Prefix.Length, etc.Length - (sequence.Prefix.Length + sequence.Suffix.Length));
                    var sequenceContext = new SequenceContext(option, context);
                    Console.WriteLine($"ESC [ {string.Join(" ", [etc, GetDisplayName(character)])} => {sequence.GetType()}");
                    sequence.Process(lines, sequenceContext);
                    context.TextIndex = i + 1;
                    return;
                }
                if (character == '\x1b')
                {
                    var l = Math.Min(10, context.Text.Length - context.TextIndex);
                    var s = context.Text.Substring(context.TextIndex, l);
                    throw new NotSupportedException(Regex.Escape(s));
                }
            }
        }
        else if (c == ']')
        {
            var s1 = context.TextIndex + 2;
            for (var i = s1; i < context.Text.Length; i++)
            {
                var character = context.Text[i];
                if (OSCSequenceByCharacter.Contains(character) == true)
                {
                    var etc = context.Text.Substring(s1, i - s1);
                    var sequence = OSCSequenceByCharacter[character, etc];
                    var option = etc.Substring(sequence.Prefix.Length, etc.Length - (sequence.Prefix.Length + sequence.Suffix.Length));
                    var sequenceContext = new SequenceContext(option, context);
                    Console.WriteLine($"ESC ] {string.Join(" ", [etc, GetDisplayName(character)])} => {sequence.GetType()}");
                    sequence.Process(lines, sequenceContext);
                    context.TextIndex = i + 1;
                    return;
                }
                if (character == '\x1b')
                {
                    var l = Math.Min(10, context.Text.Length - context.TextIndex);
                    var s = context.Text.Substring(context.TextIndex, l);
                    throw new NotSupportedException(Regex.Escape(s));
                }
            }
        }
        else
        {
            var s1 = context.TextIndex + 1;
            for (var i = s1; i < context.Text.Length; i++)
            {
                var character = context.Text[i];
                if (ESCSequenceByCharacter.Contains(character) == true)
                {
                    var etc = context.Text.Substring(s1, i - s1);
                    var sequence = ESCSequenceByCharacter[character, etc];
                    var option = etc.Substring(sequence.Prefix.Length, etc.Length - (sequence.Prefix.Length + sequence.Suffix.Length));
                    var sequenceContext = new SequenceContext(option, context);
                    Console.WriteLine($"ESC {string.Join(" ", [etc, GetDisplayName(character)])} => {sequence.GetType()}");
                    sequence.Process(lines, sequenceContext);
                    context.TextIndex = i + 1;
                    return;
                }
                // if (character == '\x1b')
                // {
                //     var l = Math.Min(10, context.Text.Length - context.TextIndex);
                //     var s = context.Text.Substring(context.TextIndex, l);
                //     throw new NotSupportedException(Regex.Escape(s));
                // }
            }
        }
        context.TextIndex = context.Text.Length;
    }

    public static string GetDisplayName(char character) => character switch
    {
        '\a' => "BEL",
        '\b' => "BS",
        '\t' => "HT",
        '\n' => "LF",
        '\v' => "VT",
        '\f' => "FF",
        '\r' => "CR",
        '\x1b' => "ESC",
        _ => $"{character}",
    };
}
