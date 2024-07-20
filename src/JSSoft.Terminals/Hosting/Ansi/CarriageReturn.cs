// <copyright file="CarriageReturn.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi;

internal sealed class CarriageReturn : IAsciiCode
{
    public void Process(AsciiCodeContext context)
    {
        var index = context.Index;
        var beginIndex = context.BeginIndex;
        context.Index =  index.CarriageReturn(beginIndex);
        context.TextIndex++;
    }
}