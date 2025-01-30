// <copyright file="TerminalDrawingContext.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using JSSoft.Terminals;
using JSSoft.Terminals.Renderers;

namespace JSSoft.Commands.AppUI.Controls;

internal sealed class TerminalDrawingContext(DrawingContext context) : ITerminalDrawingContext
{
    private static readonly Dictionary<TerminalColor, Pen> _penByColor = [];
    private static readonly Dictionary<TerminalGlyphRun, GlyphRun> _runByOrigin = [];
    private readonly DrawingContext _context = context;

    public static Pen GetPen(TerminalColor color)
    {
        if (_penByColor.TryGetValue(color, out var pen) is false)
        {
            pen = new Pen(TerminalMarshal.ToBrush(color));
            _penByColor.Add(color, pen);
        }

        return pen;
    }

    public void DrawRectangle(TerminalColor color, int thickness, TerminalRect rect)
    {
        var pen = GetPen(color);
        pen.Thickness = thickness;
        _context.DrawRectangle(pen, TerminalMarshal.Convert(rect));
    }

    public void DrawText(TerminalGlyphRun glyphRun)
    {
        if (_runByOrigin.TryGetValue(glyphRun, out var run) is false)
        {
            run = Create(glyphRun);
            _runByOrigin.Add(glyphRun, run);
            glyphRun.Disposed += (s, e) => _runByOrigin.Remove(glyphRun);
        }

        var transform = Matrix.CreateTranslation(glyphRun.Position.X, glyphRun.Position.Y);
        using var scope = _context.PushTransform(transform);
        _context.DrawGlyphRun(TerminalMarshal.ToBrush(glyphRun.Color), run);
    }

    public void FillRectangle(TerminalColor color, TerminalRect rect)
    {
        _context.FillRectangle(TerminalMarshal.ToBrush(color), TerminalMarshal.Convert(rect));
    }

    public IDisposable PushTransform(TerminalPoint position)
    {
        var translation = Matrix.CreateTranslation(position.X, position.Y);
        return _context.PushTransform(translation);
    }

    private static GlyphRun Create(TerminalGlyphRun obj)
    {
        if (obj.Font is not TerminalFont font)
        {
            throw new ArgumentException($"Font '{obj.Font}' is unavailable.", nameof(obj));
        }

        var typeface = font.GetTypeface(obj.IsBold, obj.IsItalic, obj.Group);
        var fontSize = font.Size;
        var characters = obj.GlyphInfos.Select(item => item.Character).ToArray();
        var glyphInfoList = new List<GlyphInfo>(obj.GlyphInfos.Length);
        for (var i = 0; i < obj.GlyphInfos.Length; i++)
        {
            var span = obj.Spans[i];
            glyphInfoList.Add(new GlyphInfo((ushort)obj.GlyphInfos[i].Tag, i, font.Width * span));
        }

        return new GlyphRun(typeface.GlyphTypeface, fontSize, characters, [.. glyphInfoList]);
    }
}
