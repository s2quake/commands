// <copyright file="TerminalDisplayInfo.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public partial struct TerminalDisplayInfo
{
    public bool IsBold { get; set; }

    public bool IsItalic { get; set; }

    public bool IsUnderline { get; set; }

    public bool IsInverse { get; set; }

    public object? Foreground { get; set; }

    public object? Background { get; set; }

    public static TerminalDisplayInfo Empty { get; } = new TerminalDisplayInfo();
}
