// <copyright file="ITerminalFontExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Extensions;

public static class ITerminalFontExtensions
{
    public static int GetGlyphSpan(this ITerminalFont font, char character)
    {
        if (font.Contains(character) == true)
        {
            var characterInfo = font[character];
            var defaultWidth = font.Width;
            var horizontalAdvance = characterInfo.XAdvance;
            var span = (int)Math.Ceiling((float)horizontalAdvance / defaultWidth);
            return Math.Max(span, 1);
        }
        return 1;
    }
}
