// <copyright file="RestoreCursor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

internal sealed class RestoreCursor : CSISequenceBase
{
    public RestoreCursor()
        : base('u')
    {
    }

    public override string DisplayName => "CSI u";

    protected override void OnProcess(SequenceContext context)
    {
        var view = context.View;
        var index = context.Index;
        var viewCoord = context.ViewCoordinate;
        var coord = viewCoord + new TerminalCoord(0, view.Y);
        context.Index = new TerminalIndex(coord, view.Width);
    }
}
