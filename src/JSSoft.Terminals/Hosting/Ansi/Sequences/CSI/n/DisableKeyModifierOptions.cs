// <copyright file="DisableKeyModifierOptions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

sealed class DisableKeyModifierOptions : CSISequenceBase
{
    public DisableKeyModifierOptions()
        : base('n')
    {
    }

    public override string DisplayName => "CSI > Ps n";

    public override string Prefix => ">";

    protected override void OnProcess(SequenceContext context)
    {
    }
}
