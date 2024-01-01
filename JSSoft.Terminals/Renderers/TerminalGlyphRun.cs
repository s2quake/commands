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

namespace JSSoft.Terminals.Renderers;

public class TerminalGlyphRun(TerminalGlyphRunInfo info) : IDisposable
{
    private bool _isDisposed;

    public ITerminalFont Font { get; } = info.Font;

    public TerminalColor Color { get; } = info.Color;

    public bool IsBold { get; } = info.IsBold;

    public int Group { get; } = info.Group;

    public TerminalGlyphInfo[] GlyphInfos { get; } = info.GlyphInfos;

    public int[] Spans { get; } = info.Spans;

    public TerminalPoint Position { get; } = info.Position;

    public event EventHandler? Disposed;

    internal void Render(ITerminalDrawingContext drawingContext)
    {
        drawingContext.DrawText(this);
    }

    protected virtual void OnDisposed(EventArgs e)
    {
        Disposed?.Invoke(this, e);
    }

    #region IDisposable

    public void Dispose()
    {
        if (_isDisposed == true)
            throw new ObjectDisposedException($"{this}");

        OnDisposed(EventArgs.Empty);
        _isDisposed = true;
        GC.SuppressFinalize(this);
    }

    #endregion
}
