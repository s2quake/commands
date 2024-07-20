// <copyright file="Backspace.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi;

internal sealed class Backspace : IAsciiCode
{
    public void Process(AsciiCodeContext context)
    {
        var lines = context.Lines;
        var index1 = context.Index;
        var line = lines[index1.Y];
        var index2 = line.Backspace(index1);
        context.Index = index2;
        context.TextIndex++;
    }
}
