// <copyright file="ITerminalFont.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public interface ITerminalFont
{
    bool Contains(char character);

    TerminalGlyphInfo this[char character] { get; }

    int Width { get; }

    int Height { get; }

    int Size { get; }
}
