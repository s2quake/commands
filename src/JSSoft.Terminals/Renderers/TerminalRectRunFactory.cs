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
