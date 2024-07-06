// <copyright file="TerminalDataInfo.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Serializations;

public struct TerminalDataInfo
{
    public string OutputText { get; set; }

    public string Command { get; set; }

    public string InputText { get; set; }

    public string Completion { get; set; }

    public TerminalDisplayInfo[] OutputTextDisplayInfo { get; set; }

    public TerminalDisplayInfo[] PromptDisplayInfo { get; set; }

    public TerminalDisplayInfo[] CommandDisplayInfo { get; set; }

    public int CursorPosition { get; set; }

    public string[] Histories { get; set; }

    public int HistoryIndex { get; set; }
}
