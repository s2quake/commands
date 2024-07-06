// <copyright file="SequenceType.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi;

enum SequenceType
{
    // \u001b
    ESC,

    // \u001b [
    CSI,

    // \u001b P ... \u001b \\
    DCS,

    // \u001b ]
    OSC
}
