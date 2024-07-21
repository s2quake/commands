// <copyright file="CSISequenceBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

internal abstract class CSISequenceBase(char character)
    : SequenceBase(SequenceType.CSI, character)
{
}
