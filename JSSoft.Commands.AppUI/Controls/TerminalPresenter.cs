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
