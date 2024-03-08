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
using JSSoft.Terminals.Renderers.Extensions;

namespace JSSoft.Terminals.Renderers;

public class TerminalRowRenderer(ITerminalRow row) : ITerminalRenderer
{
    private ITerminalRow _row = row;
    private TerminalGlyphRun[]? _glyphRuns;
    private TerminalRectRun[]? _rectRuns;

    public virtual void Reset(ITerminalRow row)
    {
        if (_glyphRuns is not null)
        {
            Array.ForEach(_glyphRuns, item => item.Dispose());
        }
        _row = row;
        _glyphRuns = null;
        _rectRuns = null;
    }

    public virtual void Render(ITerminalDrawingContext drawingContext)
    {
        _rectRuns ??= TerminalRectRunFactory<TerminalRectRun>.Create(_row, item => new TerminalRectRun(item));
        if (_glyphRuns is null)
        {
            var font = _row.Terminal.ActualStyle.Font;
            _glyphRuns = TerminalGlyphRunFactory<TerminalGlyphRun>.Create(_row, item => new TerminalGlyphRun(item));
        }
        if (_rectRuns is not null)
        {
            Array.ForEach(_rectRuns, item => item.Render(drawingContext));
        }
        if (_glyphRuns is not null)
        {
            var transform = _row.Terminal.GetTransform(_row.Index);
            using var _ = drawingContext.PushTransform(0, transform.Y);
            Array.ForEach(_glyphRuns, item => drawingContext.DrawText(item));
        }
    }
}
