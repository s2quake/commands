// <copyright file="ChangeAttributesInRectangularArea.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

sealed class ChangeAttributesInRectangularArea : CSISequenceBase
{
    public ChangeAttributesInRectangularArea()
        : base('r')
    {
    }

    public override string DisplayName => "CSI Pt ; Pl ; Pb ; Pr ; Pm $ r";

    public override string Suffix => "$";

    protected override void OnProcess(SequenceContext context)
    {
    }
}
