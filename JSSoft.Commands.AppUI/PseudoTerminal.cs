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
using System.Collections.Generic;
using JSSoft.Terminals;
using JSSoft.Terminals.Pty;
using System.Threading;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using Avalonia.Threading;
using JSSoft.Commands.AppUI.Controls;
using System.Threading.Tasks;

namespace JSSoft.Commands.AppUI;

public sealed class PseudoTerminal
{
    private readonly TerminalControl _terminalControl;
    private IPtyConnection? _pty;
    private CancellationTokenSource? _cancellationTokenSource;
    private string _app = TerminalEnvironment.IsWindows() == true ? Path.Combine(Environment.SystemDirectory, "cmd.exe") : "sh";
    private TerminalSize _size = new(80, 24);

    public PseudoTerminal(TerminalControl terminalControl)
    {
        _terminalControl = terminalControl;
    }

    public string App
    {
        get => _app;
        set => _app = value;
    }

    public TerminalSize Size
    {
        get => _size;
        set => _size = value;
    }

    public async Task OpenAsync(CancellationToken cancellationToken)
    {
        var app = _app;
        var size = _size;
        var options = new PtyOptions
        {
            Name = "Custom terminal",
            Cols = size.Width,
            Rows = size.Height,
            Cwd = Environment.CurrentDirectory,
            App = app,
            Environment = new Dictionary<string, string>()
            {
            },
        };

        _pty = await PtyProvider.SpawnAsync(options, cancellationToken);
        _cancellationTokenSource = new();
        _terminalControl.Executing += TerminalControl_Executing;
        ReadStream(_pty, Append, _cancellationTokenSource.Token);
    }

    public async Task CloseAsync(CancellationToken cancellationToken)
    {
        if (_pty is null || _cancellationTokenSource is null)
            throw new InvalidOperationException();

        _terminalControl.Executing -= TerminalControl_Executing;
        _cancellationTokenSource.Cancel();
        // _pty.Kill();
        // _pty.WaitForExit(milliseconds: 1000);
        // _pty.Dispose();
        _cancellationTokenSource.Dispose();
    }

    public bool IsOpen { get; private set; }

    private static async void ReadStream(IPtyConnection pty, Action<string> action, CancellationToken cancellationToken)
    {
        var buffer = new byte[4096];
        try
        {
            while (cancellationToken.IsCancellationRequested != true)
            {
                var count = await pty.ReaderStream.ReadAsync(buffer, cancellationToken);
                if (count == 0)
                {
                    break;
                }
                var t = Encoding.UTF8.GetString(buffer, 0, count);
                Trace.WriteLine(Regex.Escape(t));
                action(t);
            }
        }
        catch (TaskCanceledException)
        {
        }
    }

    private void Append(string text)
    {
        Dispatcher.UIThread.Invoke(() => _terminalControl.Append(text));
    }

    private async void TerminalControl_Executing(object? sender, TerminalExecutingRoutedEventArgs e)
    {
        if (_pty is null)
            throw new InvalidOperationException();

        var token = e.GetToken();
        var commandBuffer = Encoding.UTF8.GetBytes(e.Command + "\n");
        await _pty.WriterStream.WriteAsync(commandBuffer, cancellationToken: default);
        await _pty.WriterStream.FlushAsync();
        e.Success(token);
    }
}
