// <copyright file="SetResetKeyModifierOptions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

internal sealed class SetResetKeyModifierOptions : CSISequenceBase
{
    public SetResetKeyModifierOptions()
        : base('m')
    {
    }

    public override string DisplayName => "CSI > Pp ; Pv m\nCSI > Pp m";

    public override string Prefix => ">";

    protected override void OnProcess(SequenceContext context)
    {
    }
}
