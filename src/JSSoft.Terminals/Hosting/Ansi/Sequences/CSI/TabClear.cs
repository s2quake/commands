// <copyright file="TabClear.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

/// <summary>
/// CSI Ps g
/// </summary>
internal sealed class TabClear : CSISequenceBase
{
    public TabClear()
        : base('g')
    {
    }

    public override string DisplayName => "CSI Ps g";

    protected override void OnProcess(SequenceContext context)
    {
        var option = context.GetParameterAsInteger(index: 0);
        // Ps = 0  ⇒  Clear Current Column (default).
        // Ps = 3  ⇒  Clear All.
    }
}
