// <copyright file="TerminalFont.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Avalonia.Media;
using JSSoft.Terminals;

namespace JSSoft.Commands.AppUI.Controls;

internal sealed class TerminalFont : ITerminalFont
{
    private readonly FontFamily[] _fontFamilies;
    private readonly Typeface[] _typefacesN;
    private readonly Typeface[] _typefacesB;
    private readonly Typeface[] _typefacesI;
    private readonly Typeface[] _typefacesBI;
    private Dictionary<char, TerminalGlyphInfo> _glyphInfoByCharacter = new(char.MaxValue);
    private int _size;

    public TerminalFont(FontFamily[] fontFamilies)
        : this(fontFamilies, 12)
    {
    }

    public TerminalFont(FontFamily[] fontFamilies, int size)
    {
        _fontFamilies = [.. fontFamilies.Distinct()];
        _typefacesN = [.. _fontFamilies.Select(item => new Typeface(item))];
        _typefacesB =
        [
            .. _fontFamilies.Select(item => new Typeface(item, FontStyle.Normal, FontWeight.Bold))
        ];
        _typefacesI = [.. _fontFamilies.Select(item => new Typeface(item, FontStyle.Italic))];
        _typefacesBI =
        [
            .. _fontFamilies.Select(item => new Typeface(item, FontStyle.Italic, FontWeight.Bold))
        ];
        _size = size;
        Update();
    }

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

    public TerminalGlyphInfo this[char character] => _glyphInfoByCharacter[character];

    public Typeface GetTypeface(bool isBold, bool isItalic, int group)
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
        {
            return _typefacesN[0];
        }

        if (isBold is true && isItalic is true)
        {
            return _typefacesBI[index];
        }

        if (isBold is true)
        {
            return _typefacesB[index];
        }

        if (isItalic is true)
        {
            return _typefacesI[index];
        }

        return _typefacesN[index];
    }

    public bool Contains(char character)
    {
        foreach (var item in _typefacesN)
        {
            if (item.GlyphTypeface.TryGetGlyph(character, out var glyph) is true)
            {
                return true;
            }
        }

        return false;
    }

    private static Dictionary<char, TerminalGlyphInfo> GetCharacterInfos(
        Typeface[] typefaces, int size)
    {
        var glyphInfoCount = typefaces[0].GlyphTypeface.GlyphCount;
        var glyphInfoByCharacter = new Dictionary<char, TerminalGlyphInfo>(glyphInfoCount);
        for (var i = char.MinValue; i < char.MaxValue; i++)
        {
            foreach (var item in typefaces)
            {
                var glyphTypeface = item.GlyphTypeface;
                var designEmHeight = (double)glyphTypeface.Metrics.DesignEmHeight;
                var id = glyphTypeface.GetGlyph(i);
                if (id is not 0 && glyphTypeface.TryGetGlyphMetrics(id, out var metrics) is true)
                {
                    var xAdvance = glyphTypeface.GetGlyphAdvance(id);
                    var yAdvance = (metrics.Height + metrics.YBearing) / designEmHeight * size;
                    var glyphInfo = new TerminalGlyphInfo
                    {
                        Character = i,
                        Width = (int)Math.Ceiling(metrics.Width / designEmHeight * size),
                        Height = (int)Math.Ceiling(metrics.Height / designEmHeight * size),
                        XOffset = (int)Math.Ceiling(metrics.XBearing / designEmHeight * size),
                        YOffset = (int)Math.Ceiling(metrics.YBearing / designEmHeight * size),
                        XAdvance = (int)Math.Ceiling(xAdvance / designEmHeight * size),
                        YAdvance = (int)Math.Ceiling(yAdvance),
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

    private static (int W, int H) GetSize(IGlyphTypeface glyphTypeface, int size)
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

        width = (int)Math.Ceiling(width / designEmHeight * size);
        height = (int)Math.Ceiling(height / designEmHeight * size);

        return (width, height);
    }

    private static int CalculateWidth(IGlyphTypeface glyphTypeface, int width, ushort glyph)
    {
        if (glyphTypeface.TryGetGlyphMetrics(glyph, out var metrics) is true)
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
        Trace.TraceInformation($"{Width}, {Height}, {Size}, {_fontFamilies[0].Name}");
    }
}
