// <copyright file="SystemTerminalKeyBinding.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public sealed class SystemTerminalKeyBinding : TerminalKeyBindingBase<SystemTerminalHost>
{
    private readonly Func<SystemTerminalHost, bool> _action;
    private readonly Func<SystemTerminalHost, bool> _verify;

    public SystemTerminalKeyBinding(TerminalModifiers modifiers, TerminalKey key, Func<SystemTerminalHost, bool> action)
        : this(modifiers, key, action, (obj) => true)
    {
    }

    public SystemTerminalKeyBinding(TerminalModifiers modifiers, TerminalKey key, Action<SystemTerminalHost> action)
        : this(modifiers, key, action, (obj) => true)
    {
    }

    public SystemTerminalKeyBinding(TerminalModifiers modifiers, TerminalKey key, Action<SystemTerminalHost> action, Func<SystemTerminalHost, bool> verify)
        : base(modifiers, key)
    {
        _action = (t) =>
        {
            action(t);
            return true;
        };
        _verify = verify;
    }

    public SystemTerminalKeyBinding(TerminalModifiers modifiers, TerminalKey key, Func<SystemTerminalHost, bool> action, Func<SystemTerminalHost, bool> verify)
        : base(modifiers, key)
    {
        _action = action;
        _verify = verify;
    }

    public SystemTerminalKeyBinding(TerminalKey keyCode, Func<SystemTerminalHost, bool> action)
        : this(keyCode, action, (obj) => true)
    {
    }

    public SystemTerminalKeyBinding(TerminalKey keyCode, Action<SystemTerminalHost> action)
        : this(keyCode, action, (obj) => true)
    {
    }

    public SystemTerminalKeyBinding(TerminalKey key, Action<SystemTerminalHost> action, Func<SystemTerminalHost, bool> verify)
        : base(default!, key)
    {
        _action = (t) =>
        {
            action(t);
            return true;
        };
        _verify = verify;
    }

    public SystemTerminalKeyBinding(TerminalKey key, Func<SystemTerminalHost, bool> action, Func<SystemTerminalHost, bool> verify)
        : base(default!, key)
    {
        _action = action;
        _verify = verify;
    }

    protected override bool OnCanInvoke(SystemTerminalHost obj) => _verify(obj);

    protected override void OnInvoke(SystemTerminalHost obj) => _action(obj);
}
