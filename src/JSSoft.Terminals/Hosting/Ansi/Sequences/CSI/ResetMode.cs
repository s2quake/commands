// <copyright file="ResetMode.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

internal sealed class ResetMode : CSISequenceBase
{
    public ResetMode()
        : base('l')
    {
    }

    public override string DisplayName => "CSI Pm l";

    protected override void OnProcess(SequenceContext context)
    {
        // Ps = 2  ⇒  Keyboard Action Mode (KAM).
        // Ps = 4  ⇒  Replace Mode (IRM).
        // Ps = 1 2  ⇒  Send/receive (SRM).
        // Ps = 2 0  ⇒  Normal Linefeed (LNM).
        var option = context.Parameter;
    }
}
