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
using JSSoft.Terminals.Input;
using System.Diagnostics;
using JSSoft.Terminals.Extensions;
using System.Threading;
using System.IO;

namespace JSSoft.Terminals.Hosting;

public class Terminal : ITerminal
{
    private readonly ITerminalStyle _originStyle;
    private readonly TerminalFieldSetter _setter;
    private readonly TerminalRowCollection _view;
    private readonly TerminalSelectionCollection _selections;
    private readonly TerminalLineCollection _lines;
    private readonly TerminalTextWriter _writer;
    private readonly TerminalTextReader _reader = new();
    private readonly TerminalModes _modes = new();

    private bool _isReadOnly;
    private string _title = string.Empty;

    private int _maximumBufferHeight = 500;
    private bool _isFocused;
    private TerminalCoord _cursorCoordinate = TerminalCoord.Empty;
    private TerminalCoord _originCoordinate = TerminalCoord.Empty;
    private TerminalCoord _viewCoordinate = TerminalCoord.Empty;
    private TerminalDisplayInfo _displayInfo = TerminalDisplayInfo.Empty;

    private ITerminalStyle _actualStyle;
    private ITerminalStyle? _style;

    private IInputHandler? _inputHandler;
    private TerminalSelection _selecting = TerminalSelection.Empty;
    private TerminalSize _bufferSize = new(80, 25);
    private TerminalSize _size;

    internal readonly SynchronizationContext SynchronizationContext;

    public Terminal(ITerminalStyle originStyle, ITerminalScroll scroll)
    {
        if (SynchronizationContext.Current == null)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }
        SynchronizationContext = SynchronizationContext.Current!;
        _setter = new TerminalFieldSetter(this, OnPropertyChanged);
        _originStyle = _actualStyle = originStyle;
        Scroll = scroll;
        _lines = new(this);
        _writer = new(this);
        _view = new(this);
        _selections = new(this, InvokeUpdatedEvent);
        _inputHandler = new TerminalInputHandler();
        _inputHandler.Attach(this);
        _lines.Updated += Lines_Updated;
        _view.Updated += View_Updated;
        _actualStyle.PropertyChanged += ActualStyle_PropertyChanged;
        Scroll.PropertyChanged += Scroll_PropertyChanged;
    }

    public TerminalDisplayInfo DisplayInfo
    {
        get => _displayInfo;
        set => _setter.SetField(ref _displayInfo, value, nameof(DisplayInfo));
    }

    public string Title
    {
        get => _title;
        set => _setter.SetField(ref _title, value, nameof(Title));
    }

    public bool IsReadOnly
    {
        get => _isReadOnly;
        set => _setter.SetField(ref _isReadOnly, value, nameof(IsReadOnly));
    }

    public IInputHandler? InputHandler
    {
        get => _inputHandler;
        set
        {
            _inputHandler?.DeselectIf(condition: IsFocused, this);
            _inputHandler?.Detach(this);
            _setter.SetField(ref _inputHandler, value, nameof(InputHandler));
            _inputHandler?.Attach(this);
            _inputHandler?.SelectIf(condition: IsFocused, this);
        }
    }

    public bool IsFocused
    {
        get => _isFocused;
        set
        {
            if (_setter.SetField(ref _isFocused, value, nameof(IsFocused)) == true)
            {
                Update();
            }
        }
    }

    public int MaximumBufferHeight
    {
        get => _maximumBufferHeight;
        set
        {
            if (value < 5 || value < _bufferSize.Height)
                throw new ArgumentOutOfRangeException(nameof(value));
            _setter.SetField(ref _maximumBufferHeight, value, nameof(MaximumBufferHeight));
        }
    }

    public TerminalSize BufferSize => _bufferSize;

    public TerminalSize Size
    {
        get => _size;
        set
        {
            if (_size != value)
            {
                Resize(value.Width, value.Height);
            }
        }
    }

    public IReadOnlyList<ITerminalRow> View => _view;

    public TerminalCoord CursorCoordinate
    {
        get => _cursorCoordinate;
        private set
        {
            _setter.SetField(ref _cursorCoordinate, CoerceValue(value), nameof(CursorCoordinate));

            TerminalCoord CoerceValue(TerminalCoord value)
            {
                var x = value.X;
                var y = value.Y;
                var (bufferWidth, bufferHeight) = (_bufferSize.Width, _bufferSize.Height);
                var rowCount = _view.Count;
                var maxBufferHeight = Math.Max(bufferHeight, rowCount);
                x = Math.Min(x, bufferWidth - 1);
                x = Math.Max(x, 0);
                y = Math.Min(y, maxBufferHeight - 1);
                y = Math.Max(y, 0);
                return new TerminalCoord(x, y);
            }
        }
    }

    public TerminalCoord OriginCoordinate
    {
        get => _originCoordinate;
        set
        {
            if (_originCoordinate != value)
            {
                var index = new TerminalIndex(this, value).Linefeed();
                _originCoordinate = value;
                InvokePropertyChangedEvent(nameof(OriginCoordinate));
            }
        }
    }

    public TerminalCoord ViewCoordinate
    {
        get => _viewCoordinate;
        set
        {
            if (value.X < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            if (value.Y < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            if (value.X >= BufferSize.Width)
                throw new ArgumentOutOfRangeException(nameof(value));
            if (value.Y >= BufferSize.Height)
                throw new ArgumentOutOfRangeException(nameof(value));

            _setter.SetField(ref _viewCoordinate, value, nameof(ViewCoordinate));
        }
    }

    public ITerminalStyle ActualStyle
    {
        get => _actualStyle;
        private set
        {
            if (_actualStyle != value)
            {
                _actualStyle.PropertyChanged -= ActualStyle_PropertyChanged;
                _actualStyle = value;
                _actualStyle.PropertyChanged += ActualStyle_PropertyChanged;
                InvokePropertyChangedEvent(nameof(ActualStyle));
                UpdateCursorCoordinate();
            }
            else
            {
                throw new UnreachableException();
            }
        }
    }

    public ITerminalStyle? Style
    {
        get => _style;
        set
        {
            if (_setter.SetField(ref _style, value, nameof(Style)) == true)
            {
                ActualStyle = _style ?? _originStyle;
                _view.Update(_lines);
            }
        }
    }

    public ITerminalScroll Scroll { get; }

    public TerminalSelection Selecting
    {
        get => _selecting;
        set
        {
            if (_selecting != value)
            {
                _selecting = value;
                OnPropertyChanged(new(nameof(Selecting)));
                OnUpdated(new([.. _view]));
            }
        }
    }

    public ITerminalSelectionCollection Selections => _selections;

    public TextWriter Out => _writer;

    public TextReader In => _reader;

    public ITerminalModes Modes => _modes;

    public static TerminalSize GetBufferSize(Terminal terminal, TerminalSize size)
    {
        var bufferSize = TerminalSize.Empty;
        var font = terminal.ActualStyle.Font;
        var itemWidth = font.Width;
        var itemHeight = font.Height;
        bufferSize.Width = size.Width / itemWidth;
        bufferSize.Height = size.Height / itemHeight;
        bufferSize.Height = Math.Min(terminal.MaximumBufferHeight, bufferSize.Height);
        return bufferSize;
    }

    public TerminalPoint ViewToWorld(TerminalPoint position)
    {
        var font = ActualStyle.Font;
        var offset = font.Height * Scroll.Value;
        return position + new TerminalPoint(0, offset);
    }

    public TerminalCoord ViewToWorld(TerminalCoord viewCoord)
    {
        var offset = Scroll.Value;
        return viewCoord + new TerminalCoord(0, offset);
    }

    public TerminalCoord PositionToCoordinate(TerminalPoint position)
    {
        var font = ActualStyle.Font;
        var itemWidth = font.Width;
        var itemHeight = font.Height;
        var x = position.X / itemWidth;
        var y = position.Y / itemHeight;
        return new TerminalCoord(x, y);
    }

    public TerminalCharacterInfo? GetInfo(TerminalCoord coord)
        => _lines.GetInfo(coord.X, coord.Y);

    public bool BringIntoView(int y)
    {
        var scroll = Scroll;
        var topIndex = _originCoordinate.Y;
        var bottomIndex = topIndex + BufferSize.Height;
        if (scroll.Value < _lines.Count - BufferSize.Height)
        {
            var value = _lines.Count - BufferSize.Height;
            scroll.PropertyChanged -= Scroll_PropertyChanged;
            scroll.Value = scroll.CoerceValue(value);
            scroll.PropertyChanged += Scroll_PropertyChanged;
            return true;
        }
        if (scroll.Value < topIndex)
        {
            var value = topIndex;
            scroll.PropertyChanged -= Scroll_PropertyChanged;
            scroll.Value = scroll.CoerceValue(value);
            scroll.PropertyChanged += Scroll_PropertyChanged;
            return true;
        }
        return false;
    }

    public string Copy()
        => Selections.Any() == true ? _lines.GetString([.. Selections]) : string.Empty;

    public void Paste(string text) => WriteInput(text);

    public void Resize(double width, double height)
    {
        if (width < 0)
            throw new ArgumentOutOfRangeException(nameof(width));
        if (height < 0)
            throw new ArgumentOutOfRangeException(nameof(height));

        var size = new TerminalSize((int)width, (int)height);
        var bufferSize = GetBufferSize(this, size);
        if (bufferSize == _bufferSize)
            return;

        using (var _ = _setter.LockEvent())
        {
            _setter.SetField(ref _size, size, nameof(Size));
            if (_setter.SetField(ref _bufferSize, bufferSize, nameof(BufferSize)) == true)
            {
                _lines.Update();
                Scroll.PropertyChanged -= Scroll_PropertyChanged;
                Scroll.ViewportSize = _bufferSize.Height;
                Scroll.SmallChange = 1;
                Scroll.LargeChange = _bufferSize.Height;
                Scroll.Minimum = 0;
                Scroll.Maximum = GetScrollMaximum();
                Scroll.IsVisible = Scroll.Maximum > 0;
                Scroll.Value = Scroll.CoerceValue(Scroll.Value);
                Scroll.PropertyChanged += Scroll_PropertyChanged;
            }
        }
        _view.Update(_lines);
        UpdateCursorCoordinate();
    }

    public void Update(params ITerminalRow[] rows) => InvokeUpdatedEvent(rows);

    public void Clear()
    {
        using var _ = _setter.LockEvent();
        _lines.Clear();
    }

    public void Append(string text)
    {
        _lines.Append(text, _displayInfo);
        UpdateCursorCoordinate();
        BringIntoView(_cursorCoordinate.Y);
    }

    public void WriteInput(string text) => _reader.Write(text);

    public event PropertyChangedEventHandler? PropertyChanged;

    public event EventHandler<TerminalUpdateEventArgs>? Updated;

    public event EventHandler<TerminalModeChangedEventArgs>? ModeChanged
    {
        add => _modes.ModeChanged += value;
        remove => _modes.ModeChanged -= value;
    }

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        => PropertyChanged?.Invoke(this, e);

    protected virtual void OnUpdated(TerminalUpdateEventArgs e)
        => Updated?.Invoke(this, e);

    private void UpdateCursorCoordinate()
    {
        var index = _lines.Index;
        _setter.SetField(ref _cursorCoordinate, _lines.GetCoordinate(index), nameof(CursorCoordinate));
    }

    private void InvokePropertyChangedEvent(string propertyName)
        => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

    private void ActualStyle_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ITerminalStyle.IsScrollForwardEnabled))
        {
            Scroll.PropertyChanged -= Scroll_PropertyChanged;
            Scroll.Maximum = GetScrollMaximum();
            Scroll.IsVisible = Scroll.Maximum > 0;
            Scroll.PropertyChanged += Scroll_PropertyChanged;
        }
    }

    private void Lines_Updated(object? sender, EventArgs e)
    {
        Scroll.PropertyChanged -= Scroll_PropertyChanged;
        Scroll.Minimum = 0;
        Scroll.Maximum = GetScrollMaximum();
        Scroll.IsVisible = Scroll.Maximum > 0;
        Scroll.PropertyChanged += Scroll_PropertyChanged;
        _view.Update(_lines);
        UpdateCursorCoordinate();
    }

    private void View_Updated(object? sender, TerminalRowUpdateEventArgs e)
        => InvokeUpdatedEvent(e.ChangedRows);

    private void Scroll_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        => _view.Update(_lines);

    private int GetScrollMaximum()
    {
        if (ActualStyle.IsScrollForwardEnabled == false)
        {
            return Math.Max(_originCoordinate.Y, _lines.Count - BufferSize.Height);
        }
        return Math.Max(_lines.Count, _maximumBufferHeight) - BufferSize.Height;
    }

    private void InvokeUpdatedEvent(ITerminalRow[] rows) => OnUpdated(new(rows));

    #region ITerminal

    ITerminalScroll ITerminal.Scroll => Scroll;

    #endregion
}
