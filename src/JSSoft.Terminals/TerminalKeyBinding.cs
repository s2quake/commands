// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

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
