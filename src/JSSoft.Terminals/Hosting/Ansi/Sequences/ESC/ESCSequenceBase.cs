// <copyright file="ESCSequenceBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.ESC;

internal abstract class ESCSequenceBase(char character)
    : SequenceBase(SequenceType.ESC, character)
{
}
