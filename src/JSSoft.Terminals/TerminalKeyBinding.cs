// <copyright file="TerminalKeyBinding.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public sealed class TerminalKeyBinding : TerminalKeyBindingBase<ITerminal>
{
    private readonly Func<ITerminal, bool> _action;
    private readonly Func<ITerminal, bool> _verify;

    public TerminalKeyBinding(TerminalModifiers modifiers, TerminalKey key, Func<ITerminal, bool> action)
        : this(modifiers, key, action, (obj) => true)
    {
    }

    public TerminalKeyBinding(TerminalModifiers modifiers, TerminalKey key, Action<ITerminal> action)
        : this(modifiers, key, action, (obj) => true)
    {
    }

    public TerminalKeyBinding(TerminalModifiers modifiers, TerminalKey key, Action<ITerminal> action, Func<ITerminal, bool> verify)
        : base(modifiers, key)
    {
        _action = (t) =>
        {
            action(t);
            return true;
        };
        _verify = verify;
    }

    public TerminalKeyBinding(TerminalModifiers modifiers, TerminalKey key, Func<ITerminal, bool> action, Func<ITerminal, bool> verify)
        : base(modifiers, key)
    {
        _action = action;
        _verify = verify;
    }

    public TerminalKeyBinding(TerminalKey keyCode, Func<ITerminal, bool> action)
        : this(keyCode, action, (obj) => true)
    {
    }

    public TerminalKeyBinding(TerminalKey keyCode, Action<ITerminal> action)
        : this(keyCode, action, (obj) => true)
    {
    }

    public TerminalKeyBinding(TerminalKey key, Action<ITerminal> action, Func<ITerminal, bool> verify)
        : base(default!, key)
    {
        _action = (t) =>
        {
            action(t);
            return true;
        };
        _verify = verify;
    }

    public TerminalKeyBinding(TerminalKey key, Func<ITerminal, bool> action, Func<ITerminal, bool> verify)
        : base(default!, key)
    {
        _action = action;
        _verify = verify;
    }

    protected override bool OnCanInvoke(ITerminal obj) => _verify(obj);

    protected override void OnInvoke(ITerminal obj) => _action(obj);
}
