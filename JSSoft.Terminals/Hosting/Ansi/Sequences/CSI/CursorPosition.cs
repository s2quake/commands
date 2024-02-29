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

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

sealed class CursorPosition : CSISequenceBase
{
    public CursorPosition()
        : base('H')
    {
    }

    public override string DisplayName => "CSI Ps ; Ps H";

    protected override void OnProcess(SequenceContext context)
    {
        var view = context.View;
        var r1 = context.GetParametersAsInteger(index: 0, defaultValue: 1) - 1;
        var c1 = context.GetParametersAsInteger(index: 1, defaultValue: 1) - 1;
        var r2 = TerminalMathUtility.Clamp(r1, 0, view.Height - 1) + view.Y;
        var c2 = TerminalMathUtility.Clamp(c1, 0, view.Width - 1);
        var index = new TerminalIndex(new TerminalCoord(c2, r2), view.Width);
        context.Index = index;
        context.BeginIndex = index;
    }
}
