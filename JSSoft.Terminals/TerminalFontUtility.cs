// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

namespace JSSoft.Terminals;

public static class TerminalFontUtility
{
    public const int DefaultItemWidth = 14;
    public const int DefaultItemHeight = 27;

    public static TerminalGlyphInfo? GetGlyph(ITerminalFont font, char character)
    {
        if (font.Contains(character) == true)
            return font[character];
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