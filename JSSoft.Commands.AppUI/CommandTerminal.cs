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
using JSSoft.Terminals;
using JSSoft.Commands.Extensions;
using JSSoft.Commands.AppUI.Controls;

namespace JSSoft.Commands.AppUI;

sealed class CommandTerminal : IDisposable
{
    private readonly CommandContext _commandContext;
    private readonly TerminalControl _terminalControl;
    private string _prompt = "terminal $ ";

    public CommandTerminal(CommandContext commandContext, TerminalControl terminalControl)
    {
        _commandContext = commandContext;
        _terminalControl = terminalControl;
        _commandContext.Out = new TerminalControlTextWriter(_terminalControl);
        _commandContext.Error = new TerminalControlTextWriter(_terminalControl);
        _terminalControl.Completor = _commandContext.GetCompletion;
        _terminalControl.Out.Write(_prompt);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    private async void Terminal_Executing(object? sender, TerminalExecutingRoutedEventArgs e)
    {
        var token = e.GetToken();
        var cancellationTokenSource = new CancellationTokenSource();
        var progress = new TerminalProgress(_terminalControl);
        try
        {
            _terminalControl.Out.WriteLine(e.Command);
            await _commandContext.ExecuteAsync(e.Command, cancellationTokenSource.Token, progress);
            e.Success(token);
            _terminalControl.Out.Write(_prompt);
        }
        catch (Exception exception)
        {
            var message = exception.Message;
            var tsb = new TerminalStringBuilder
            {
                Foreground = TerminalColorType.BrightRed
            };
            tsb.Append(message);
            tsb.AppendEnd();
            _commandContext.Error.WriteLine(tsb.ToString());
            e.Fail(token, exception);
            _terminalControl.Out.Write(_prompt);
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
