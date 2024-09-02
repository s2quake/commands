// <copyright file="TerminalCursorRenderer.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using JSSoft.Terminals.Extensions;

namespace JSSoft.Terminals.Renderers;

public class TerminalCursorRenderer : TerminalRendererBase
{
    private readonly ITerminal _terminal;
    private readonly TerminalStyleProperty<TerminalCursorShape> _cursorShapeProperty;
    private readonly TerminalStyleProperty<int> _cursorThicknessProperty;
    private readonly TerminalStyleProperty<TerminalColor> _cursorBackgroundColorProperty;
    private readonly TerminalStyleProperty<double> _cursorBlinkDelayProperty;
    private readonly TerminalStyleProperty<bool> _isCursorBlinkableProperty;
    private readonly TerminalCursorTimer _cursorTimer;

    private bool _isCursorVisible = true;
    private TerminalCursorShape _cursorShape;
    private int _cursorThickness;
    private TerminalColor _cursorBackgroundColor;
    private int _cursorBlinkDelay;
    private bool _isCursorBlinkable;
    private TerminalGlyphRun? _cursorGlyphRun;
    private Action<ITerminalDrawingContext, TerminalCoord, int> _cursorRenderer;

    public TerminalCursorRenderer(ITerminal terminal)
    {
        _terminal = terminal;
        _cursorTimer = new TerminalCursorTimer()
        {
            Interval = _cursorBlinkDelay,
            IsEnabled = _terminal.IsFocused && _isCursorBlinkable,
        };
        _cursorShapeProperty = new(terminal, nameof(ITerminalStyle.CursorShape), CursorShapeProperty_Changed);
        _cursorThicknessProperty = new(terminal, nameof(ITerminalStyle.CursorThickness), (s, e) => CursorThickness = e.Value);
        _cursorBackgroundColorProperty = new(terminal, nameof(ITerminalStyle.CursorBackgroundColor), (s, e) => CursorBackgroundColor = e.Value);
        _cursorBlinkDelayProperty = new(terminal, nameof(ITerminalStyle.CursorBlinkDelay), (s, e) => CursorBlinkDelay = (int)(e.Value * 1000));
        _isCursorBlinkableProperty = new(terminal, nameof(ITerminalStyle.IsCursorBlinkable), (s, e) => IsCursorBlinkable = e.Value);
        _cursorGlyphRun = GetGlyphRun(this);
        _cursorRenderer = GetCursorRenderer(this, _cursorShape);
        _terminal.PropertyChanged += Terminal_PropertyChanged;
        _cursorTimer.Tick += CursorTimer_Tick;
    }

    public TerminalCursorShape CursorShape
    {
        get => _cursorShape;
        private set => SetField(ref _cursorShape, value, nameof(TerminalCursorShape));
    }

    public int CursorThickness
    {
        get => _cursorThickness;
        private set => SetField(ref _cursorThickness, value, nameof(CursorThickness));
    }

    public TerminalColor CursorBackgroundColor
    {
        get => _cursorBackgroundColor;
        private set => SetField(ref _cursorBackgroundColor, value, nameof(CursorBackgroundColor));
    }

    public int CursorBlinkDelay
    {
        get => _cursorBlinkDelay;
        private set
        {
            if (SetField(ref _cursorBlinkDelay, value, nameof(CursorBlinkDelay)) is true)
            {
                _cursorTimer.Interval = _cursorBlinkDelay;
            }
        }
    }

    public bool IsCursorBlinkable
    {
        get => _isCursorBlinkable;
        private set
        {
            if (SetField(ref _isCursorBlinkable, value, nameof(IsCursorBlinkable)) is true)
            {
                _cursorTimer.IsEnabled = _terminal.IsFocused && _isCursorBlinkable;
            }
        }
    }

    public bool IsCursorVisible
    {
        get
        {
            if (_terminal.ActualStyle.CursorVisibility == TerminalCursorVisibility.OnlyInFocus && _terminal.IsFocused is false)
            {
                return false;
            }

            if (_terminal.IsFocused is true && IsCursorBlinkable is true)
            {
                return _isCursorVisible;
            }

            return true;
        }
    }

    protected virtual TerminalGlyphRun CreateGlyphRun(TerminalGlyphRunInfo info) => new(info);

    protected override void OnRender(ITerminalDrawingContext drawingContext)
    {
        if (IsCursorVisible is false)
        {
            return;
        }

        var terminal = _terminal;
        var scroll = _terminal.Scroll;
        var cursorCoord = terminal.CursorCoordinate;
        var top = scroll.Value;
        var bottom = top + terminal.BufferSize.Height;
        if (cursorCoord.Y >= top && cursorCoord.Y < bottom)
        {
            var height = _terminal.ActualStyle.Font.Height;
            var characterInfo = _terminal.GetInfo(cursorCoord);
            var span = characterInfo?.Span ?? 1;
            var transform = drawingContext.PushTransform(new TerminalPoint(0, -scroll.Value * height));
            var isFocused = terminal.IsFocused;
            if (isFocused is true)
            {
                _cursorRenderer.Invoke(drawingContext, cursorCoord, span);
            }
            else
            {
                var backgroundRect = terminal.GetBackgroundRect(cursorCoord, 1);
                var cursorBackgroundColor = CursorBackgroundColor;
                var cursorThickness = CursorThickness;
                _cursorGlyphRun?.Render(drawingContext);
                drawingContext.DrawRectangle(cursorBackgroundColor, cursorThickness, backgroundRect);
            }
        }
    }

    protected override void OnDispose()
    {
        _cursorShapeProperty.Changed -= CursorShapeProperty_Changed;
        _cursorTimer.Tick -= CursorTimer_Tick;
        _terminal.PropertyChanged -= Terminal_PropertyChanged;
        _cursorGlyphRun?.Dispose();
        _cursorTimer.Dispose();
        _isCursorBlinkableProperty.Dispose();
        _cursorBlinkDelayProperty.Dispose();
        _cursorThicknessProperty.Dispose();
        _cursorBackgroundColorProperty.Dispose();
        _cursorShapeProperty.Dispose();
    }

    protected virtual void OnRenderBlock(ITerminalDrawingContext drawingContext, TerminalCoord coord, int span)
    {
        var terminal = _terminal;
        var backgroundRect = terminal.GetBackgroundRect(coord, span);
        var cursorBackgroundColor = CursorBackgroundColor;
        // var cursorThickness = CursorThickness;
        drawingContext.FillRectangle(cursorBackgroundColor, backgroundRect);
        _cursorGlyphRun?.Render(drawingContext);
    }

    protected virtual void OnRenderUnderline(ITerminalDrawingContext drawingContext, TerminalCoord coord, int span)
    {
        var terminal = _terminal;
        var backgroundRect = terminal.GetBackgroundRect(coord, span);
        var cursorBackgroundColor = CursorBackgroundColor;
        var cursorThickness = CursorThickness;
        var rect = new TerminalRect(backgroundRect.X, backgroundRect.Y + backgroundRect.Height - cursorThickness, backgroundRect.Width, cursorThickness);
        _cursorGlyphRun?.Render(drawingContext);
        drawingContext.FillRectangle(cursorBackgroundColor, rect);
    }

    protected virtual void OnRenderVerticalBar(ITerminalDrawingContext drawingContext, TerminalCoord coord, int span)
    {
        var terminal = _terminal;
        var backgroundRect = terminal.GetBackgroundRect(coord, span);
        var cursorBackgroundColor = CursorBackgroundColor;
        var cursorThickness = CursorThickness;
        var rect = new TerminalRect(backgroundRect.X, backgroundRect.Y, cursorThickness, backgroundRect.Height);
        _cursorGlyphRun?.Render(drawingContext);
        drawingContext.FillRectangle(cursorBackgroundColor, rect);
    }

    protected static bool Predicate(char character)
    {
        if (character == 0)
        {
            return false;
        }

        if (character == ' ')
        {
            return false;
        }

        return true;
    }

    private static TerminalGlyphRun? GetGlyphRun(TerminalCursorRenderer obj)
    {
        var terminal = obj._terminal;
        var cursorCoordinate = terminal.CursorCoordinate - new TerminalCoord(0, terminal.Scroll.Value);
        if (TryGetCell(terminal, cursorCoordinate, out var cell) is true && Predicate(cell.Character) is true)
        {
            var row = terminal.View[cursorCoordinate.Y];
            return TerminalGlyphRunFactory<TerminalGlyphRun>.Create(row, cursorCoordinate.X, obj.CreateGlyphRun);
        }

        return null;

        static bool TryGetCell(ITerminal terminal, TerminalCoord coord, out TerminalCharacterInfo cell)
        {
            if (coord.Y >= 0 && coord.Y < terminal.View.Count)
            {
                if (coord.X >= 0 && coord.X < terminal.View[coord.Y].Length)
                {
                    cell = terminal.View[coord.Y][coord.X];
                    return true;
                }
            }

            cell = TerminalCharacterInfo.Empty;
            return false;
        }
    }

    private static Action<ITerminalDrawingContext, TerminalCoord, int> GetCursorRenderer(TerminalCursorRenderer obj, TerminalCursorShape cursorShape) => cursorShape switch
    {
        TerminalCursorShape.Block => obj.OnRenderBlock,
        TerminalCursorShape.Underline => obj.OnRenderUnderline,
        TerminalCursorShape.VerticalBar => obj.OnRenderVerticalBar,
        _ => throw new NotSupportedException(),
    };

    private void CursorShapeProperty_Changed(object? sender, TerminalStylePropertyChangedEventArgs<TerminalCursorShape> e)
    {
        _cursorShape = e.Value;
        _cursorRenderer = GetCursorRenderer(this, e.Value);
    }

    private void Terminal_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ITerminal.CursorCoordinate))
        {
            _cursorGlyphRun?.Dispose();
            _cursorGlyphRun = GetGlyphRun(this);
            _cursorTimer.Reset();
            _isCursorVisible = true;
        }
        else if (e.PropertyName == nameof(ITerminal.IsFocused))
        {
            _cursorTimer.IsEnabled = _terminal.IsFocused && _isCursorBlinkableProperty.Value;
        }
    }

    private void CursorTimer_Tick(object? sender, EventArgs e)
    {
        _isCursorVisible = !_isCursorVisible;
        _terminal.Update();
    }
}
