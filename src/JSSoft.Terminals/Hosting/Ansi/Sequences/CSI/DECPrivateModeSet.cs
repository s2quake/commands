// <copyright file="DECPrivateModeSet.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

// DECSET
internal sealed class DECPrivateModeSet : CSISequenceBase
{
    public DECPrivateModeSet()
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
                context.Modes[TerminalMode.DECCKM] = true;
                break;
            default:
                Console.WriteLine($"Mode '{value}' is not supported.");
                break;
        }
    }
}
