// <copyright file="TerminalGlyphRun.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Renderers;

public class TerminalGlyphRun(TerminalGlyphRunInfo info) : IDisposable
{
    private bool _isDisposed;

    public ITerminalFont Font { get; } = info.Font;

    public TerminalColor Color { get; } = info.Color;

    public bool IsBold { get; } = info.IsBold;

    public bool IsItalic { get; } = info.IsItalic;

    public int Group { get; } = info.Group;

    public TerminalGlyphInfo[] GlyphInfos { get; } = info.GlyphInfos;

    public int[] Spans { get; } = info.Spans;

    public TerminalPoint Position { get; } = info.Position;

    public event EventHandler? Disposed;

    internal void Render(ITerminalDrawingContext drawingContext)
    {
        drawingContext.DrawText(this);
    }

    protected virtual void OnDisposed(EventArgs e)
    {
        Disposed?.Invoke(this, e);
    }

    public void Dispose()
    {
        if (_isDisposed is true)
        {
            throw new ObjectDisposedException($"{this}");
        }

        OnDisposed(EventArgs.Empty);
        _isDisposed = true;
        GC.SuppressFinalize(this);
    }
}
