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

using JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

/// <summary>
/// https://terminalguide.namepad.de/seq/csi_ce/
/// </summary>
sealed class CursorNextLine : CSISequenceBase
{
    public CursorNextLine()
        : base('E')
    {
    }

    protected override void OnProcess(TerminalLineCollection lines, SequenceContext context)
    {
        var view = context.View;
        var index = context.Index;
        var value = context.GetOptionValue(index: 0) ?? 1;
        var count = Math.Max(1, value);
        index = index.CursorDown(count, view.Bottom).MoveToFirstOfLine();
        context.Index = index;
    }
}
