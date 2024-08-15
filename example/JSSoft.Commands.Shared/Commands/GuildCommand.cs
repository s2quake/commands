// <copyright file="GuildCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#if JSSOFT_COMMANDS_REPL

using System.ComponentModel.Composition;

namespace JSSoft.Commands.Applications.Commands;

[Export(typeof(ICommand))]
[method: ImportingConstructor]
internal sealed class GuildCommand(NodeCommand nodeCommand)
    : CommandMethodBase(nodeCommand)
{
    [CommandMethod]
    public void Request(string guildName)
    {
        Out.WriteLine($"request join '{guildName}'");
    }

    [CommandMethod]
    public void Leave(string guildName)
    {
        Out.WriteLine($"leave '{guildName}'");
    }
}
#endif // JSSOFT_COMMANDS_REPL
