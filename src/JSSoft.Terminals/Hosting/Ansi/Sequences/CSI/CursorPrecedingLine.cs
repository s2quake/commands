// <copyright file="CursorPrecedingLine.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

internal sealed class CursorPrecedingLine : CSISequenceBase
{
    public CursorPrecedingLine()
        : base('F')
    {
    }

    public override string DisplayName => "CSI Ps F";

    protected override void OnProcess(SequenceContext context)
    {
        var index = context.Index;
        var value = context.GetParameterAsInteger(index: 0, defaultValue: 1);
        var count = Math.Max(1, value);
        context.Index = index.CursorRight(count);
    }
}
