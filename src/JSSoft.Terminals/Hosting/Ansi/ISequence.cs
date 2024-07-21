// <copyright file="ISequence.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi;

internal interface ISequence : IComparable<ISequence>, IEquatable<ISequence>
{
    SequenceType Type { get; }

    char Character { get; }

    void Process(SequenceContext context);

    bool Match(string text, Range parameterRange, out Range actualParameterRange);
}
