// <copyright file="RestoreDECPrivateModeValues.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

internal sealed class RestoreDECPrivateModeValues : CSISequenceBase
{
    public RestoreDECPrivateModeValues()
        : base('r')
    {
    }

    public override string DisplayName => "CSI ? Pm r";

    public override string Prefix => "?";

    protected override void OnProcess(SequenceContext context)
    {
    }
}
