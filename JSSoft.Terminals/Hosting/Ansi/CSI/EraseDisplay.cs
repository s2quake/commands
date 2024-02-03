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

using JSSoft.Terminals.Hosting.Ansi.CSI;

namespace JSSoft.Terminals.Hosting.Ansi.EraseFunctions;

/// <summary>
/// https://terminalguide.namepad.de/seq/csi_cj/
/// </summary>
sealed class EraseDisplay : CSISequenceBase
{
    private static readonly Action<TerminalLineCollection, EscapeSequenceContext> EmptyAction = (items, context) => { };

    public EraseDisplay()
        : base('J')
    {
    }

    protected override void OnProcess(TerminalLineCollection lines, EscapeSequenceContext context)
    {
        var option = context.GetOptionValue(index: 0) ?? 0;
        var action = GetAction(option);
        action.Invoke(lines, context);
    }

    // erase from cursor until end of screen
    private void Action0(TerminalLineCollection lines, EscapeSequenceContext context)
    {
        var view = context.View;
        var index0 = context.Index;
        var index1 = new TerminalIndex(x: view.Right - 1, y: view.Bottom - 1, view.Width);
        var length = Math.Min(lines.Count, index1.Value) - index0.X;
        lines.Erase(index0, length);
    }

    // erase from cursor to beginning of screen
    private void Action1(TerminalLineCollection lines, EscapeSequenceContext context)
    {
        var view = context.View;
        var index0 = new TerminalIndex(x: view.Left, y: view.Top, view.Width);
        var index1 = context.Index;
        var length = index1 - index0;
        lines.Erase(index0, length);
    }

    // erase entire screen
    private void Action2(TerminalLineCollection lines, EscapeSequenceContext context)
    {
        var view = context.View;
        var index0 = new TerminalIndex(x: view.Left, y: view.Top, view.Width);
        var index1 = new TerminalIndex(x: view.Right - 1, y: view.Bottom - 1, view.Width);
        var length = Math.Min(lines.Count, index1.Value) - index0.Value;
        lines.Erase(index0, length);
    }

    // erase saved lines
    private void Action3(TerminalLineCollection lines, EscapeSequenceContext context)
    {

    }

    private Action<TerminalLineCollection, EscapeSequenceContext> GetAction(int option) => option switch
    {
        0 => Action0,
        1 => Action1,
        2 => Action2,
        3 => Action3,
        _ => EmptyAction,
    };
}
