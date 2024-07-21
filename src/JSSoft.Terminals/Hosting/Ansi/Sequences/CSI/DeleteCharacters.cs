// <copyright file="DeleteCharacters.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

internal sealed class DeleteCharacters : CSISequenceBase
{
    public DeleteCharacters()
        : base('P')
    {
    }

    public override string DisplayName => "CSI Ps P";
    protected override void OnProcess(SequenceContext context)
    {
        var lines = context.Lines;
        var index = context.Index;
        var line = lines[index.Y];
        var length = context.GetParameterAsInteger(index: 0, defaultValue: 1);
        line.Delete(index.X, length);
    }
}
