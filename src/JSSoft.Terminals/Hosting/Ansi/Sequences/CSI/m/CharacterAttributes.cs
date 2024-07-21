// <copyright file="CharacterAttributes.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

internal sealed class CharacterAttributes : CSISequenceBase
{
    public CharacterAttributes()
        : base('m')
    {
    }

    public override string DisplayName => "CSI Pm m";

    protected override void OnProcess(SequenceContext context)
    {
        var displayInfo = context.DisplayInfo;
        var codes = context.Parameters.Select(SequenceContext.Parse).ToArray();
        if (codes.Length > 0)
        {
            displayInfo.SetGraphicRendition(codes);
        }
        else
        {
            displayInfo.Reset();
        }

        context.DisplayInfo = displayInfo;
    }
}
