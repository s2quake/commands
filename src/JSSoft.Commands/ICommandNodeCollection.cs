// <copyright file="ICommandNodeCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace JSSoft.Commands;

public interface ICommandNodeCollection : IEnumerable<ICommandNode>
{
    int Count { get; }

    ICommandNode this[string name] { get; }

    ICommandNode this[int index] { get; }

    bool Contains(string name);

    bool TryGetValue(string name, [MaybeNullWhen(false)] out ICommandNode value);
}
