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

namespace JSSoft.Terminals.Hosting.Ansi.EraseFunctions;

/// <summary>
/// CSI Ps J
/// </summary>
sealed class EraseInDisplay : CSISequenceBase
{
    private static readonly Action<TerminalLineCollection, SequenceContext> EmptyAction = (items, context) => { };

    public EraseInDisplay()
        : base('J')
    {
    }

    protected override void OnProcess(TerminalLineCollection lines, SequenceContext context)
    {
        var option = context.GetNumericValue(index: 0, defaultValue: 0);
        var action = GetAction(option);
        action.Invoke(lines, context);
    }

    private void EraseBelow(TerminalLineCollection lines, SequenceContext context)
    {
        var view = context.View;
        var index0 = context.Index;
        var index1 = new TerminalIndex(x: view.Right - 1, y: view.Bottom - 1, view.Width);
        var length = Math.Min(lines.Count, index1.Value) - index0.X;
        lines.Erase(index0, length);
    }

    private void EraseAbove(TerminalLineCollection lines, SequenceContext context)
    {
        var view = context.View;
        var index0 = new TerminalIndex(x: view.Left, y: view.Top, view.Width);
        var index1 = context.Index;
        var length = index1 - index0;
        lines.Erase(index0, length);
    }

    private void EraseAll(TerminalLineCollection lines, SequenceContext context)
    {
        var view = context.View;
        var index0 = new TerminalIndex(x: view.Left, y: view.Top, view.Width);
        var index1 = new TerminalIndex(x: view.Right, y: view.Bottom - 1, view.Width);
        var length = index1.Value - index0.Value;
        lines.Erase(index0, length);
    }

    private void EraseSavedLines(TerminalLineCollection lines, SequenceContext context)
    {
    }

    private Action<TerminalLineCollection, SequenceContext> GetAction(int option) => option switch
    {
        0 => EraseBelow,
        1 => EraseAbove,
        2 => EraseAll,
        3 => EraseSavedLines,
        _ => EmptyAction,
    };
}
