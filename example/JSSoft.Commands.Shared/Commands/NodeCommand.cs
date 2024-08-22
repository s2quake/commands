// <copyright file="NodeCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#if JSSOFT_COMMANDS_REPL

using System.ComponentModel.Composition;

namespace JSSoft.Commands.Applications.Commands;

[Export(typeof(ICommand))]
[Export]
internal sealed class NodeCommand : CommandMethodBase
{
}
#endif // JSSOFT_COMMANDS_REPL
