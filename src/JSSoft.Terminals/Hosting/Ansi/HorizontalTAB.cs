// <copyright file="HorizontalTAB.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi;

sealed class HorizontalTAB : IAsciiCode
{
    public void Process(AsciiCodeContext context)
    {
        var lines = context.Lines;
        var beginIndex = context.BeginIndex;
        var index1 = context.Index;
        var span = 8 - (index1.X % 8);
        var index2 = index1.Expect(span);
        var line = lines.Prepare(beginIndex, index2);
        line.SetEmpty(index2, span);
        context.Index = index2 + span;
        context.TextIndex++;
    }
}
