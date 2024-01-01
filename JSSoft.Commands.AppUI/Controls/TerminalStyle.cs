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
using Avalonia.Media;
using JSSoft.Terminals;

namespace JSSoft.Commands.AppUI.Controls;

public sealed class TerminalStyle : ITerminalStyle
{
    private readonly TerminalFieldSetter _setter;

    private FontFamily _fontFamily = FontManager.Current.DefaultFontFamily;
    private double _fontSize = 12;
    private TerminalFont? _font;

    private Color _foregroundColor = Convert(TerminalStyleUtility.DefaultForegroundColor);
    private Color _backgroundColor = Convert(TerminalStyleUtility.DefaultBackgroundColor);
    private Color _selectionForegroundColor = Convert(TerminalStyleUtility.DefaultSelectionForegroundColor);
    private Color _selectionBackgroundColor = Convert(TerminalStyleUtility.DefaultSelectionBackgroundColor);
    private TerminalColorSource _selectionForegroundColorSource = TerminalStyleUtility.DefaultSelectionForegroundColorSource;
    private TerminalColorSource _selectionBackgroundColorSource = TerminalStyleUtility.DefaultSelectionBackgroundColorSource;
    private Color _cursorForegroundColor = Convert(TerminalStyleUtility.DefaultCursorForegroundColor);
    private Color _cursorBackgroundColor = Convert(TerminalStyleUtility.DefaultCursorBackgroundColor);
    private TerminalColorSource _cursorForegroundColorSource = TerminalStyleUtility.DefaultCursorForegroundColorSource;
    private TerminalColorSource _cursorBackgroundColorSource = TerminalStyleUtility.DefaultCursorBackgroundColorSource;

    private TerminalCursorShape _cursorShape = TerminalStyleUtility.DefaultCursorShape;
    private TerminalCursorVisibility _cursorVisibility = TerminalStyleUtility.DefaultCursorVisibility;

    private int _cursorThickness = TerminalStyleUtility.DefaultCursorThickness;
    private bool _isCursorBlinkable = TerminalStyleUtility.DefaultIsCursorBlinkable;
    private double _cursorBlinkDelay = TerminalStyleUtility.DefaultCursorBlinkDelay;
    private bool _isScrollForwardEnabled = TerminalStyleUtility.DefaultIsScrollForwardEnabled;

    private Color _black = Convert(TerminalStyleUtility.DefaultBlack);
    private Color _red = Convert(TerminalStyleUtility.DefaultRed);
    private Color _green = Convert(TerminalStyleUtility.DefaultGreen);
    private Color _yellow = Convert(TerminalStyleUtility.DefaultYellow);
    private Color _blue = Convert(TerminalStyleUtility.DefaultBlue);
    private Color _magenta = Convert(TerminalStyleUtility.DefaultMagenta);
    private Color _cyan = Convert(TerminalStyleUtility.DefaultCyan);
    private Color _white = Convert(TerminalStyleUtility.DefaultWhite);
    private Color _brightBlack = Convert(TerminalStyleUtility.DefaultBrightBlack);
    private Color _brightRed = Convert(TerminalStyleUtility.DefaultBrightRed);
    private Color _brightGreen = Convert(TerminalStyleUtility.DefaultBrightGreen);
    private Color _brightYellow = Convert(TerminalStyleUtility.DefaultBrightYellow);
    private Color _brightBlue = Convert(TerminalStyleUtility.DefaultBrightBlue);
    private Color _brightMagenta = Convert(TerminalStyleUtility.DefaultBrightMagenta);
    private Color _brightCyan = Convert(TerminalStyleUtility.DefaultBrightCyan);
    private Color _brightWhite = Convert(TerminalStyleUtility.DefaultBrightWhite);

    public TerminalStyle()
    {
        _setter = new(this, InvokePropertyChangedEvent);
    }

    public FontFamily FontFamily
    {
        get => _fontFamily;
        set
        {
            if (_fontFamily != value)
            {
                _fontFamily = value;
                _font = null;
                InvokePropertyChangedEvent(new PropertyChangedEventArgs(nameof(ITerminalStyle.Font)));
            }
        }
    }

    public double FontSize
    {
        get => _fontSize;
        set
        {
            if (_fontSize != value)
            {
                _fontSize = value;
                if (_font != null)
                {
                    _font.Size = (int)value;
                }
                InvokePropertyChangedEvent(new PropertyChangedEventArgs(nameof(ITerminalStyle.Font)));
            }
        }
    }

    public Color ForegroundColor
    {
        get => _foregroundColor;
        set => _setter.SetField(ref _foregroundColor, value, nameof(ForegroundColor));
    }

    public Color BackgroundColor
    {
        get => _backgroundColor;
        set => _setter.SetField(ref _backgroundColor, value, nameof(BackgroundColor));
    }

    public Color SelectionForegroundColor
    {
        get => _selectionForegroundColor;
        set => _setter.SetField(ref _selectionForegroundColor, value, nameof(SelectionForegroundColor));
    }

    public Color SelectionBackgroundColor
    {
        get => _selectionBackgroundColor;
        set => _setter.SetField(ref _selectionBackgroundColor, value, nameof(SelectionBackgroundColor));
    }

    public TerminalColorSource SelectionForegroundColorSource
    {
        get => _selectionForegroundColorSource;
        set => _setter.SetField(ref _selectionForegroundColorSource, value, nameof(SelectionForegroundColorSource));
    }

    public TerminalColorSource SelectionBackgroundColorSource
    {
        get => _selectionBackgroundColorSource;
        set => _setter.SetField(ref _selectionBackgroundColorSource, value, nameof(SelectionBackgroundColorSource));
    }

    public Color CursorForegroundColor
    {
        get => _cursorForegroundColor;
        set => _setter.SetField(ref _cursorForegroundColor, value, nameof(CursorForegroundColor));
    }

    public Color CursorBackgroundColor
    {
        get => _cursorBackgroundColor;
        set => _setter.SetField(ref _cursorBackgroundColor, value, nameof(CursorBackgroundColor));
    }

    public TerminalColorSource CursorForegroundColorSource
    {
        get => _cursorForegroundColorSource;
        set => _setter.SetField(ref _cursorForegroundColorSource, value, nameof(CursorForegroundColorSource));
    }

    public TerminalColorSource CursorBackgroundColorSource
    {
        get => _cursorBackgroundColorSource;
        set => _setter.SetField(ref _cursorBackgroundColorSource, value, nameof(CursorBackgroundColorSource));
    }

    public TerminalCursorShape CursorShape
    {
        get => _cursorShape;
        set => _setter.SetField(ref _cursorShape, value, nameof(CursorShape));
    }

    public TerminalCursorVisibility CursorVisibility
    {
        get => _cursorVisibility;
        set => _setter.SetField(ref _cursorVisibility, value, nameof(CursorVisibility));
    }

    public int CursorThickness
    {
        get => _cursorThickness;
        set => _setter.SetField(ref _cursorThickness, value, nameof(CursorThickness));
    }

    public bool IsCursorBlinkable
    {
        get => _isCursorBlinkable;
        set => _setter.SetField(ref _isCursorBlinkable, value, nameof(IsCursorBlinkable));
    }

    public double CursorBlinkDelay
    {
        get => _cursorBlinkDelay;
        set => _setter.SetField(ref _cursorBlinkDelay, value, nameof(CursorBlinkDelay));
    }

    public bool IsScrollForwardEnabled
    {
        get => _isScrollForwardEnabled;
        set => _setter.SetField(ref _isScrollForwardEnabled, value, nameof(IsScrollForwardEnabled));
    }

    public Color Black
    {
        get => _black;
        set => _setter.SetField(ref _black, value, nameof(Black));
    }

    public Color Red
    {
        get => _red;
        set => _setter.SetField(ref _red, value, nameof(Red));
    }

    public Color Green
    {
        get => _green;
        set => _setter.SetField(ref _green, value, nameof(Green));
    }

    public Color Yellow
    {
        get => _yellow;
        set => _setter.SetField(ref _yellow, value, nameof(Yellow));
    }

    public Color Blue
    {
        get => _blue;
        set => _setter.SetField(ref _blue, value, nameof(Blue));
    }

    public Color Magenta
    {
        get => _magenta;
        set => _setter.SetField(ref _magenta, value, nameof(Magenta));
    }

    public Color Cyan
    {
        get => _cyan;
        set => _setter.SetField(ref _cyan, value, nameof(Cyan));
    }

    public Color White
    {
        get => _white;
        set => _setter.SetField(ref _white, value, nameof(White));
    }

    public Color BrightBlack
    {
        get => _brightBlack;
        set => _setter.SetField(ref _brightBlack, value, nameof(BrightBlack));
    }

    public Color BrightRed
    {
        get => _brightRed;
        set => _setter.SetField(ref _brightRed, value, nameof(BrightRed));
    }

    public Color BrightGreen
    {
        get => _brightGreen;
        set => _setter.SetField(ref _brightGreen, value, nameof(BrightGreen));
    }

    public Color BrightYellow
    {
        get => _brightYellow;
        set => _setter.SetField(ref _brightYellow, value, nameof(BrightYellow));
    }

    public Color BrightBlue
    {
        get => _brightBlue;
        set => _setter.SetField(ref _brightBlue, value, nameof(BrightBlue));
    }

    public Color BrightMagenta
    {
        get => _brightMagenta;
        set => _setter.SetField(ref _brightMagenta, value, nameof(BrightMagenta));
    }

    public Color BrightCyan
    {
        get => _brightCyan;
        set => _setter.SetField(ref _brightCyan, value, nameof(BrightCyan));
    }

    public Color BrightWhite
    {
        get => _brightWhite;
        set => _setter.SetField(ref _brightWhite, value, nameof(BrightWhite));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void InvokePropertyChangedEvent(PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }

    private static Color Convert(TerminalColor color) => TerminalMarshal.Convert(color);

    private static TerminalColor Convert(Color color) => TerminalMarshal.Convert(color);

    #region ITerminalStyle

    ITerminalFont ITerminalStyle.Font => _font ??= new TerminalFont(_fontFamily, (int)_fontSize);

    TerminalColor ITerminalStyle.ForegroundColor => Convert(ForegroundColor);

    TerminalColor ITerminalStyle.BackgroundColor => Convert(BackgroundColor);

    TerminalColor ITerminalStyle.SelectionForegroundColor => Convert(SelectionForegroundColor);

    TerminalColor ITerminalStyle.SelectionBackgroundColor => Convert(SelectionBackgroundColor);

    TerminalColor ITerminalStyle.CursorForegroundColor => Convert(CursorForegroundColor);

    TerminalColor ITerminalStyle.CursorBackgroundColor => Convert(CursorBackgroundColor);

    TerminalColor ITerminalStyle.Black => Convert(Black);

    TerminalColor ITerminalStyle.Red => Convert(Red);

    TerminalColor ITerminalStyle.Green => Convert(Green);

    TerminalColor ITerminalStyle.Yellow => Convert(Yellow);

    TerminalColor ITerminalStyle.Blue => Convert(Blue);

    TerminalColor ITerminalStyle.Magenta => Convert(Magenta);

    TerminalColor ITerminalStyle.Cyan => Convert(Cyan);

    TerminalColor ITerminalStyle.White => Convert(White);

    TerminalColor ITerminalStyle.BrightBlack => Convert(BrightBlack);

    TerminalColor ITerminalStyle.BrightRed => Convert(BrightRed);

    TerminalColor ITerminalStyle.BrightGreen => Convert(BrightGreen);

    TerminalColor ITerminalStyle.BrightYellow => Convert(BrightYellow);

    TerminalColor ITerminalStyle.BrightBlue => Convert(BrightBlue);

    TerminalColor ITerminalStyle.BrightMagenta => Convert(BrightMagenta);

    TerminalColor ITerminalStyle.BrightCyan => Convert(BrightCyan);

    TerminalColor ITerminalStyle.BrightWhite => Convert(BrightWhite);

    #endregion
}
