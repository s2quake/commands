// <copyright file="SetTitleMode.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

internal sealed class SetTitleMode : CSISequenceBase
{
    public SetTitleMode()
        : base('t')
    {
    }

    public override string DisplayName => "CSI > Pm t";

    public override string Prefix => ">";

    protected override void OnProcess(SequenceContext context)
    {
    }
}
