// <copyright file="PseudoTerminal.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Threading;
using JSSoft.Commands.AppUI.Controls;
using JSSoft.Terminals.Pty;

namespace JSSoft.Commands.AppUI;

public sealed class PseudoTerminal(TerminalControl terminalControl)
{
    private readonly TerminalControl _terminalControl = terminalControl;
    private IPtyConnection? _pty;
    private CancellationTokenSource? _cancellationTokenSource;
    private Size _size = new(80, 24);

    public event EventHandler? Exited;

    public string App { get; set; } = string.Empty;

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

    public bool IsOpen { get; }

    public void Open()
    {
        var app = App;
        var size = _size;
        var options = new PtyOptions
        {
            Width = (int)size.Width,
            Height = (int)size.Height,
            WorkingDirectory = Environment.CurrentDirectory,
            App = app,
            EnvironmentVariables = new Dictionary<string, string>()
            {
                { "LANG", "en_US.UTF-8" },
            },
        };

        _pty = PtyProvider.Spawn(options);
        _cancellationTokenSource = new();
        _pty.Exited += Pty_Exited;
        _ = ReadInput(_pty, _terminalControl, _cancellationTokenSource.Token);
        _ = ReadStream(_pty, Append, _cancellationTokenSource.Token);
    }

    public void Close()
    {
        if (_pty is null || _cancellationTokenSource is null)
        {
            throw new InvalidOperationException();
        }

        _pty.Exited -= Pty_Exited;
        _cancellationTokenSource.Cancel();
        _pty.Dispose();
        _cancellationTokenSource.Dispose();
    }

    private static async Task ReadInput(
        IPtyConnection pty, TerminalControl control, CancellationToken cancellationToken)
    {
        var buffer = new char[4096];
        try
        {
            while (cancellationToken.IsCancellationRequested is false)
            {
                var count = await control.In.ReadAsync(buffer, cancellationToken);
                if (count is 0)
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
            // do nothing
        }
    }

    private static async Task ReadStream(
        IPtyConnection pty, Action<string> action, CancellationToken cancellationToken)
    {
        var buffer = new byte[2048];
        try
        {
            var sb = new StringBuilder();
            while (cancellationToken.IsCancellationRequested is false)
            {
                var count = await Task.Run(() => pty.Read(buffer, buffer.Length));
                if (count is 0)
                {
                    Console.WriteLine("ReadStream ended");
                    break;
                }

                if (count == -1)
                {
                    continue;
                }

                var s = Encoding.UTF8.GetString(buffer, 0, count);
                sb.Append(s.Normalize());
                if (await Task.Run(() => pty.CanRead is not true))
                {
#if DEBUG && NET8_0
                    Console.WriteLine($"Read: {ToLiteral(sb.ToString())}");
#endif
                    action(sb.ToString());
                    sb.Clear();
                }
            }
        }
        catch (TaskCanceledException)
        {
            // do nothing
        }

#if DEBUG && NET8_0
        static string ToLiteral(string valueTextForCompiler)
        {
            return Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(
                valueTextForCompiler, false);
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
