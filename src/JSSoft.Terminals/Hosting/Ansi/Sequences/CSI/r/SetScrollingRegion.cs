// <copyright file="SetScrollingRegion.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

internal sealed class SetScrollingRegion : CSISequenceBase
{
    public SetScrollingRegion()
        : base('r')
    {
    }

    public override string DisplayName => "CSI Ps ; Ps r";

    protected override void OnProcess(SequenceContext context)
    {
    }
}
