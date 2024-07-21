// <copyright file="CursorCharacterAbsolute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

internal sealed class CursorCharacterAbsolute : CSISequenceBase
{
    public CursorCharacterAbsolute()
        : base('G')
    {
    }

    public override string DisplayName => "CSI Ps G";

    protected override void OnProcess(SequenceContext context)
    {
        var view = context.View;
        var index = context.Index;
        var c1 = context.GetParameterAsInteger(index: 0, defaultValue: 1) - 1;
        var c2 = TerminalMathUtility.Clamp(c1, 0, view.Width - 1);
        context.Index = index.CursorToColumn(c2);
    }
}
