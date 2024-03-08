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
