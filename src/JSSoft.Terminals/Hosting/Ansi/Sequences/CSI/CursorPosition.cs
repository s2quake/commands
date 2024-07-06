// <copyright file="CursorPosition.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

sealed class CursorPosition : CSISequenceBase
{
    public CursorPosition()
        : base('H')
    {
    }

    public override string DisplayName => "CSI Ps ; Ps H";

    protected override void OnProcess(SequenceContext context)
    {
        var view = context.View;
        var r1 = context.GetParameterAsInteger(index: 0, defaultValue: 1) - 1;
        var c1 = context.GetParameterAsInteger(index: 1, defaultValue: 1) - 1;
        var r2 = TerminalMathUtility.Clamp(r1, 0, view.Height - 1) + view.Y;
        var c2 = TerminalMathUtility.Clamp(c1, 0, view.Width - 1);
        var index = new TerminalIndex(new TerminalCoord(c2, r2), view.Width);
        context.Index = index;
        context.BeginIndex = index;
    }
}
