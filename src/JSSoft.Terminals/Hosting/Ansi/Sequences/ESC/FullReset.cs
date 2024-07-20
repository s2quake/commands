// <copyright file="FullReset.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.ESC;

internal sealed class FullReset : ESCSequenceBase
{
    public FullReset()
        : base('c')
    {
    }

    public override string DisplayName => "ESC c";

    protected override void OnProcess(SequenceContext context)
    {
        var lines = context.Lines;
        lines.Take(context.Index);
        context.OriginCoordinate = new (context.Index.X, context.Index.Y);
    }
}
