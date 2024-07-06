// <copyright file="TerminalBackgroundRenderer.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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
