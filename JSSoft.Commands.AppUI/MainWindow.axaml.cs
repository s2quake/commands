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

using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using JSSoft.Commands.AppUI.Controls;
using JSSoft.Commands.Extensions;
using JSSoft.Terminals;

namespace JSSoft.Commands.AppUI;

public partial class MainWindow : Window
{
    private readonly CommandContext _commandContext;
    private readonly string _originTitle;

    public MainWindow()
    {
        InitializeComponent();
        App.Current.RegisterService(_terminal);
        _commandContext = App.Current.GetService<CommandContext>()!;
        _commandContext.Owner = this;
        _commandContext.Out = new TerminalControlTextWriter(_terminal);
        _commandContext.Error = new TerminalControlTextWriter(_terminal);
        _terminal.Completor = _commandContext.GetCompletion;
        _terminal.PropertyChanged += Terminal_PropertyChanged;
        _originTitle = $"{Title}";
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        _terminal.Focus();
        _terminal.Executing += Terminal_Executing;
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        _terminal.Executing -= Terminal_Executing;
        base.OnUnloaded(e);
    }

    private void Terminal_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == TerminalControl.BufferSizeProperty)
        {
            Title = $"{_originTitle} — {(int)_terminal.BufferSize.Width}x{(int)_terminal.BufferSize.Height}";
        }
    }

    private async void Terminal_Executing(object? sender, TerminalExecutingRoutedEventArgs e)
    {
        var token = e.GetToken();
        var cancellationTokenSource = new CancellationTokenSource();
        var progress = new TerminalProgress(_terminal);
        try
        {
            await _commandContext.ExecuteAsync(e.Command, cancellationTokenSource.Token, progress);
            e.Success(token);
        }
        catch (Exception exception)
        {
            e.Fail(token, exception);
            _commandContext.Error.WriteLine(TerminalStringBuilder.GetString(exception.Message, TerminalColorType.BrightRed));
        }
    }

    #region TerminalProgress

    sealed class TerminalProgress(TerminalControl terminal) : IProgress<ProgressInfo>
    {
        private readonly TerminalControl _terminal = terminal;

        public async void Report(ProgressInfo value)
        {
            var isCompleted = value.Value == double.MaxValue;
            var progressValue = TerminalMathUtility.Clamp(value.Value, 0, 1);
            var progressText = GenerateText(progressValue, value.Text);
            await Task.CompletedTask;
            // if (isCompleted == true)
            // {
            //     await Dispatcher.UIThread.InvokeAsync(() =>
            //     {
            //         _terminal.Progress(string.Empty);
            //         _terminal.AppendLine(progressText);
            //     });
            // }
            // else
            // {
            //     await Dispatcher.UIThread.InvokeAsync(() => _terminal.Progress(progressText));
            // }
        }

        private string GenerateText(double value, string message)
        {
            var width = (int)_terminal.BufferSize.Width;
            var text = $"{message}: ";
            var percent = $"[{(int)(value * 100),3:D}%] ";
            var column = (int)((width - text.Length - percent.Length - 2));
            var w = (int)(column * value);
            var progress = "#".PadRight(w, '#').PadRight(column, ' ');
            return $"{text}{percent}[{progress}]";
        }
    }

    #endregion
}
