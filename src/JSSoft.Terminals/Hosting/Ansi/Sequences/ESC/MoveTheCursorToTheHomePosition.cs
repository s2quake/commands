// <copyright file="MoveTheCursorToTheHomePosition.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.ESC;

internal sealed class MoveTheCursorToTheHomePosition : ESCSequenceBase
{
    public MoveTheCursorToTheHomePosition()
        : base('H')
    {
    }

    public override string DisplayName => "ESC H";

    protected override void OnProcess(SequenceContext context)
    {
        var view = context.View;
        var x = 0;
        var y = 0 + view.Y;
        var index = new TerminalIndex(new TerminalCoord(x, y), view.Width);
        context.Index = index;
        context.BeginIndex = index;
    }
}
