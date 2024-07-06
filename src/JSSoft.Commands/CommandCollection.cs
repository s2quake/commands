// <copyright file="CommandCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public sealed class CommandCollection(IEnumerable<ICommand> commands)
    : Dictionary<string, ICommand>(commands.ToDictionary(item => item.Name))
{
}
