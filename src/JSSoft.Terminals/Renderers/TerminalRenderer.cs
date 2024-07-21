// <copyright file="TerminalRenderer.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Renderers;

public class TerminalRenderer : TerminalRendererBase
{
    private readonly ITerminal _terminal;
    private readonly ITerminalRenderer[] _renderers;

    public void Render(ITerminalDrawingContext drawingContext) => ((ITerminalRenderer)this).Render(drawingContext);

    public void Dispose() => ((IDisposable)this).Dispose();

    public TerminalRenderer(ITerminal terminal)
    {
        _terminal = terminal;
        _renderers =
        [
            CreateBackgroundRenderer(_terminal),
            CreateRowsRenderer(_terminal),
            CreateCursorRenderer(_terminal),
        ];
    }

    protected virtual ITerminalRenderer CreateRowsRenderer(ITerminal terminal) => new TerminalRowsRenderer(terminal);

    protected virtual ITerminalRenderer CreateBackgroundRenderer(ITerminal terminal) => new TerminalBackgroundRenderer(terminal);

    protected virtual ITerminalRenderer CreateCursorRenderer(ITerminal terminal) => new TerminalCursorRenderer(terminal);

    protected override void OnRender(ITerminalDrawingContext drawingContext)
    {
        Array.ForEach(_renderers, item => item.Render(drawingContext));
    }

    protected override void OnDispose()
    {
        for (var i = _renderers.Length - 1; i >= 0; i--)
        {
            if (_renderers[i] is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
