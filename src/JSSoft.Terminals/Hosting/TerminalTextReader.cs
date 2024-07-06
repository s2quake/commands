// <copyright file="TerminalTextReader.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Terminals.Hosting;

sealed class TerminalTextReader : TextReader
{
    private readonly StringBuilder _sb = new();
    private readonly ManualResetEvent _manualResetEvent = new(initialState: false);

    public override int Read()
    {
        if (_sb.Length == 0)
        {
            _manualResetEvent.Reset();
            return -1;
        }
        var value = _sb[0];
        _sb.Remove(0, 1);
        return value;
    }

    public override async Task<int> ReadAsync(char[] buffer, int index, int count)
    {
        await Task.Run(_manualResetEvent.WaitOne);
        return await base.ReadAsync(buffer, index, count);
    }

    public void Write(char value)
    {
        _sb.Append(value);
        _manualResetEvent.Set();
    }

    public void Write(string? value)
    {
        _sb.Append(value);
        _manualResetEvent.Set();
    }
}
