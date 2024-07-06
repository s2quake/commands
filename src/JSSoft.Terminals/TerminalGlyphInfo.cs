// <copyright file="TerminalGlyphInfo.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public struct TerminalGlyphInfo
{
    public char Character { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int XOffset { get; set; }

    public int YOffset { get; set; }

    public int XAdvance { get; set; }

    public int YAdvance { get; set; }

    public int Tag { get; set; }

    public int Group { get; set; }

    public static TerminalGlyphInfo Empty { get; } = new TerminalGlyphInfo();
}
