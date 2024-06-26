﻿// Released under the MIT License.
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

sealed class EraseInLine : CSISequenceBase
{
    private static readonly Action<SequenceContext> EmptyAction = (context) => { };

    public EraseInLine()
        : base('K')
    {
    }

    public override string DisplayName => "CSI Ps K";

    protected override void OnProcess(SequenceContext context)
    {
        var option = context.GetParameterAsInteger(index: 0, defaultValue: 0);
        var action = GetAction(option);
        action.Invoke(context);
    }

    private void EraseToRight(SequenceContext context)
    {
        var lines = context.Lines;
        var index = context.Index;
        if (lines.TryGetLine(index, out var line) == true)
        {
            line.Erase(index, line.Length - index.X);
        }
    }

    private void EraseToLeft(SequenceContext context)
    {
        var lines = context.Lines;
        var index = context.Index;
        var index1 = index.MoveToFirstOfLine();
        var index2 = index;
        var length = Math.Max(index2 - index1, 1);
        var line = lines[index1];
        line.Erase(index1, length);
    }

    private void EraseAll(SequenceContext context)
    {
        var lines = context.Lines;
        var view = context.View;
        var index = context.Index;
        var line = lines[index];
        line.Erase(0, line.Length);
    }

    private Action<SequenceContext> GetAction(int option) => option switch
    {
        0 => EraseToRight,
        1 => EraseToLeft,
        2 => EraseAll,
        _ => EmptyAction,
    };
}
