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
using JSSoft.Terminals.Serializations;
using System.Text.RegularExpressions;
using System.Threading;

namespace JSSoft.Terminals.Hosting;

public class Terminal : ITerminal
{
    private readonly ITerminalStyle _originStyle;
    private readonly TerminalFieldSetter _setter;
    private readonly TerminalRowCollection _view;
    private readonly TerminalSelection _selections;
    private readonly List<string> _historyList = [];
    private readonly TerminalBlock _block;
    private readonly TerminalBlockCollection _blocks;

    private TerminalBlock _outputBlock;
    private string _prompt = string.Empty;
    private string _inputText = string.Empty;
    private string _command = string.Empty;
    private string _completion = string.Empty;
    private int _historyIndex;
    private bool _isExecuting;
    private bool _isReadOnly;
    private int _cursorPosition;
    private TerminalCompletor _completor = (items, find) => [];
    private TerminalColorType? _foregroundColor;
    private TerminalColorType? _backgroundColor;

    private int _maximumBufferHeight = 500;
    private bool _isFocused;
    private TerminalCoord _cursorCoordinate = TerminalCoord.Empty;
    private TerminalCoord _originCoordinate = TerminalCoord.Empty;
    private TerminalCoord _viewCoordinate = TerminalCoord.Empty;

    private string _compositionString = string.Empty;
    private ITerminalStyle _actualStyle;
    private ITerminalStyle? _style;

    private IInputHandler? _inputHandler;
    private Terminals.TerminalSelection _selecting = Terminals.TerminalSelection.Empty;
    private TerminalSize _bufferSize = new(80, 25);
    private TerminalSize _size;

    private EventHandler<TerminalExecutingEventArgs>? _executing;
    private EventHandler<TerminalExecutedEventArgs>? _executed;

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
        _blocks = new(this);
        _view = new(this);
        _outputBlock = _blocks.CurrentBlock;
        _block = _blocks.CurrentBlock;
        _selections = new(this, InvokeUpdatedEvent);
        _inputHandler = new TerminalInputHandler();
        _inputHandler.Attach(this);
        _blocks.Updated += Blocks_Updated;
        _view.Updated += View_Updated;
        _actualStyle.PropertyChanged += ActualStyle_PropertyChanged;
        Scroll.PropertyChanged += Scroll_PropertyChanged;
    }

    public TerminalCompletor Completor
    {
        get => _completor;
        set => _setter.SetField(ref _completor, value, nameof(Completor));
    }

    public TerminalColorType? ForegroundColor
    {
        get => _foregroundColor;
        set => _setter.SetField(ref _foregroundColor, value, nameof(ForegroundColor));
    }

    public TerminalColorType? BackgroundColor
    {
        get => _backgroundColor;
        set => _setter.SetField(ref _backgroundColor, value, nameof(BackgroundColor));
    }

    public bool IsExecuting => _isExecuting;

    public bool IsReadOnly
    {
        get => _isReadOnly;
        set => _setter.SetField(ref _isReadOnly, value, nameof(IsReadOnly));
    }

    public int CursorPosition
    {
        get => _cursorPosition;
        set
        {
            if (value < 0 || value > _command.Length)
                throw new ArgumentOutOfRangeException(nameof(value));

            if (_cursorPosition != value)
            {
                using var _ = _setter.LockEvent();
                _setter.SetField(ref _cursorPosition, value, nameof(CursorPosition));
                _inputText = _command.Substring(0, _cursorPosition);
                UpdateCursorCoordinate();
            }
        }
    }

    public string Prompt
    {
        get => _prompt;
        set
        {
            if (_prompt != value)
            {
                using var _ = _setter.LockEvent();
                _setter.SetField(ref _prompt, value, nameof(Prompt));
                _setter.SetField(ref _cursorPosition, _command.Length, nameof(CursorPosition));
                _block.Command = _command;
                UpdateCursorCoordinate();
            }
        }
    }

    public string Command
    {
        get => _command;
        set
        {
            if (_command != value)
            {
                using var _ = _setter.LockEvent();
                _setter.SetField(ref _command, value, nameof(Command));
                _setter.SetField(ref _cursorPosition, _command.Length, nameof(CursorPosition));
                _block.Command = _command;
                _inputText = _command;
                _completion = string.Empty;
                UpdateCursorCoordinate();
            }
        }
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

    public TerminalSize Size => _size;

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
                // _outputBlock.Take(index.Value);
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
                _view.Update(_blocks);
            }
        }
    }

    public ITerminalScroll Scroll { get; }

    public Terminals.TerminalSelection Selecting
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

    public string CompositionString
    {
        get => _compositionString;
        set => _setter.SetField(ref _compositionString, value, nameof(CompositionString));
    }

    public ITerminalSelection Selections => _selections;

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
        // if (position.X < 0 || position.Y < 0)
        //     return TerminalCoord.Invalid;
        var x = (int)(position.X / itemWidth);
        var y = (int)(position.Y / itemHeight);
        // if (x >= BufferSize.Width || y >= _blocks.Height)
        //     return TerminalCoord.Invalid;
        return new TerminalCoord(x, y);
    }

    public TerminalCharacterInfo? GetInfo(TerminalCoord coord)
    {
        return _blocks.GetInfo(coord.X, coord.Y);
    }

    public bool BringIntoView(int y)
    {
        var scroll = Scroll;
        var topIndex = _originCoordinate.Y;
        var bottomIndex = topIndex + BufferSize.Height;
        if (scroll.Value < _blocks.Height - BufferSize.Height)
        {
            var value = _blocks.Height - BufferSize.Height;
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
    {
        if (Selections.Any() == true)
        {
            var range = Selections.First();
            return GetString(range);
        }
        else
        {
            return string.Empty;
        }
    }

    public void Paste(string text) => ProcessText(text);

    public void ProcessText(string text)
    {
        var itemList = new List<char>(text.Length);
        var terminal = this;
        foreach (var item in text)
        {
            if (item == 0 && itemList.Count > 0)
            {
                terminal.InsertCharacter([.. itemList]);
                itemList.Clear();
            }
            if (terminal.IsReadOnly == false && terminal.IsExecuting == false && item != 0 && OnKeyPress(item) == false)
            {
                if (item == '\n')
                {
                    var items = itemList.ToArray();
                    itemList.Clear();
                    terminal.InsertCharacter(items);
                    terminal.Execute();
                }
                else
                {
                    itemList.Add(item);
                }
            }
        }
        if (itemList.Count > 0)
            terminal.InsertCharacter([.. itemList]);

        static bool OnKeyPress(char character)
        {
            if (character == '\t' || character == 27 || character == 25)
                return true;
            return false;
        }
    }

    public void ResizeBuffer(double width, double height)
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
            _setter.SetField(ref _bufferSize, bufferSize, nameof(BufferSize));
            _blocks.Update();
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
        _view.Update(_blocks);
        UpdateCursorCoordinate();
    }

    public void Update(params ITerminalRow[] rows) => InvokeUpdatedEvent(rows);

    public void Execute()
    {
        if (_isReadOnly == true)
            throw new InvalidOperationException("Terminal is readonly.");
        if (_isExecuting == true)
            throw new InvalidOperationException("Terminal is being executed.");

        var commandText = _command;
        var prompt = _prompt;
        if (_historyList.Contains(commandText) == false)
        {
            _historyList.Add(commandText);
            _historyIndex = _historyList.Count;
        }
        else
        {
            _historyIndex = _historyList.LastIndexOf(commandText) + 1;
        }

        using (var _ = _setter.LockEvent())
        {
            _setter.SetField(ref _command, string.Empty, nameof(Command));
            _setter.SetField(ref _cursorPosition, 0, nameof(CursorPosition));
            _block.Command = _command;
            _inputText = string.Empty;
            _completion = string.Empty;
        }
        ExecuteEvent(commandText, prompt);
    }

    public void Clear()
    {
        using var _ = _setter.LockEvent();
        _setter.SetField(ref _command, string.Empty, nameof(Command));
        _setter.SetField(ref _cursorPosition, 0, nameof(CursorPosition));
        _block.Command = _command;
        _blocks.Clear();
        _outputBlock = _blocks.CurrentBlock;
        _inputText = string.Empty;
        _completion = string.Empty;
    }

    public void Cancel()
    {
        OnCancellationRequested(EventArgs.Empty);
    }

    public void Reset(TerminalCoord coord)
    {
        using var _ = _setter.LockEvent();
        _setter.SetField(ref _command, string.Empty, nameof(Command));
        _setter.SetField(ref _cursorPosition, 0, nameof(CursorPosition));
        _block.Command = _command;
        _blocks.Clear();
        _outputBlock = _blocks.CurrentBlock;
        _inputText = string.Empty;
        _completion = string.Empty;
    }

    public void Append(string text)
    {
        var displayInfo = new TerminalDisplayInfo()
        {
            Foreground = _foregroundColor,
            Background = _backgroundColor,
        };
        _outputBlock.Append(text, displayInfo);
        UpdateCursorCoordinate();
        BringIntoView(_cursorCoordinate.Y);
    }

    public void NextCompletion()
    {
        CompletionImpl(NextCompletion);
    }

    public void PrevCompletion()
    {
        CompletionImpl(PrevCompletion);
    }

    public void Delete()
    {
        if (_cursorPosition < _command.Length)
        {
            using var _ = _setter.LockEvent();
            _setter.SetField(ref _command, _command.Remove(_cursorPosition, 1), nameof(Command));
            _block.Command = _command;
            _inputText = _command;
        }
    }

    public void Backspace()
    {
        if (_cursorPosition > 0)
        {
            var length = 1;
            using var _ = _setter.LockEvent();
            _setter.SetField(ref _command, _command.Remove(_cursorPosition - length, length), nameof(Command));
            _setter.SetField(ref _cursorPosition, _cursorPosition - length, nameof(CursorPosition));
            _block.Command = _command;
            _inputText = _command;
            UpdateCursorCoordinate();
        }
    }

    public void NextHistory()
    {
        if (_historyIndex + 1 < _historyList.Count)
        {
            _historyIndex++;
            Command = _historyList[_historyIndex];
        }
    }

    public void PrevHistory()
    {
        if (_historyIndex > 0)
        {
            _historyIndex--;
            Command = _historyList[_historyIndex];
        }
        else if (_historyList.Count == 1)
        {
            _historyIndex = 0;
            Command = _historyList[_historyIndex];
        }
    }

    public static Match[] MatchCompletion(string text)
    {
        var matches = Regex.Matches(text, "\\S+");
        var argList = new List<Match>(matches.Count);
        for (var i = 0; i < matches.Count; i++)
        {
            argList.Add(matches[i]);
        }
        return [.. argList];
    }

    public static string NextCompletion(string[] completions, string text)
    {
        completions = [.. completions];
        if (completions.Contains(text) == true)
        {
            for (var i = 0; i < completions.Length; i++)
            {
                var r = string.Compare(text, completions[i], true);
                if (r == 0)
                {
                    if (i + 1 < completions.Length)
                        return completions[i + 1];
                    else
                        return completions.First();
                }
            }
        }
        else
        {
            for (var i = 0; i < completions.Length; i++)
            {
                var r = string.Compare(text, completions[i], true);
                if (r < 0)
                {
                    return completions[i];
                }
            }
        }
        return text;
    }

    public static string PrevCompletion(string[] completions, string text)
    {
        completions = [.. completions];
        if (completions.Contains(text) == true)
        {
            for (var i = completions.Length - 1; i >= 0; i--)
            {
                var r = string.Compare(text, completions[i], true);
                if (r == 0)
                {
                    if (i - 1 >= 0)
                        return completions[i - 1];
                    else
                        return completions.Last();
                }
            }
        }
        else
        {
            for (var i = completions.Length - 1; i >= 0; i--)
            {
                var r = string.Compare(text, completions[i], true);
                if (r < 0)
                {
                    return completions[i];
                }
            }
        }
        return text;
    }

    public void ResetColor()
    {
        using var _ = _setter.LockEvent();
        _setter.SetField(ref _foregroundColor, null, nameof(ForegroundColor));
        _setter.SetField(ref _backgroundColor, null, nameof(BackgroundColor));
    }

    public TerminalDataInfo Save()
    {
        var data = new TerminalDataInfo
        {
            Prompt = _prompt,
            Command = _command,
            InputText = _inputText,
            Completion = _completion,
            CursorPosition = _cursorPosition,
            Histories = [.. _historyList],
            HistoryIndex = _historyIndex,
            PromptDisplayInfo = new TerminalDisplayInfo[_prompt.Length],
            CommandDisplayInfo = new TerminalDisplayInfo[_command.Length],
        };

        return data;
    }

    public void Load(TerminalDataInfo data)
    {
        using var _ = _setter.LockEvent();
        _setter.SetField(ref _command, data.Command, nameof(Command));
        _setter.SetField(ref _prompt, data.Prompt, nameof(Prompt));
        _setter.SetField(ref _cursorPosition, data.CursorPosition, nameof(CursorPosition));
        _inputText = data.InputText;
        _completion = data.Completion;
        _historyList.Clear();
        _historyList.AddRange(data.Histories);
        _historyIndex = data.HistoryIndex;
    }

    public void InsertCharacter(params char[] characters)
    {
        if (_isReadOnly == true)
            throw new InvalidOperationException();

        if (characters.Length != 0)
        {
            var text = new string(characters);
            using var _ = _setter.LockEvent();
            _setter.SetField(ref _command, _command.Insert(_cursorPosition, text), nameof(Command));
            _setter.SetField(ref _cursorPosition, _cursorPosition + text.Length, nameof(CursorPosition));
            _block.Command = _command;
            _inputText = _command;
            _completion = string.Empty;
            UpdateCursorCoordinate();
            BringIntoView(_cursorCoordinate.Y);
        }
    }

    public event EventHandler<TerminalExecutingEventArgs>? Executing
    {
        add { _executing += value; }
        remove { _executing -= value; }
    }

    public event EventHandler<TerminalExecutedEventArgs>? Executed
    {
        add { _executed += value; }
        remove { _executed -= value; }
    }

    public event EventHandler? CancellationRequested;

    public event PropertyChangedEventHandler? PropertyChanged;

    public event EventHandler<TerminalUpdateEventArgs>? Updated;

    protected virtual void OnCancellationRequested(EventArgs e)
    {
        CancellationRequested?.Invoke(this, e);
    }

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }

    protected virtual void OnUpdated(TerminalUpdateEventArgs e)
    {
        Updated?.Invoke(this, e);
    }

    private void UpdateCursorCoordinate()
    {
        var index1 = new TerminalIndex(this, TerminalCoord.Empty);
        // var index2 = index1.MoveForward(_block, _cursorPosition + _prompt.Length);
        var index2 = _block._index + _cursorPosition;
        _setter.SetField(ref _cursorCoordinate, _block.GetCoordinate(index2), nameof(CursorCoordinate));
    }

    private void InvokePropertyChangedEvent(string propertyName)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

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

    private void Blocks_Updated(object? sender, EventArgs e)
    {
        Scroll.PropertyChanged -= Scroll_PropertyChanged;
        Scroll.Minimum = 0;
        Scroll.Maximum = GetScrollMaximum();
        Scroll.IsVisible = Scroll.Maximum > 0;
        Scroll.PropertyChanged += Scroll_PropertyChanged;
        _view.Update(_blocks);
        UpdateCursorCoordinate();
    }

    private void View_Updated(object? sender, TerminalRowUpdateEventArgs e)
    {
        InvokeUpdatedEvent(e.ChangedRows);
    }

    private void Scroll_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _view.Update(_blocks);
    }

    private string GetString(Terminals.TerminalSelection selection)
    {
        return string.Empty;
        // var bufferWidth = BufferSize.Width;
        // var c1 = selection.BeginCoord;
        // var c2 = selection.EndCoord;
        // var (s1, s2) = c1 < c2 ? (c1, c2) : (c2, c1);
        // var capacity = (s2.Y - s1.Y + 1) * bufferWidth;
        // var list = new List<char>(capacity);
        // var sb = new StringBuilder();
        // var x = s1.X;
        // for (var y = s1.Y; y <= s2.Y; y++)
        // {
        //     var i = 0;
        //     var count = y == s2.Y ? s2.X : bufferWidth;
        //     for (; x < count; x++)
        //     {
        //         var coord = new TerminalCoord(x, y);
        //         var index = CoordinateToIndex(coord);
        //         if (index >= 0)
        //         {
        //             var glyphInfo = _characterInfos[index];
        //             var character = glyphInfo.Character;
        //             if (character != char.MinValue)
        //             {
        //                 sb.Append(character);
        //                 i++;
        //             }
        //         }
        //     }
        //     x = 0;
        //     if (i == 0 && y > s1.Y)
        //         sb.AppendLine();
        // }
        // return sb.ToString();
    }

    private int GetScrollMaximum()
    {
        if (ActualStyle.IsScrollForwardEnabled == false)
        {
            return Math.Max(_originCoordinate.Y, _blocks.Height - BufferSize.Height);
        }
        return Math.Max(_blocks.Height, _maximumBufferHeight) - BufferSize.Height;
    }

    private void InvokeUpdatedEvent(ITerminalRow[] rows) => OnUpdated(new(rows));

    private void CompletionImpl(Func<string[], string, string> func)
    {
#if NETFRAMEWORK
        var matchCompletions = CommandUtility.MatchCompletion(_inputText);
        var matches = new List<Match>(matchCompletions.Count);
        foreach (Match item in matchCompletions)
        {
            matches.Add(item);
        }
#else
        var matches = new List<Match>(CommandUtility.MatchCompletion(_inputText));
#endif // NETFRAMEWORK
        var find = string.Empty;
        var prefix = false;
        var postfix = false;
        var leftText = _inputText;
        if (matches.Count > 0)
        {
            var match = matches.Last();
            var matchText = match.Value;
            if (matchText.Length > 0 && matchText.First() == '\"')
            {
                prefix = true;
                matchText = matchText.Substring(1);
            }
            if (matchText.Length > 1 && matchText.Last() == '\"')
            {
                postfix = true;
                matchText = matchText.Remove(matchText.Length - 1);
            }
            if (matchText == matchText.Trim())
            {
                find = matchText;
                matches.RemoveAt(matches.Count - 1);
                leftText = _inputText.Remove(match.Index);
            }
        }

        var argList = new List<string>();
        for (var i = 0; i < matches.Count; i++)
        {
            var matchText = matches[i].Value.Trim();
            if (matchText != string.Empty)
                argList.Add(matchText);
        }

        var completions = Completor([.. argList], find);
        if (completions != null && completions.Length != 0)
        {
            var completion = func(completions, _completion);
            var inputText = _inputText;
            var command = leftText + completion;
            if (prefix == true || postfix == true)
            {
                command = leftText + "\"" + completion + "\"";
            }
            Command = command;
            _inputText = inputText;
            _completion = completion;
        }
    }

    private void InsertPrompt(string prompt)
    {
        using var _ = _setter.LockEvent();
        _outputBlock.Take();
        _setter.SetField(ref _isExecuting, false, nameof(IsExecuting));
        _setter.SetField(ref _prompt, prompt, nameof(Prompt));
        _setter.SetField(ref _cursorPosition, 0, nameof(CursorPosition));
        _block.Command = _command;
        _inputText = string.Empty;
        _completion = string.Empty;
        UpdateCursorCoordinate();
        BringIntoView(_cursorCoordinate.Y);
    }

    private void ExecuteEvent(string commandText, string prompt)
    {
        var action = new Action<Exception?>((e) =>
        {
            InsertPrompt(_prompt != string.Empty ? _prompt : prompt);
            _executed?.Invoke(this, new TerminalExecutedEventArgs(commandText, e));
        });
        var eventArgs = new TerminalExecutingEventArgs(commandText, action);
        InvokePropertyChangedEvent(nameof(IsExecuting));
        _isExecuting = true;
        _executing?.Invoke(this, eventArgs);
        if (eventArgs.Token == Guid.Empty)
        {
            action(null);
        }
    }

    #region ITerminal

    ITerminalScroll ITerminal.Scroll => Scroll;

    #endregion
}
