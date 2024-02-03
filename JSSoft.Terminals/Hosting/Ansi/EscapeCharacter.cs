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

using System.Diagnostics;
using JSSoft.Terminals.Hosting.Ansi.CSI;
using JSSoft.Terminals.Hosting.Ansi.EraseFunctions;

namespace JSSoft.Terminals.Hosting.Ansi;

sealed class EscapeCharacter : IAsciiCode
{
    private static readonly Dictionary<SequenceType, Dictionary<char, ISequence>> SequencesByType = new()
    {
        { SequenceType.ESC, new Dictionary<char, ISequence>() },
        { SequenceType.CSI, new Dictionary<char, ISequence>() },
        { SequenceType.DCS, new Dictionary<char, ISequence>() },
        { SequenceType.OSC, new Dictionary<char, ISequence>() },
    };
    private static readonly Dictionary<char, ISequence> CSISequenceByCharacter = SequencesByType[SequenceType.CSI];
    private static readonly Dictionary<char, ISequence> ESCSequenceByCharacter = SequencesByType[SequenceType.ESC];

    static EscapeCharacter()
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
            SequencesByType[obj.Type].Add(obj.Character, obj);
        }
    }

    public void Process(TerminalLineCollection lines, AsciiCodeContext context)
    {
        var c = context.Text[context.TextIndex + 1];
        if (c == '[')
        {
            var s1 = context.TextIndex + 2;
            for (var i = s1; i < context.Text.Length; i++)
            {
                var character = context.Text[i];
                if (CSISequenceByCharacter.ContainsKey(character) == true)
                {
                    var escapeSequence = CSISequenceByCharacter[character];
                    var option = context.Text.Substring(s1, i - s1);
                    var escapeSequenceContext = new EscapeSequenceContext(option, context);
                    Console.WriteLine($"ESC [ {option}{character}");
                    escapeSequence.Process(lines, escapeSequenceContext);
                    context.TextIndex = i + 1;
                    return;
                }
            }
        }
        else
        {
            var s1 = context.TextIndex + 1;
            for (var i = s1; i < context.Text.Length; i++)
            {
                var character = context.Text[i];
                if (ESCSequenceByCharacter.ContainsKey(character) == true)
                {
                    var escapeSequence = ESCSequenceByCharacter[character];
                    var option = context.Text.Substring(s1, i - s1);
                    var escapeSequenceContext = new EscapeSequenceContext(option, context);
                    Console.WriteLine($"ESC {option}{character}");
                    escapeSequence.Process(lines, escapeSequenceContext);
                    context.TextIndex = i + 1;
                    return;
                }
                if (character == '\x1b')
                {
                    throw new NotImplementedException();
                }
            }
        }
        context.TextIndex = context.Text.Length;
    }
}
