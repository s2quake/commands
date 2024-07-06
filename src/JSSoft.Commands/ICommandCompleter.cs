// <copyright file="ICommandCompleter.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public interface ICommandCompleter
{
    string[] GetCompletions(CommandCompletionContext completionContext);
}
