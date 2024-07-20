// <copyright file="DECPrivateModeReset.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

// DECRST
internal sealed class DECPrivateModeReset : CSISequenceBase
{
    public DECPrivateModeReset()
        : base('h')
    {
    }

    public override string Prefix => "?";

    public override string DisplayName => "CSI ? Pm h";

    protected override void OnProcess(SequenceContext context)
    {
        var value = context.GetParameterAsInteger(index: 0);
        switch (value)
        {
            case 1:
                context.Modes[TerminalMode.DECCKM] = false;
                break;
            default:
                Console.WriteLine($"Mode '{value}' is not supported.");
                break;
        }
    }
}
