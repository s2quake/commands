// <copyright file="TerminalKeyBindingBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public abstract class TerminalKeyBindingBase<T> : ITerminalKeyBinding
    where T : class
{
    protected TerminalKeyBindingBase(TerminalModifiers modifiers, TerminalKey key)
    {
        Modifiers = modifiers;
        Key = key;
    }

    protected TerminalKeyBindingBase(TerminalKey key)
    {
        Key = key;
    }

    protected abstract bool OnCanInvoke(T obj);

    protected abstract void OnInvoke(T obj);

    public TerminalModifiers Modifiers { get; } = default!;

    public TerminalKey Key { get; }

    public Type Type => typeof(T);

    public bool IsPreview { get; set; }

    void ITerminalKeyBinding.Invoke(object obj)
    {
        if (Type.IsAssignableFrom(obj.GetType()) != true)
        {
            throw new ArgumentException("invalid type", nameof(obj));
        }

        OnInvoke((T)obj);
    }

    bool ITerminalKeyBinding.CanInvoke(object obj)
    {
        if (Type.IsAssignableFrom(obj.GetType()) != true)
        {
            return false;
        }

        return OnCanInvoke((T)obj);
    }
}
