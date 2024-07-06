// <copyright file="SoftTerminalReset.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

sealed class SoftTerminalReset : CSISequenceBase
{
    public SoftTerminalReset()
        : base('p')
    {
    }

    public override string DisplayName => "CSI ! p";

    public override string Prefix => "!";

    protected override void OnProcess(SequenceContext context)
    {
    }
}
