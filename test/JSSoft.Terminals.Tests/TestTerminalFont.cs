// <copyright file="TestTerminalFont.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Tests;

internal sealed class TestTerminalFont : ITerminalFont
{
    public bool Contains(char character) => true;

    public TerminalGlyphInfo this[char character] => TerminalGlyphInfo.Empty;

    public int Width { get; }

    public int Height { get; }

    public int Size { get; }

    public static TestTerminalFont Default { get; } = new();
}
