// <copyright file="SetMode.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

sealed class SetMode : CSISequenceBase
{
    public SetMode()
        : base('h')
    {
    }

    public override string DisplayName => "CSI Pm h";

    protected override void OnProcess(SequenceContext context)
    {
        throw new NotImplementedException();
    }
}
