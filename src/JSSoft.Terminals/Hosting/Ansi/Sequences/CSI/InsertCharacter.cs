// <copyright file="InsertCharacter.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

sealed class InsertCharacter : CSISequenceBase
{
    private const char SpaceCharacter = ' ';

    public InsertCharacter()
        : base('@')
    {
    }

    public override string DisplayName => "CSI Ps @\nCSI Ps SP @";

    protected override void OnProcess(SequenceContext context)
    {
        var lines = context.Lines;
        var index1 = context.Index;
        var beginIndex = context.BeginIndex;
        var font = context.Font;
        var span = TerminalFontUtility.GetGlyphSpan(font, SpaceCharacter);
        var characterInfo = new TerminalCharacterInfo
        {
            Character = SpaceCharacter,
            DisplayInfo = context.DisplayInfo,
            Span = span,
        };
        var index2 = index1.Expect(span);
        var line = lines.Prepare(beginIndex, index2);
        line.InsertCharacter(index2.X, characterInfo);
    }
}
