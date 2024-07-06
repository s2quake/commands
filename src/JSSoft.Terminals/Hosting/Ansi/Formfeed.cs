// <copyright file="Formfeed.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi;

sealed class Formfeed : IAsciiCode
{
    public void Process(AsciiCodeContext context)
    {
        Console.Beep();
        context.TextIndex++;
    }
}