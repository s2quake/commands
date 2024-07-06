// <copyright file="TerminalGlyphRunInfo.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Renderers;

public record TerminalGlyphRunInfo
{
    public TerminalGlyphRunInfo(ITerminalFont font)
    {
        Font = font;
    }

    public ITerminalFont Font { get; }

    public TerminalColor Color { get; set; }

    public bool IsBold { get; set; }

    public bool IsItalic { get; set; }

    public int Group { get; set; }

    public TerminalGlyphInfo[] GlyphInfos { get; set; } = [];

    public int[] Spans { get; set; } = [];

    public TerminalPoint Position { get; set; }
}
