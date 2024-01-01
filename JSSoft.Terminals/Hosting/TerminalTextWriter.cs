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

using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Terminals.Hosting;

sealed class TerminalTextWriter(Terminal terminal) : TextWriter
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
