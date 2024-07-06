// <copyright file="NormalKeypad.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.ESC;

sealed class NormalKeypad : ESCSequenceBase
{
    public NormalKeypad()
        : base('>')
    {
    }

    public override string DisplayName => "ESC >";

    protected override void OnProcess(SequenceContext context)
    {
    }
}
