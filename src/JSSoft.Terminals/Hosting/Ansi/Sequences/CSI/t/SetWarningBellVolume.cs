// <copyright file="SetWarningBellVolume.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

internal sealed class SetWarningBellVolume : CSISequenceBase
{
    public SetWarningBellVolume()
        : base('t')
    {
    }

    public override string DisplayName => "CSI Ps SP t";

    public override string Suffix => " ";

    protected override void OnProcess(SequenceContext context)
    {
    }
}
