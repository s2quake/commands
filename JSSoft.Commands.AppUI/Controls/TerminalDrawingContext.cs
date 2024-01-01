using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using JSSoft.Terminals;
using JSSoft.Terminals.Renderers;

namespace JSSoft.Commands.AppUI.Controls;

sealed class TerminalDrawingContext(DrawingContext context) : ITerminalDrawingContext
{
    private static readonly Dictionary<TerminalColor, Pen> _penByColor = [];
    private static readonly Dictionary<TerminalGlyphRun, GlyphRun> _runByOrigin = [];
    private readonly DrawingContext _context = context;

    public void DrawRectangle(TerminalColor color, int thickness, TerminalRect rect)
    {
        var pen = GetPen(color);
        pen.Thickness = thickness;
        _context.DrawRectangle(pen, TerminalMarshal.Convert(rect));
    }

    public void DrawText(TerminalGlyphRun glyphRun)
    {
        if (_runByOrigin.ContainsKey(glyphRun) == false)
        {
            _runByOrigin.Add(glyphRun, Create(glyphRun));
            glyphRun.Disposed += (s, e) => _runByOrigin.Remove(glyphRun);
        }
        using var _ = _context.PushTransform(Matrix.CreateTranslation(glyphRun.Position.X, glyphRun.Position.Y));
        _context.DrawGlyphRun(TerminalMarshal.ToBrush(glyphRun.Color), _runByOrigin[glyphRun]);
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

    public static Pen GetPen(TerminalColor color)
    {
        if (_penByColor.ContainsKey(color) == false)
        {
            _penByColor.Add(color, new Pen(TerminalMarshal.ToBrush(color)));
        }
        return _penByColor[color];
    }

    private static GlyphRun Create(TerminalGlyphRun obj)
    {
        if (obj.Font is not TerminalFont font)
            throw new ArgumentException($"Font '{obj.Font}' is unavailable.", nameof(obj));

        var typeface = font.GetTypeface(obj.IsBold, obj.Group);
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
