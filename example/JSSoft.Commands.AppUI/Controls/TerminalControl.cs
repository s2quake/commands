// <copyright file="TerminalControl.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Rendering;
using Avalonia.Utilities;
using JSSoft.Terminals;
using JSSoft.Terminals.Extensions;
using JSSoft.Terminals.Input;

namespace JSSoft.Commands.AppUI.Controls;

[TemplatePart(PART_TerminalPresenter, typeof(TerminalPresenter))]
[TemplatePart(PART_VerticalScrollBar, typeof(ScrollBar))]
public class TerminalControl : TemplatedControl, ICustomHitTest
{
    public const string PART_TerminalPresenter = nameof(PART_TerminalPresenter);
    public const string PART_VerticalScrollBar = nameof(PART_VerticalScrollBar);

    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<TerminalControl, string>(nameof(Title));

    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<TerminalControl, bool>(nameof(IsReadOnly));

    public static readonly StyledProperty<TerminalStyle?> TerminalStyleProperty =
        AvaloniaProperty.Register<TerminalControl, TerminalStyle?>(nameof(TerminalStyle));

    public static readonly StyledProperty<TerminalControlCompleter> CompleterProperty =
        AvaloniaProperty.Register<TerminalControl, TerminalControlCompleter>(
            nameof(Completer), defaultValue: (items, find) => []);

    public static readonly DirectProperty<TerminalControl, Size> BufferSizeProperty =
        AvaloniaProperty.RegisterDirect<TerminalControl, Size>(
            nameof(BufferSize), o => o.BufferSize);

    private readonly TerminalKeyBindingCollection _keyBindings
        = [.. TerminalKeyBindings.GetDefaultBindings()];

    private readonly Terminals.Hosting.Terminal _terminal;
    private readonly TerminalStyle _terminalStyle = new();
    private readonly TerminalScroll _terminalScroll = new();
    private readonly TerminalControlTextInputMethodClient _imClient = new();
    private IInputHandler? _inputHandler;
    private TerminalPresenter? _terminalPresenter;
    private Size _bufferWidth;
    private double _scrollValue;
    private Visual _inputVisual;
    private Window? _window;

    static TerminalControl()
    {
        FocusableProperty.OverrideDefaultValue(typeof(TerminalControl), true);
        TextInputMethodClientRequestedEvent.AddClassHandler<TerminalControl>((tc, e) =>
        {
            if (!tc.IsReadOnly)
            {
                e.Client = tc._imClient;
            }
        });
    }

    public TerminalControl()
    {
        _terminal = new(_terminalStyle, _terminalScroll);
        _terminal.PropertyChanged += Terminal_PropertyChanged;
        _inputHandler = _terminal.InputHandler;
        _terminalScroll.PropertyChanged += TerminalScroll_PropertyChanged;
        _inputVisual = this;
    }

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public TerminalStyle? TerminalStyle
    {
        get => GetValue(TerminalStyleProperty);
        set => SetValue(TerminalStyleProperty, value);
    }

    public TerminalControlCompleter Completer
    {
        get => GetValue(CompleterProperty);
        set => SetValue(CompleterProperty, value);
    }

    public Size BufferSize
    {
        get => _bufferWidth;
        private set => SetAndRaise(BufferSizeProperty, ref _bufferWidth, value);
    }

    public TextWriter Out => _terminal.Out;

    public TextReader In => _terminal.In;

    public async Task CopyAsync()
    {
        var text = _terminal.Copy();
        if (TopLevel.GetTopLevel(this)?.Clipboard is { } clipboard)
        {
            await clipboard.SetTextAsync(text);
        }
    }

    public async Task PasteAsync()
    {
        if (TopLevel.GetTopLevel(this)?.Clipboard is { } clipboard &&
            await clipboard.GetTextAsync() is { } text)
        {
            _terminal.Paste(text);
        }
    }

    public void SelectAll() => _terminal.Selections.SelectAll();

    public void WriteInput(string text) => _terminal.WriteInput(text);

    bool ICustomHitTest.HitTest(Point point) => true;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (e.NameScope.Find(PART_TerminalPresenter) is TerminalPresenter terminalPresenter)
        {
            _terminalStyle.FontFamily = FontFamily;
            _terminalStyle.FontSize = FontSize;
            _terminalPresenter = terminalPresenter;
            _terminalPresenter.SetObject(_terminal);
            _inputVisual = _terminalPresenter;
        }

        if (e.NameScope.Find(PART_VerticalScrollBar) is ScrollBar scrollBar)
        {
            _terminalScroll.ScrollBar = scrollBar;
        }

        _imClient.SetPresenter(_terminalPresenter, this);
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        _scrollValue -= e.Delta.Y;
        _terminalScroll.PropertyChanged -= TerminalScroll_PropertyChanged;
        _terminalScroll.Value = _terminalScroll.CoerceValue((int)Math.Floor(_scrollValue));
        _terminalScroll.PropertyChanged += TerminalScroll_PropertyChanged;
        _scrollValue = MathUtilities.Clamp(
            _scrollValue, _terminalScroll.Minimum, _terminalScroll.Maximum);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        _inputHandler?.PointerDown(_terminal, new TerminalPointerEventData(e, _inputVisual));
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        _inputHandler?.PointerMove(_terminal, new TerminalPointerEventData(e, _inputVisual));
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        _inputHandler?.PointerUp(_terminal, new TerminalPointerEventData(e, _inputVisual));
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);
        _inputHandler?.PointerEnter(_terminal, new TerminalPointerEventData(e, _inputVisual));
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        _inputHandler?.PointerExit(_terminal, new TerminalPointerEventData(e, _inputVisual));
    }

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);
        _terminal.IsFocused = true;
        _imClient.SetPresenter(_terminalPresenter, this);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        _terminal.IsFocused = false;
        _imClient.SetPresenter(null, null);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (TopLevel.GetTopLevel(this) is Window window)
        {
            _window = window;
            _window.Activated += Window_Activated;
            _window.Deactivated += Window_Deactivated;
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        if (_window is not null)
        {
            _window.Activated -= Window_Activated;
            _window.Deactivated -= Window_Deactivated;
        }

        base.OnDetachedFromVisualTree(e);
        _imClient.SetPresenter(null, null);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        var modifiers = TerminalMarshal.Convert(e.KeyModifiers);
        var key = TerminalMarshal.Convert(e.Key);
        if (e.Handled is false && _keyBindings.Process(_terminal, modifiers, key) is true)
        {
            e.Handled = true;
        }

        if (e.Handled is false && ProcessHotKey(e) is true)
        {
            e.Handled = true;
        }

        if (e.Handled is false && e.KeySymbol is { } keySymbol)
        {
            _terminal.WriteInput(keySymbol);
            e.Handled = true;
        }
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);
        if (e.Handled is false && e.Text is { } text)
        {
            _terminal.WriteInput(text);
            e.Handled = true;
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == FontFamilyProperty)
        {
            _terminalStyle.FontFamily = FontFamily;
        }
        else if (change.Property == FontSizeProperty)
        {
            _terminalStyle.FontSize = FontSize;
        }
        else if (change.Property == TerminalStyleProperty)
        {
            _terminal.Style = TerminalStyle;
        }
        else if (change.Property == TitleProperty)
        {
            if (_terminal.Title != Title)
            {
                _terminal.PropertyChanged -= Terminal_PropertyChanged;
                _terminal.Title = Title;
                _terminal.PropertyChanged += Terminal_PropertyChanged;
            }
        }
    }

    private bool ProcessHotKey(KeyEventArgs e)
    {
        var keyMap = Application.Current!.PlatformSettings!.HotkeyConfiguration;
        bool Match(List<KeyGesture> gestures) => gestures.Exists(g => g.Matches(e));

        if (Match(keyMap.Copy))
        {
            _ = CopyAsync();
            return true;
        }
        else if (Match(keyMap.Paste))
        {
            _ = PasteAsync();
            return true;
        }
        else if (Match(keyMap.SelectAll))
        {
            SelectAll();
            return true;
        }

        return false;
    }

    private void Terminal_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ITerminal.InputHandler))
        {
            _inputHandler = _terminal.InputHandler;
        }
        else if (e.PropertyName == nameof(ITerminal.BufferSize))
        {
            BufferSize = new Size(_terminal.BufferSize.Width, _terminal.BufferSize.Height);
        }
        else if (e.PropertyName == nameof(ITerminal.Title))
        {
            SetCurrentValue(TitleProperty, _terminal.Title);
        }
    }

    private void TerminalScroll_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ITerminalScroll.Value))
        {
            _scrollValue = _terminalScroll.Value;
        }
    }

    private void Window_Deactivated(object? sender, EventArgs e)
    {
        _terminal.IsFocused = false;
    }

    private void Window_Activated(object? sender, EventArgs e)
    {
        _terminal.IsFocused = IsFocused;
    }
}
