// <copyright file="TerminalInputHandler.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Input;

sealed class TerminalInputHandler : InputHandler<TerminalInputHandlerContext>
{
    protected override TerminalInputHandlerContext CreateContext(ITerminal terminal)
    {
        return new TerminalInputHandlerContext(terminal);
    }
}
