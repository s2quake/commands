// <copyright file="OSCSequenceBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.OSC;

abstract class OSCSequenceBase(char character)
    : SequenceBase(SequenceType.OSC, character)
{
}
