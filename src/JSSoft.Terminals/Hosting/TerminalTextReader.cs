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
