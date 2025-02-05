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
        {
            var message = $"'{nameof(columnCount)}' must be 1 or greater.";
            throw new ArgumentOutOfRangeException(nameof(columnCount), message);
        }

        _columnCount = columnCount;
    }

    public TableDataBuilder(object[] headers)
    {
        if (headers.Length < 1)
        {
            var message = $"The length of {nameof(headers)} must be 1 or greater.";
            throw new ArgumentException(message, nameof(headers));
        }

        _headers = headers;
        _columnCount = headers.Length;
    }

    public string[][] Data
    {
        get
        {
            if (_data is null)
            {
                if (_headers is not null)
                {
                    _rows.Insert(0, [.. _headers.Select(item => $"{item:R}")]);
                }

                _data = [.. _rows];
            }

            return _data;
        }
    }

    public bool HasHeader => _headers is not null;

    public void Add(object?[] items)
    {
        if (_columnCount != items.Length)
        {
            var message = $"The length of items must be {_columnCount}.";
            throw new ArgumentException(message, nameof(items));
        }

        _rows.Add(items.Select(ToString).ToArray());
    }

    private static string ToString(object? item)
        => item is null ? string.Empty : $"{item:R}".Replace(Environment.NewLine, string.Empty);
}
