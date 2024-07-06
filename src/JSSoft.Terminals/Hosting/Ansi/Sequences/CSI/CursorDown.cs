// <copyright file="CursorDown.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

sealed class CursorDown : CSISequenceBase
{
    public CursorDown()
        : base('B')
    {
    }

    public override string DisplayName => "CSI Ps B";

    protected override void OnProcess(SequenceContext context)
    {
        var view = context.View;
        var index = context.Index;
        var value = context.GetParameterAsInteger(index: 0, defaultValue: 1);
        var count = Math.Max(1, value);
        index = index.CursorDown(count, view.Bottom);
        context.Index = index;
        context.BeginIndex = index;
    }
}
