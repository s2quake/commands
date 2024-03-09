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

public class TerminalBackgroundRenderer : TerminalRendererBase
{
    private readonly ITerminal _terminal;
    private readonly TerminalStyleProperty<TerminalColor> _backgroundColorProperty;
    private TerminalColor _backgroundColor;
    private TerminalRect _backgroundRect;

    public TerminalBackgroundRenderer(ITerminal terminal)
    {
        _terminal = terminal;
        _backgroundColorProperty = new(_terminal, nameof(ITerminalStyle.BackgroundColor), (s, e) => BackgroundColor = e.Value);
        BackgroundRect = new TerminalRect(0, 0, _terminal.Size.Width, _terminal.Size.Height);
        _terminal.PropertyChanged += Terminal_PropertyChanged;
    }

    public TerminalColor BackgroundColor
    {
        get => _backgroundColor;
        private set => SetField(ref _backgroundColor, value, nameof(BackgroundColor));
    }

    public TerminalRect BackgroundRect
    {
        get => _backgroundRect;
        private set => SetField(ref _backgroundRect, value, nameof(BackgroundRect));
    }

    protected override void OnRender(ITerminalDrawingContext drawingContext)
    {
    }

    protected override void OnDispose()
    {
        _terminal.PropertyChanged -= Terminal_PropertyChanged;
        _backgroundColorProperty.Dispose();
        base.OnDispose();
    }

    private void Terminal_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ITerminal.Size))
        {
            BackgroundRect = new TerminalRect(0, 0, _terminal.Size.Width, _terminal.Size.Height);
        }
    }

    private void BackgroundColorProperty_Changed(object? sender, EventArgs e)
    {
        BackgroundColor = _backgroundColorProperty.Value;
    }
}
