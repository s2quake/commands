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
using JSSoft.Terminals.Pty;
using System.Threading;
using System.Text;
using System.Diagnostics;
using Avalonia.Threading;
using JSSoft.Commands.AppUI.Controls;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Avalonia;

namespace JSSoft.Commands.AppUI;

public sealed class PseudoTerminal(TerminalControl terminalControl)
{
    private readonly TerminalControl _terminalControl = terminalControl;
    private IPtyConnection? _pty;
    private CancellationTokenSource? _cancellationTokenSource;
    private string _app = string.Empty;
    private Size _size = new(80, 24);

    public string App
    {
        get => _app;
        set => _app = value;
    }

    public Size Size
    {
        get => _size;
        set
        {
            if (_size != value)
            {
                _size = value;
                _pty?.Resize((int)value.Width, (int)value.Height);
            }
        }
    }

    public void Open()
    {
        var app = _app;
        var size = _size;
        var options = new PtyOptions
        {
            Width = (int)size.Width,
            Height = (int)size.Height,
            WorkingDirectory = Environment.CurrentDirectory,
            App = app,
            EnvironmentVariables = new Dictionary<string, string>()
            {
                { "LANG", "en_US.UTF-8" }
            },
        };

        _pty = PtyProvider.Spawn(options);
        _cancellationTokenSource = new();
        _pty.Exited += Pty_Exited;
        ReadInput(_pty, _terminalControl, _cancellationTokenSource.Token);
        ReadStream(_pty, Append, _cancellationTokenSource.Token);
    }

    public void Close()
    {
        if (_pty is null || _cancellationTokenSource is null)
            throw new InvalidOperationException();

        _pty.Exited -= Pty_Exited;
        _cancellationTokenSource.Cancel();
        _pty.Dispose();
        _cancellationTokenSource.Dispose();
    }

    public bool IsOpen { get; private set; }

    public event EventHandler? Exited;

    private static async void ReadInput(IPtyConnection pty, TerminalControl control, CancellationToken cancellationToken)
    {
        var buffer = new char[4096];
        try
        {
            while (cancellationToken.IsCancellationRequested != true)
            {
                var count = await control.In.ReadAsync(buffer, cancellationToken);
                if (count == 0)
                {
                    await Task.Delay(1, cancellationToken);
                    continue;
                }
                var bytes = Encoding.UTF8.GetBytes(buffer, 0, count);
                await Task.Run(() => pty.Write(bytes, bytes.Length), cancellationToken);
            }
        }
        catch (TaskCanceledException)
        {
        }
    }

    private static async void ReadStream(IPtyConnection pty, Action<string> action, CancellationToken cancellationToken)
    {
        var buffer = new byte[2048];
        try
        {
            var sb = new StringBuilder();
            while (cancellationToken.IsCancellationRequested != true)
            {
                var count = await Task.Run(() => pty.Read(buffer, buffer.Length));
                if (count == 0)
                {
                    Console.WriteLine("ReadStream ended");
                    break;
                }
                // Console.WriteLine($"count: {count}");
                if (count == -1)
                {
                    continue;
                }
                var s = Encoding.UTF8.GetString(buffer, 0, count);
                sb.Append(s.Normalize());
                if (await Task.Run(() => pty.CanRead == true))
                {
                    continue;
                }
                else
                {
#if DEBUG && NET8_0
                    Trace.WriteLine($"Read: {ToLiteral(sb.ToString())}");
#endif
                    action(sb.ToString());
                    sb.Clear();
                }
            }
        }
        catch (TaskCanceledException)
        {
        }
#if DEBUG && NET8_0
        static string ToLiteral(string valueTextForCompiler)
        {
            return Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(valueTextForCompiler, false);
        }
#endif
    }

    private void Pty_Exited(object? sender, PtyExitedEventArgs e)
        => Exited?.Invoke(this, e);

    private void Append(string text)
    {
        Dispatcher.UIThread.Invoke(() => _terminalControl.Out.Write(text));
    }
}
