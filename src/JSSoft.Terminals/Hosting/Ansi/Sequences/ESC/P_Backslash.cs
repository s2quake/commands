// <copyright file="P_Backslash.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.ESC;

internal sealed class P_Backslash : ESCSequenceBase
{
    public P_Backslash()
        : base('\\')
    {
    }

    public override string Prefix => "P";

    protected override void OnProcess(SequenceContext context)
    {
    }
}
