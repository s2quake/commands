// <copyright file="DeviceStatusReport.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

sealed class DeviceStatusReport : CSISequenceBase
{
    public DeviceStatusReport()
        : base('n')
    {
    }

    public override string DisplayName => "CSI Ps n";

    protected override void OnProcess(SequenceContext context)
    {
        var lines = context.Lines;
        var ps = context.GetParameterAsInteger(0);
        switch (ps)
        {
            case 5:
                context.SendSequence("\x1b[0n");
                break;
            case 6:
                var cursor = context.GetCoordinate(lines, context.Index);
                cursor.Y = Math.Min(cursor.Y - context.View.Y, context.View.Height - 1);
                context.SendSequence($"\x1b[{cursor.Y + 1};{cursor.X + 1}R");
                break;
        }
    }
}
