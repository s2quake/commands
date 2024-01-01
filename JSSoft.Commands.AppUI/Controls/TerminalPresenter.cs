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
using Avalonia.Controls;
using Avalonia.Media;
using JSSoft.Terminals;
using JSSoft.Terminals.Renderers;

namespace JSSoft.Commands.AppUI.Controls;

public class TerminalPresenter : Control
{
    private Terminals.Hosting.Terminal? _terminal;
    private TerminalRenderer? _renderer;

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        _renderer?.Render(new TerminalDrawingContext(context));
    }

    internal void SetObject(Terminals.Hosting.Terminal terminal)
    {
        if (_terminal != null)
        {
            _terminal.Updated -= Terminal_Updated;
            _terminal.Scroll.PropertyChanged -= Scroll_PropertyChanged;
            _terminal.PropertyChanged -= Terminal_PropertyChanged;
            _renderer?.Dispose();
        }
        _terminal = terminal;
        if (_terminal != null)
        {
            _renderer = new TerminalRenderer(_terminal);
            _terminal.PropertyChanged += Terminal_PropertyChanged;
            _terminal.Scroll.PropertyChanged += Scroll_PropertyChanged;
            _terminal.Updated += Terminal_Updated;
        }
        InvalidateVisual();
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        _terminal?.ResizeBuffer((float)e.NewSize.Width, (float)e.NewSize.Height);
    }

    private void Terminal_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ITerminal.CursorCoordinate))
        {
            InvalidateVisual();
        }
    }

    private void Scroll_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ITerminalScroll.Value))
        {
            // Trace.WriteLine(nameof(Scroll_PropertyChanged));
            InvalidateVisual();
        }
    }

    private void Terminal_Updated(object? sender, TerminalUpdateEventArgs e)
    {
        // System.Diagnostics.Trace.WriteLine(nameof(Terminal_Updated));
        InvalidateVisual();
    }
}
