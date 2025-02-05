// <copyright file="SequenceBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi;

internal readonly record struct SequenceValue(
    ISequence Value, string Parameter, int EndIndex)
{
}
