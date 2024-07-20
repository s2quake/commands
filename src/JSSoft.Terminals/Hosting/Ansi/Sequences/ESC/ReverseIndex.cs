// <copyright file="ReverseIndex.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.ESC;

internal sealed class ReverseIndex : ESCSequenceBase
{
    public ReverseIndex()
        : base('M')
    {
    }

    public override string DisplayName => "ESC M";

    protected override void OnProcess(SequenceContext context)
    {
        var lines = context.Lines;
        lines.ReverseLineFeed(context.Index.Y);
    }
}
