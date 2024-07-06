// <copyright file="DesignateG0CharacterSet.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.ESC;

sealed class DesignateG0CharacterSet : ESCSequenceBase
{
    public DesignateG0CharacterSet()
        : base('(')
    {
    }

    public override string DisplayName => "ESC ( C";

    protected override void OnProcess(SequenceContext context)
    {
        // var rest = context.Text.Substring(context.TextIndex + 2);
        // if (rest[0] == 'B')
        // {
        //     context.TextIndex++;
        // }
    }

    protected override bool Match(string text, Range parameterRange, out Range actualParameterRange)
    {
        var s = parameterRange.Start.Value + 1;
        if (text[s] == 'B')
        {
            actualParameterRange = new Range(s, s + 1);
            return true;
        }
        return base.Match(text, parameterRange, out actualParameterRange);
    }
}
