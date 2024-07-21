// <copyright file="ICommandCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace JSSoft.Commands;

public interface ICommandCollection : IEnumerable<ICommand>
{
    int Count { get; }

    ICommand this[string name] { get; }

    ICommand this[int index] { get; }

    bool Contains(string name);

    bool TryGetValue(string name, [MaybeNullWhen(false)] out ICommand value);
}
