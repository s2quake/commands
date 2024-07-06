// <copyright file="TerminalRowRenderer.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using JSSoft.Terminals.Extensions;
using JSSoft.Terminals.Renderers.Extensions;

namespace JSSoft.Terminals.Renderers;

public class TerminalRowRenderer(ITerminalRow row) : ITerminalRenderer
{
    private ITerminalRow _row = row;
    private TerminalGlyphRun[]? _glyphRuns;
    private TerminalRectRun[]? _rectRuns;

    public virtual void Reset(ITerminalRow row)
    {
        if (_glyphRuns is not null)
        {
            Array.ForEach(_glyphRuns, item => item.Dispose());
        }
        _row = row;
        _glyphRuns = null;
        _rectRuns = null;
    }

    public virtual void Render(ITerminalDrawingContext drawingContext)
    {
        _rectRuns ??= TerminalRectRunFactory<TerminalRectRun>.Create(_row, item => new TerminalRectRun(item));
        if (_glyphRuns is null)
        {
            var font = _row.Terminal.ActualStyle.Font;
            _glyphRuns = TerminalGlyphRunFactory<TerminalGlyphRun>.Create(_row, item => new TerminalGlyphRun(item));
        }
        if (_rectRuns is not null)
        {
            Array.ForEach(_rectRuns, item => item.Render(drawingContext));
        }
        if (_glyphRuns is not null)
        {
            var transform = _row.Terminal.GetTransform(_row.Index);
            using var _ = drawingContext.PushTransform(0, transform.Y);
            Array.ForEach(_glyphRuns, item => drawingContext.DrawText(item));
        }
    }
}
