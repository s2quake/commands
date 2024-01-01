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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Avalonia.Media;
using JSSoft.Terminals;

namespace JSSoft.Commands.AppUI.Controls;

sealed class TerminalFont : ITerminalFont
{
    private readonly FontFamily[] _fontFamilies;
    private Dictionary<char, TerminalGlyphInfo> _glyphInfoByCharacter = new(char.MaxValue);
    private int _size;
    private readonly Typeface[] _typefacesN;
    private readonly Typeface[] _typefacesB;

    public TerminalFont(FontFamily fontFamily)
        : this(fontFamily, 12)
    {
    }

    public TerminalFont(FontFamily fontFamily, int size)
    {
        _fontFamilies = new FontFamily[] { fontFamily, FontFamily.Parse(FontManager.Current.DefaultFontFamily.Name) }.Distinct().ToArray();
        _typefacesN = [.. _fontFamilies.Select(item => new Typeface(item))];
        _typefacesB = [.. _fontFamilies.Select(item => new Typeface(item, FontStyle.Normal, FontWeight.UltraBold))];
        _size = size;
        Update();
    }

    public TerminalGlyphInfo this[char character] => _glyphInfoByCharacter[character];

    public int Width { get; private set; }

    public int Height { get; private set; }

    public int Size
    {
        get => _size;
        set
        {
            if (_size != value)
            {
                _size = value;
                Update();
            }
        }
    }

    public Typeface GetTypeface(bool isBold, int group)
    {
        var index = -1;
        for (var i = 0; i < _typefacesN.Length; i++)
        {
            if (_typefacesN[i].GetHashCode() == group)
            {
                index = i;
                break;
            }
        }
        if (index == -1)
            return _typefacesN[0];
        if (isBold == true)
            return _typefacesB[index];
        return _typefacesN[index];
    }

    public bool Contains(char character)
    {
        foreach (var item in _typefacesN)
        {
            if (item.GlyphTypeface.TryGetGlyph(character, out var glyph) == true)
                return true;
        }
        return false;
    }

    private static Dictionary<char, TerminalGlyphInfo> GetCharacterInfos(Typeface[] typefaces, int size)
    {
        var designEmHeight1 = (double)typefaces[0].GlyphTypeface.Metrics.DesignEmHeight;
        var glyphInfoByCharacter = new Dictionary<char, TerminalGlyphInfo>(typefaces[0].GlyphTypeface.GlyphCount);
        for (var i = char.MinValue; i < char.MaxValue; i++)
        {
            foreach (var item in typefaces)
            {
                var glyphTypeface = item.GlyphTypeface;
                var designEmHeight = (double)glyphTypeface.Metrics.DesignEmHeight;
                var id = glyphTypeface.GetGlyph(i);
                if (id != 0 && glyphTypeface.TryGetGlyphMetrics(id, out var metrics) == true)
                {
                    var xAdvance = glyphTypeface.GetGlyphAdvance(id);
                    var glyphInfo = new TerminalGlyphInfo
                    {
                        Character = i,
                        Width = (int)Math.Ceiling(metrics.Width / designEmHeight * size),
                        Height = (int)Math.Ceiling(metrics.Height / designEmHeight * size),
                        XOffset = (int)Math.Ceiling(metrics.XBearing / designEmHeight * size),
                        YOffset = (int)Math.Ceiling(metrics.YBearing / designEmHeight * size),
                        XAdvance = (int)Math.Ceiling(xAdvance / designEmHeight * size),
                        YAdvance = (int)Math.Ceiling((metrics.Height + metrics.YBearing) / designEmHeight * size),
                        Tag = id,
                        Group = item.GetHashCode(),
                    };
                    glyphInfoByCharacter.Add(i, glyphInfo);
                    break;
                }
            }
        }
        return glyphInfoByCharacter;
    }

    private static (int, int) GetSize(IGlyphTypeface glyphTypeface, int size)
    {
        var width = 0;
        var height = glyphTypeface.Metrics.LineSpacing;
        var designEmHeight = (double)glyphTypeface.Metrics.DesignEmHeight;
        for (ushort i = 48; i <= 57; i++)
        {
            width = CalculateWidth(glyphTypeface, width, i);
        }
        for (ushort i = 65; i <= 90; i++)
        {
            width = CalculateWidth(glyphTypeface, width, i);
        }
        for (ushort i = 97; i <= 122; i++)
        {
            width = CalculateWidth(glyphTypeface, width, i);
        }
        return ((int)Math.Ceiling(width / designEmHeight * size), (int)Math.Ceiling(height / designEmHeight * size));
    }

    private static int CalculateWidth(IGlyphTypeface glyphTypeface, int width, ushort glyph)
    {
        if (glyphTypeface.TryGetGlyphMetrics(glyph, out var metrics) == true)
        {
            var xAdvance = glyphTypeface.GetGlyphAdvance(glyph);
            width = Math.Max(width, xAdvance);
        }

        return width;
    }

    private void Update()
    {
        (Width, Height) = GetSize(_typefacesN[0].GlyphTypeface, _size);
        _glyphInfoByCharacter = GetCharacterInfos(_typefacesN, _size);
        Trace.WriteLine($"{Width}, {Height}, {Size}, {_fontFamilies[0].Name}");
    }
}
