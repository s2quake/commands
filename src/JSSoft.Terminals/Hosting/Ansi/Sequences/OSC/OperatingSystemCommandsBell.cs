// <copyright file="OperatingSystemCommandsBell.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.OSC;

/// <summary>
/// OSC Ps ; Pt BEL
/// </summary>
internal sealed class OperatingSystemCommandsBell : OperatingSystemCommandsBase
{
    public OperatingSystemCommandsBell()
        : base('\a')
    {
    }

    public override string DisplayName => "OSC Ps ; Pt BEL";

    protected override void OnProcess(SequenceContext context)
    {
        var ps = context.GetParameterAsInteger(index: 0);
        var pt = context.GetParameterAsString(index: 1);
        switch (ps)
        {
            case 0:
                {
                    context.Title = pt;
                }

                break;
        }
    }
}
