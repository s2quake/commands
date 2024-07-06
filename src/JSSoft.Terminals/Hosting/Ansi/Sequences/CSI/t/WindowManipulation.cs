// <copyright file="WindowManipulation.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

sealed class WindowManipulation : CSISequenceBase
{
    public WindowManipulation()
        : base('t')
    {
    }

    public override string DisplayName => "CSI Ps ; Ps ; Ps t";

    protected override void OnProcess(SequenceContext context)
    {
    }
}
