using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Rendering;
using JSSoft.Terminals;
using JSSoft.Terminals.Input;
using JSSoft.Terminals.Extensions;
using Avalonia.Utilities;
using JSSoft.Terminals.Serializations;
using System.IO;

namespace JSSoft.Commands.AppUI.Controls;

[TemplatePart(PART_TerminalPresenter, typeof(TerminalPresenter))]
[TemplatePart(PART_VerticalScrollBar, typeof(ScrollBar))]
public class TerminalControl : TemplatedControl, ICustomHitTest
{
    public const string PART_TerminalPresenter = nameof(PART_TerminalPresenter);
    public const string PART_VerticalScrollBar = nameof(PART_VerticalScrollBar);

    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<TerminalControl, bool>(nameof(IsReadOnly));

    public static readonly StyledProperty<TerminalStyle?> TerminalStyleProperty =
        AvaloniaProperty.Register<TerminalControl, TerminalStyle?>(nameof(TerminalStyle));

    public static readonly StyledProperty<TerminalControlCompletor> CompletorProperty =
        AvaloniaProperty.Register<TerminalControl, TerminalControlCompletor>(nameof(Completor), defaultValue: (items, find) => []);

    public static readonly DirectProperty<TerminalControl, Size> BufferSizeProperty =
        AvaloniaProperty.RegisterDirect<TerminalControl, Size>(nameof(BufferSize), o => o.BufferSize);

    public static readonly RoutedEvent<TerminalExecutingRoutedEventArgs> ExecutingEvent =
        RoutedEvent.Register<TerminalControl, TerminalExecutingRoutedEventArgs>(
            nameof(Executing), RoutingStrategies.Bubble);

    public static readonly RoutedEvent<TerminalExecutedRoutedEventArgs> ExecutedEvent =
        RoutedEvent.Register<TerminalControl, TerminalExecutedRoutedEventArgs>(
            nameof(Executed), RoutingStrategies.Bubble);

    private readonly TerminalKeyBindingCollection _keyBindings = new(TerminalKeyBindings.GetDefaultBindings());

    private readonly Terminals.Hosting.Terminal _terminal;
    private readonly TerminalStyle _terminalStyle = new();
    private readonly TerminalScroll _terminalScroll = new();
    private IInputHandler? _inputHandler;
    private TerminalPresenter? _terminalPresenter;
    private ScrollBar? _scrollBar;
    private Size _bufferWidth;
    private double _scrollValue;
    private Visual _inputVisual;
    private Window? _window;

    static TerminalControl()
    {
        FocusableProperty.OverrideDefaultValue(typeof(TerminalControl), true);
    }

    public TerminalControl()
    {
        _terminal = new(_terminalStyle, _terminalScroll);
        _terminal.PropertyChanged += Terminal_PropertyChanged;
        _inputHandler = _terminal.InputHandler;
        _terminal.Prompt = "터미널 $ ";
        _terminal.Completor = GetCompletion;
        _terminal.Executing += Terminal_Executing;
        _terminal.Executed += Terminal_Executed;
        _terminalScroll.PropertyChanged += TerminalScroll_PropertyChanged;
        _inputVisual = this;

        _terminal.AppendLine("Last login: Mon Dec 25 21:19:42 on ttys002");
        // _terminal.AppendLine("1");
        // _terminal.AppendLine("2");
        // _terminal.AppendLine("3");
        // _terminal.AppendLine("4");
        // _terminal.AppendLine("5");
        // _terminal.OriginCoordinate = new TerminalCoord(0, 6);
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

    public TerminalControlCompletor Completor
    {
        get => GetValue(CompletorProperty);
        set => SetValue(CompletorProperty, value);
    }

    public Size BufferSize
    {
        get => _bufferWidth;
        private set => SetAndRaise(BufferSizeProperty, ref _bufferWidth, value);
    }

    public TextWriter Out
    {
        get => _terminal.Out;
        set => _terminal.Out = value;
    }

    public TextWriter Error
    {
        get => _terminal.Error;
        set => _terminal.Error = value;
    }

    public TextReader In
    {
        get => _terminal.In;
        set => _terminal.In = value;
    }

    public void Append(string text) => _terminal.Append(text);

    public void AppendLine(string text) => _terminal.AppendLine(text);

    public void Reset() => _terminal.Reset(TerminalCoord.Empty);

    public async void Copy()
    {
        var text = _terminal.Copy();
        if (TopLevel.GetTopLevel(this)?.Clipboard is { } clipboard)
        {
            await clipboard.SetTextAsync(text);
        }
    }

    public async void Paste()
    {
        if (TopLevel.GetTopLevel(this)?.Clipboard is { } clipboard &&
            await clipboard.GetTextAsync() is { } text)
        {
            _terminal.Paste(text);
        }
    }

    public void SelectAll() => _terminal.Selections.SelectAll();

    public object Save() => _terminal.Save();

    public void Load(object obj)
    {
        if (obj is TerminalDataInfo data)
        {
            _terminal.Load(data);
        }
    }

    public event EventHandler<TerminalExecutingRoutedEventArgs>? Executing
    {
        add => AddHandler(ExecutingEvent, value);
        remove => RemoveHandler(ExecutingEvent, value);
    }

    public event EventHandler<TerminalExecutedRoutedEventArgs>? Executed
    {
        add => AddHandler(ExecutedEvent, value);
        remove => RemoveHandler(ExecutedEvent, value);
    }

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
            _scrollBar = scrollBar;
        }
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        _scrollValue -= e.Delta.Y;
        _terminalScroll.PropertyChanged -= TerminalScroll_PropertyChanged;
        _terminalScroll.Value = _terminalScroll.CoerceValue((int)Math.Floor(_scrollValue));
        _terminalScroll.PropertyChanged += TerminalScroll_PropertyChanged;
        _scrollValue = MathUtilities.Clamp(_scrollValue, _terminalScroll.Minimum, _terminalScroll.Maximum);
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
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        _terminal.IsFocused = false;
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
        if (_window != null)
        {
            _window.Activated -= Window_Activated;
            _window.Deactivated -= Window_Deactivated;
        }
        base.OnDetachedFromVisualTree(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        var modifiers = TerminalMarshal.Convert(e.KeyModifiers);
        var key = TerminalMarshal.Convert(e.Key);
        if (e.Handled == false && e.Key == Key.Enter && e.KeyModifiers == KeyModifiers.None)
        {
            _terminal.ProcessText("\n");
            e.Handled = true;
        }
        if (e.Handled == false && _keyBindings.Process(_terminal, modifiers, key) == true)
        {
            e.Handled = true;
        }
        if (e.Handled == false && ProcessHotKey(e) == true)
        {
            e.Handled = true;
        }
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        if (e.Handled == false && e.Text is { } text)
        {
            _terminal.ProcessText(text);
            e.Handled = true;
        }
        base.OnTextInput(e);
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
    }

    private bool ProcessHotKey(KeyEventArgs e)
    {
        var keyMap = Application.Current!.PlatformSettings!.HotkeyConfiguration;
        bool Match(List<KeyGesture> gestures) => gestures.Any(g => g.Matches(e));

        if (Match(keyMap.Copy))
        {
            Copy();
            return true;
        }
        else if (Match(keyMap.Paste))
        {
            Paste();
            return true;
        }
        else if (Match(keyMap.SelectAll))
        {
            SelectAll();
            return true;
        }
        return false;
    }

    private string[] GetCompletion(string[] items, string find)
    {
        return Completor.Invoke(items, find);
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
    }

    private void Terminal_Executing(object? sender, TerminalExecutingEventArgs e)
    {
        var args = new TerminalExecutingRoutedEventArgs(e, ExecutingEvent);
        RaiseEvent(args);
    }

    private void Terminal_Executed(object? sender, TerminalExecutedEventArgs e)
    {
        var args = new TerminalExecutedRoutedEventArgs(e, ExecutedEvent);
        RaiseEvent(args);
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

    #region ICustomHitTest

    bool ICustomHitTest.HitTest(Point point) => true;

    #endregion
}
