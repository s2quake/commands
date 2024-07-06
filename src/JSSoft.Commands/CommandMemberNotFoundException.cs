// <copyright file="CommandMemberNotFoundException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public class CommandMemberNotFoundException(string memberName)
    : KeyNotFoundException($"CommandMember '{memberName}' does not exits.")
{
}
