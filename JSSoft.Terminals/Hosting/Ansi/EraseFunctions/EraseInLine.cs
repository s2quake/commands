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

namespace JSSoft.Terminals.Hosting.Ansi.EraseFunctions;

sealed class EraseInLine : IEscapeSequence
{
    private static readonly Action<TerminalLineCollection, EscapeSequenceContext> EmptyAction = (items, context) => { };

    public void Process(TerminalLineCollection lines, EscapeSequenceContext context)
    {
        var option = context.GetOptionValue(index: 0) ?? 0;
        var action = GetAction(option);
        action.Invoke(lines, context);
    }

    // erase from cursor to end of line
    private void Action0(TerminalLineCollection lines, EscapeSequenceContext context)
    {
        var index = context.Index;
        if (lines.TryGetLine(index, out var line) == true)
        {
            line.Erase(index, line.Length - index.X);
        }
    }

    // erase start of line to the cursor
    private void Action1(TerminalLineCollection lines, EscapeSequenceContext context)
    {
        var index = context.Index;
        var index1 = index.MoveToFirstOfLine();
        var index2 = index;
        var length = Math.Max(index2 - index1, 1);
        var line = lines[index1];
        line.Erase(index1, length);
    }

    // erase the entire line
    private void Action2(TerminalLineCollection lines, EscapeSequenceContext context)
    {
        var view = context.View;
        var index = context.Index;
        var line = lines[index];
        line.Erase(0, line.Length);
    }

    private Action<TerminalLineCollection, EscapeSequenceContext> GetAction(int option) => option switch
    {
        0 => Action0,
        1 => Action1,
        2 => Action2,
        _ => EmptyAction,
    };
}
