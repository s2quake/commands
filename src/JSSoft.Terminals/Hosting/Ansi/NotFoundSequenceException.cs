// <copyright file="NotFoundSequenceException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi;

sealed class NotFoundSequenceException : InvalidOperationException
{
    public NotFoundSequenceException(string sequence)
    {
        Sequence = sequence;
    }

    public string Sequence { get; }
}
