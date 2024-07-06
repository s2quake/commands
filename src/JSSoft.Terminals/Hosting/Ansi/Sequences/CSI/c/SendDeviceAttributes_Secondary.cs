// <copyright file="SendDeviceAttributes_Secondary.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

sealed class SendDeviceAttributes_Secondary : CSISequenceBase
{
    public SendDeviceAttributes_Secondary()
        : base('c')
    {
    }

    public override string DisplayName => "CSI > Ps c";

    public override string Prefix => ">";

    protected override void OnProcess(SequenceContext context)
    {
    }
}
