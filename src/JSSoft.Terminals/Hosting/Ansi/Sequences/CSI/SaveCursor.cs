// <copyright file="SaveCursor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

internal sealed class SaveCursor : CSISequenceBase
{
    public SaveCursor()
        : base('s')
    {
    }

    public override string DisplayName => "CSI s";

    protected override void OnProcess(SequenceContext context)
    {
        var view = context.View;
        var index = context.Index;
        var value = context.GetParameterAsInteger(index: 0, defaultValue: 1);
        var count = Math.Max(1, value);
        context.ViewCoordinate = (TerminalCoord)(index - index.Width * view.Y);
    }
}
