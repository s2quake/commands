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

namespace JSSoft.Terminals.Renderers.Extensions;

public static class ITerminalDrawingContextExtensions
{
    public static IDisposable PushTransform(this ITerminalDrawingContext @this, int x, int y)
    {
        return @this.PushTransform(new TerminalPoint(x, y));
    }

    // internal static TerminalRenderRange PushTransform(this ITerminalDrawingContext @this, ITerminal terminal)
    // {
    //     var (_, bufferHeight) = terminal.BufferSize;
    //     var topIndex = 0;
    //     var bottomIndex = bufferHeight;
    //     var transform = terminal.GetTransform(topIndex);
    //     var transformScope = @this.PushTransform(0.0, -transform.Y);
    //     return new TerminalRenderRange(topIndex, bottomIndex, transformScope);
    // }
}
