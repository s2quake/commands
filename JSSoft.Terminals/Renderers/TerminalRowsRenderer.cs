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

using System.ComponentModel;

namespace JSSoft.Terminals.Renderers;

public class TerminalRowsRenderer : TerminalRendererBase
{
    private readonly ITerminal _terminal;
    private TerminalRowRenderer[] _rowRenderers = [];
    private TerminalStyleProperty<TerminalColor> _backgroundColorProperty;

    public TerminalRowsRenderer(ITerminal terminal)
    {
        _terminal = terminal;
        _backgroundColorProperty = new(terminal, nameof(ITerminalStyle.BackgroundColor));
        Array.Resize(ref _rowRenderers, terminal.BufferSize.Height);
        for (var i = 0; i < _rowRenderers.Length; i++)
        {
            _rowRenderers[i] = Create(terminal.View[i]);
        }
        _terminal.Updated += Terminal_Updated;
        _terminal.PropertyChanged += Terminal_PropertyChanged;
        _backgroundColorProperty.Changed += BackgroundColorProperty_Changed;
    }

    private void Terminal_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ITerminal.BufferSize))
        {
            Array.Resize(ref _rowRenderers, _terminal.BufferSize.Height);
            for (var i = 0; i < _rowRenderers.Length; i++)
            {
                _rowRenderers[i] ??= Create(_terminal.View[i]);
            }
        }
    }

    protected override void OnRender(ITerminalDrawingContext drawingContext)
    {
        for (var i = 0; i < _terminal.View.Count; i++)
        {
            var row = _terminal.View[i];
            var rowPresenter = _rowRenderers[i];
            rowPresenter.Render(drawingContext);
        }
    }

    protected override void OnDispose()
    {
        _backgroundColorProperty.Changed -= BackgroundColorProperty_Changed;
        _terminal.Updated -= Terminal_Updated;
    }

    protected virtual TerminalRowRenderer Create(ITerminalRow row) => new(row);

    private void Terminal_Updated(object? sender, TerminalUpdateEventArgs e)
    {
        foreach (var item in e.ChangedRows)
        {
            var index = item.Index;
            if (_rowRenderers[index] is null)
                _rowRenderers[index] = Create(item);
            else
                _rowRenderers[index].Reset(item);
        }
    }

    private void BackgroundColorProperty_Changed(object? sender, EventArgs e)
    {
        for (var i = 0; i < _rowRenderers.Length; i++)
        {
            if (_rowRenderers[i] is { } rowRenderer)
            {
                rowRenderer.Reset(_terminal.View[i]);
            }
        }
    }
}
