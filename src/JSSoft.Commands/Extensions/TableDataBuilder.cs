// <copyright file="TableDataBuilder.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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
