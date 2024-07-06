// <copyright file="ITerminalKeyBinding.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public interface ITerminalKeyBinding
{
    bool CanInvoke(object obj);

    void Invoke(object obj);

    TerminalModifiers Modifiers { get; }

    TerminalKey Key { get; }

    Type Type { get; }
}
