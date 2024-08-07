﻿// <copyright file="CursorBackward.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

internal sealed class CursorBackward : CSISequenceBase
{
    public CursorBackward()
        : base('D')
    {
    }

    public override string DisplayName => "CSI Ps D";

    protected override void OnProcess(SequenceContext context)
    {
        var index = context.Index;
        var value = context.GetParameterAsInteger(index: 0, defaultValue: 1);
        var count = Math.Max(1, value);
        context.Index = index.CursorLeft(count);
    }
}
