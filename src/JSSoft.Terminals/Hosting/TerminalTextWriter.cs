// <copyright file="TerminalTextWriter.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Terminals.Hosting;

internal sealed class TerminalTextWriter(Terminal terminal) : TextWriter
{
    private readonly Terminal _terminal = terminal;

    public override Encoding Encoding => Encoding.UTF8;

    public override void Write(char value)
    {
        _terminal.SynchronizationContext.Send((state) =>
        {
            _terminal.Append($"{value}");
        }, state: null);
    }

    public override void Write(string? value)
    {
        _terminal.SynchronizationContext.Send((state) =>
        {
            _terminal.Append($"{value}");
        }, state: null);
    }

    public override void WriteLine(string? value)
    {
        _terminal.SynchronizationContext.Send((state) =>
        {
            _terminal.Append($"{value}{Environment.NewLine}");
        }, state: null);
    }

    public override Task WriteAsync(char value)
    {
        return Task.Run(() => _terminal.SynchronizationContext.Send((state) =>
        {
            _terminal.Append($"{value}");
        }, state: null));
    }

    public override Task WriteAsync(string? value)
    {
        return Task.Run(() => _terminal.SynchronizationContext.Send((state) =>
        {
            _terminal.Append($"{value}");
        }, state: null));
    }

    public override Task WriteLineAsync(string? value)
    {
        return Task.Run(() => _terminal.SynchronizationContext.Send((state) =>
        {
            _terminal.Append($"{value}{Environment.NewLine}");
        }, state: null));
    }
}
