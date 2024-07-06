// <copyright file="EraseInDisplay.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

namespace JSSoft.Terminals.Hosting.Ansi.EraseFunctions;

sealed class EraseInDisplay : CSISequenceBase
{
    private static readonly Action<SequenceContext> EmptyAction = (context) => { };

    public EraseInDisplay()
        : base('J')
    {
    }

    public override string DisplayName => "CSI Ps J";

    protected override void OnProcess(SequenceContext context)
    {
        var option = context.GetParameterAsInteger(index: 0, defaultValue: 0);
        var action = GetAction(option);
        action.Invoke(context);
    }

    private void EraseBelow(SequenceContext context)
    {
        var lines = context.Lines;
        var view = context.View;
        var index0 = context.Index;
        var index1 = new TerminalIndex(x: view.Right - 1, y: view.Bottom - 1, view.Width);
        var length = Math.Min(lines.Count * view.Width + view.Width, index1.Value) - index0.X;
        lines.Erase(index0, length);
    }

    private void EraseAbove(SequenceContext context)
    {
        var lines = context.Lines;
        var view = context.View;
        var index0 = new TerminalIndex(x: view.Left, y: view.Top, view.Width);
        var index1 = context.Index;
        var length = index1 - index0;
        lines.Erase(index0, length);
    }

    private void EraseAll(SequenceContext context)
    {
        var lines = context.Lines;
        var view = context.View;
        var index0 = new TerminalIndex(x: view.Left, y: view.Top, view.Width);
        var index1 = new TerminalIndex(x: view.Right, y: view.Bottom - 1, view.Width);
        var length = index1.Value - index0.Value;
        lines.Erase(index0, length);
    }

    private void EraseSavedLines(SequenceContext context)
    {
    }

    private Action<SequenceContext> GetAction(int option) => option switch
    {
        0 => EraseBelow,
        1 => EraseAbove,
        2 => EraseAll,
        3 => EraseSavedLines,
        _ => EmptyAction,
    };
}
