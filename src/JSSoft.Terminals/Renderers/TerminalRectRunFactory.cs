// <copyright file="TerminalRectRunFactory.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using JSSoft.Terminals.Extensions;

namespace JSSoft.Terminals.Renderers;

public class TerminalRectRunFactory<T>(int capacity, Func<TerminalRectRunInfo, T> creator) where T : ITerminalRenderer
{
    private readonly Func<TerminalRectRunInfo, T> _creator = creator;
    private readonly List<TerminalRect> _rectList = new(capacity);
    private readonly List<T> _itemList = [];

    private TerminalColor _backgroundColor;

    public static T[] Create(ITerminalRow row, Func<TerminalRectRunInfo, T> creator)
    {
        var terminal = row.Terminal;
        var bufferWidth = terminal.BufferSize.Width;
        var transformCoord = new TerminalCoord(0, terminal.Scroll.Value);
        var rectRunFactory = new TerminalRectRunFactory<T>(bufferWidth, creator);
        for (var i = 0; i < bufferWidth; i++)
        {
            var viewCoord = new TerminalCoord(i, row.Index);
            var coord = terminal.ViewToWorld(viewCoord);
            var characterInfo = terminal.GetInfo(coord);
            var span = characterInfo?.Span ?? 1;
            if (span <= 0)
                continue;
            var backgroundColor = terminal.GetBackgroundColor(coord);
            var backgroundRect = terminal.GetBackgroundRect(coord - transformCoord, span);
            rectRunFactory.Add(backgroundRect, backgroundColor);
        }
        return rectRunFactory.ToArray();
    }

    public void Add(TerminalRect rect, TerminalColor backgroundColor)
    {
        if (_backgroundColor != backgroundColor)
        {
            Flush();
        }
        _rectList.Add(rect);
        _backgroundColor = backgroundColor;
    }

    public T[] ToArray()
    {
        Flush();
        return [.. _itemList];
    }

    private void Flush()
    {
        if (_rectList.Count > 0)
        {
            var info = new TerminalRectRunInfo
            {
                Color = _backgroundColor,
                Rect = _rectList.Aggregate((item, next) => item.Union(next))
            };
            _itemList.Add(_creator(info));
            _rectList.Clear();
        }
    }
}
