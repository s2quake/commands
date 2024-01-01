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

namespace JSSoft.Commands.Extensions;

public sealed class TableDataBuilder
{
    private readonly int _columnCount;
    private readonly object[]? _headers;
    private readonly List<string[]> _rows = [];
    private string[][]? _data;

    public TableDataBuilder(int columnCount)
    {
        if (columnCount < 1)
            throw new ArgumentOutOfRangeException(nameof(columnCount), $"'{nameof(columnCount)}' must be 1 or greater.");
        _columnCount = columnCount;
    }

    public TableDataBuilder(object[] headers)
    {
        if (headers.Length < 1)
            throw new ArgumentException($"The length of {nameof(headers)} must be 1 or greater.", nameof(headers));
        _headers = headers;
        _columnCount = headers.Length;
    }

    public void Add(object?[] items)
    {
        if (_columnCount != items.Length)
            throw new ArgumentException($"The length of items must be {_columnCount}.", nameof(items));
        _rows.Add(items.Select(item => item == null ? string.Empty : $"{item:R}".Replace(Environment.NewLine, string.Empty)).ToArray());
    }

    public string[][] Data
    {
        get
        {
            if (_data == null)
            {
                if (_headers != null)
                    _rows.Insert(0, _headers.Select(item => $"{item:R}").ToArray());
                _data = [.. _rows];
            }
            return _data;
        }
    }

    public bool HasHeader => _headers != null;
}
