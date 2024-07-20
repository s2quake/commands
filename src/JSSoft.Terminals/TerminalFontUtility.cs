// <copyright file="TerminalFontUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public static class TerminalFontUtility
{
    public const int DefaultItemWidth = 14;
    public const int DefaultItemHeight = 27;

    public static TerminalGlyphInfo? GetGlyph(ITerminalFont font, char character)
    {
        if (font.Contains(character) == true)
        {
            return font[character];
        }

        return null;
    }

    public static int GetGlyphSpan(ITerminalFont font, char character)
    {
        if (GetGlyph(font, character) is { } characterInfo)
        {
            var defaultWidth = font.Width;
            var horizontalAdvance = characterInfo.XAdvance;
            var span = (int)Math.Ceiling((float)horizontalAdvance / defaultWidth);
            return Math.Max(span, 1);
        }

        return 1;
    }

    public static TerminalRect GetForegroundRect(ITerminalFont font, char character)
    {
        return GetForegroundRect(font, character, 0, 0);
    }

    public static TerminalRect GetForegroundRect(ITerminalFont font, char character, int x, int y)
    {
        if (font.Contains(character) == true)
        {
            var glyphInfo = font[character];
            var fx = x + glyphInfo.XOffset;
            var fy = y + glyphInfo.YOffset;
            return new TerminalRect(fx, fy, glyphInfo.Width, glyphInfo.Height);
        }

        return new TerminalRect(x + 1, y + 1, font.Width - 2, font.Height - 2);
    }
}
