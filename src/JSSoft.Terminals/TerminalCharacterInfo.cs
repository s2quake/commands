// <copyright file="TerminalCharacterInfo.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public struct TerminalCharacterInfo
{
    public char Character { get; set; }

    public TerminalDisplayInfo DisplayInfo { get; set; }

    public int Span { get; set; }

    public int Group { get; set; }

    public static TerminalCharacterInfo Empty { get; } = new TerminalCharacterInfo();

    public override readonly string ToString()
    {
        return $"{Character}";
    }
}
