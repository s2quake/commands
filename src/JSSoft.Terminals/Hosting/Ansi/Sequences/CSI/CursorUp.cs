// <copyright file="CursorUp.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

internal sealed class CursorUp : CSISequenceBase
{
    public CursorUp()
        : base('A')
    {
    }

    public override string DisplayName => "CSI Ps A";

    protected override void OnProcess(SequenceContext context)
    {
        var view = context.View;
        var index1 = context.Index;
        var value = context.GetParameterAsInteger(index: 0, defaultValue: 1);
        var count = Math.Max(1, value);
        var index2 = index1.CursorUp(count, view.Top);
        context.Index = index2;
        context.BeginIndex = index2;
    }
}
