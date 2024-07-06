// <copyright file="DCSSequence.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>


namespace JSSoft.Terminals.Hosting.Ansi.Sequences.DCS;

sealed class DCSSequence : SequenceBase
{
    public DCSSequence()
        : base(SequenceType.DCS, '\\')
    {
    }

    public override string Suffix => "\x1b";

    protected override bool Match(string text, Range parameterRange, out Range actualParameterRange)
    {
        return base.Match(text, parameterRange, out actualParameterRange);
    }

    protected override void OnProcess(SequenceContext context)
    {

    }
}
