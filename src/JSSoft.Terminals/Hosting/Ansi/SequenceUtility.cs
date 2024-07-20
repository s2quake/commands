// <copyright file="SequenceUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi;

internal static class SequenceUtility
{
    private static readonly SortedDictionary<SequenceType, SequenceCollection> SequenceCollectionByType = new()
    {
        { SequenceType.ESC, new("\x001b") },
        { SequenceType.CSI, new("\x001b[") },
        { SequenceType.DCS, new DCSSequenceCollection() },
        { SequenceType.OSC, new("\x001b]") },
    };
    private static readonly SequenceCollection[] SequenceCollections
        = SequenceCollectionByType.OrderByDescending(item => item.Key).Select(item => item.Value).ToArray();

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
            SequenceCollectionByType[obj.Type].Add(obj);
        }
    }

    public static void Process(AsciiCodeContext context)
    {
        var textIndex = context.TextIndex;
        var text = context.Text[context.TextIndex..];
        var sequenceCollection = GetSequenceCollection(text);
        try
        {
            var (sequence, parameter, endIndex) = sequenceCollection.GetValue(text);
#if DEBUG && NET8_0
            Console.WriteLine($"{sequence} => {ToLiteral(text[..endIndex])}");
#endif
            sequence.Process(new SequenceContext(parameter, context));
            context.TextIndex = textIndex + endIndex;
        }
        catch (NotFoundSequenceException e)
        {
            Console.WriteLine("unknown sequence: " + e.Sequence);
            context.TextIndex += e.Sequence.Length;
        }
    }

    private static SequenceCollection GetSequenceCollection(string text)
    {
        for (var i = 0; i < SequenceCollections.Length; i++)
        {
            var sequenceCollection = SequenceCollections[i];
            if (text.StartsWith(sequenceCollection.SequenceString) == true)
            {
                return sequenceCollection;
            }
        }

        throw new NotSupportedException("not supported sequence");
    }

#if DEBUG && NET8_0
    public static string ToLiteral(string valueTextForCompiler)
    {
        return Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(valueTextForCompiler, false);
    }
#endif

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
