// <copyright file="EraseInLine.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

namespace JSSoft.Terminals.Hosting.Ansi.EraseFunctions;

internal sealed class EraseInLine : CSISequenceBase
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
        if (lines.TryGetLine(index, out var line) is true)
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
