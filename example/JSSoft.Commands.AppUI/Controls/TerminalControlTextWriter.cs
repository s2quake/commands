// <copyright file="TerminalControlTextWriter.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace JSSoft.Commands.AppUI.Controls;

internal sealed class TerminalControlTextWriter(TerminalControl terminalControl) : TextWriter
{
    private readonly TerminalControl _terminalControl = terminalControl;

    public override Encoding Encoding => Encoding.UTF8;

    public override void Write(char value)
    {
        Dispatcher.UIThread.Invoke(() => _terminalControl.Out.Write(value));
    }

    public override void Write(string? value)
    {
        Dispatcher.UIThread.Invoke(() => _terminalControl.Out.Write(value));
    }

    public override void WriteLine(string? value)
    {
        Dispatcher.UIThread.Invoke(() => _terminalControl.Out.WriteLine(value));
    }

    public override async Task WriteAsync(char value)
    {
        await Dispatcher.UIThread.InvokeAsync(() => _terminalControl.Out.Write(value));
    }

    public override async Task WriteAsync(string? value)
    {
        await Dispatcher.UIThread.InvokeAsync(() => _terminalControl.Out.Write(value));
    }

    public override async Task WriteLineAsync(string? value)
    {
        await Dispatcher.UIThread.InvokeAsync(() => _terminalControl.Out.WriteLine(value));
    }
}
