// <copyright file="Linefeed.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi;

internal sealed class Linefeed : IAsciiCode
{
    public void Process(AsciiCodeContext context)
    {
        var lines = context.Lines;
        var index = context.Index;
        lines.Prepare(context.BeginIndex, ref index);
        context.Index = index.Linefeed();
        context.BeginIndex = context.Index;
        context.TextIndex++;
    }
}
