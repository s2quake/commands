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
using JSSoft.Commands.AppUI.Controls;
using JSSoft.Commands.Extensions;
using JSSoft.Terminals;

namespace JSSoft.Commands.AppUI;

public partial class MainWindow : Window
{
    private readonly CommandContext _commandContext;
    private readonly string _originTitle;
    // private string _prompt = "terminal $ ";

    private readonly PseudoTerminal _pseudoTerminal;

    public MainWindow()
    {
        InitializeComponent();
        App.Current.RegisterService(_terminal);
        _commandContext = App.Current.GetService<CommandContext>()!;
        _commandContext.Owner = this;
        _pseudoTerminal = new PseudoTerminal(_terminal);
        // _commandContext.Out = new TerminalControlTextWriter(_terminal);
        // _commandContext.Error = new TerminalControlTextWriter(_terminal);
        // _terminal.Completor = _commandContext.GetCompletion;
        _terminal.PropertyChanged += Terminal_PropertyChanged;
        _originTitle = $"{Title}";
        // _terminal.Append(_prompt);
    }

    protected override async void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        _terminal.IsReadOnly = true;
        _pseudoTerminal.Size = _terminal.BufferSize;
        await _pseudoTerminal.OpenAsync(cancellationToken: default);
        _terminal.IsReadOnly = false;
        _terminal.Focus();
        // _terminal.Executing += Terminal_Executing;
    }

    protected override async void OnUnloaded(RoutedEventArgs e)
    {
        // _terminal.Executing -= Terminal_Executing;
        base.OnUnloaded(e);
        await _pseudoTerminal.CloseAsync(cancellationToken: default);
    }

    private void Terminal_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == TerminalControl.BufferSizeProperty)
        {
            Title = $"{_originTitle} — {(int)_terminal.BufferSize.Width}x{(int)_terminal.BufferSize.Height}";
            Console.WriteLine(Title);
        }
    }

    // private async void Terminal_Executing(object? sender, TerminalExecutingRoutedEventArgs e)
    // {
    //     var token = e.GetToken();
    //     var cancellationTokenSource = new CancellationTokenSource();
    //     var progress = new TerminalProgress(_terminal);
    //     try
    //     {
    //         _terminal.AppendLine(e.Command);
    //         await _commandContext.ExecuteAsync(e.Command, cancellationTokenSource.Token, progress);
    //         e.Success(token);
    //         _terminal.Append(_prompt);
    //     }
    //     catch (Exception exception)
    //     {
    //         var message = exception.Message;
    //         var tsb = new TerminalStringBuilder
    //         {
    //             Foreground = TerminalColorType.BrightRed
    //         };
    //         tsb.Append(message);
    //         tsb.AppendEnd();
    //         _commandContext.Error.WriteLine(tsb.ToString());
    //         e.Fail(token, exception);
    //         _terminal.Append(_prompt);
    //     }
    // }

}
