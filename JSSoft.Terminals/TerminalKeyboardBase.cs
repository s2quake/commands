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

public abstract class TerminalKeyboardBase : ITerminalKeyboard
{
    private string _text = string.Empty;
    private TerminalRange _selection;
    private TerminalRect _area;

    protected TerminalKeyboardBase()
    {
        Opened += (s, e) => Current = this;
        Done += (s, e) => Current = null;
        Canceled += (s, e) => Current = null;
    }

    public void Open(ITerminal terminal, string text)
    {
        if (IsOpened == true)
            throw new InvalidOperationException("keyboard already open.");
        Terminal = terminal ?? throw new ArgumentNullException(nameof(terminal));
        _text = text ?? throw new ArgumentNullException(nameof(text));
        _selection = new TerminalRange(text.Length);
    }

    public void Close()
    {
        if (IsOpened == false)
            throw new InvalidOperationException("keyboard already closed.");
        OnClose();
        IsOpened = false;
        Terminal = null;
        _text = string.Empty;
        _selection = default;
        OnCanceled(EventArgs.Empty);
    }

    public void Update()
    {
        if (IsOpened == false && _text != string.Empty)
        {
            OnOpen(_text);
            IsOpened = true;
            OnOpened(new TerminalKeyboardEventArgs(Text, Selection, Area));
        }
        else if (IsOpened == true)
        {
            var text = Text;
            var selection = Selection;
            var area = Area;
            var result = OnUpdate();
            if (result == true)
            {
                IsOpened = false;
                OnDone(new TerminalKeyboardEventArgs(_text, _selection, _area));
                Terminal = null;
                _text = string.Empty;
                _selection = default;
                _area = default;
            }
            else if (result == false)
            {
                IsOpened = false;
                Terminal = null;
                _text = string.Empty;
                _selection = default;
                OnCanceled(EventArgs.Empty);
            }
            else if (_text != text
                    || object.Equals(_selection, selection) == false
                    || object.Equals(_area, area) == false)
            {
                _text = text;
                _selection = selection;
                _area = area;
                OnChanged(new TerminalKeyboardEventArgs(_text, _selection, _area));
            }
        }
    }

    public bool IsOpened { get; private set; }

    public abstract string Text { get; set; }

    public abstract TerminalRange Selection { get; set; }

    public abstract TerminalRect Area { get; }

    public ITerminal? Terminal { get; private set; }

    public static TerminalKeyboardBase? Current { get; private set; }

    public event EventHandler<TerminalKeyboardEventArgs>? Opened;

    public event EventHandler<TerminalKeyboardEventArgs>? Done;

    public event EventHandler? Canceled;

    public event EventHandler<TerminalKeyboardEventArgs>? Changed;

    protected abstract void OnOpen(string text);

    protected abstract void OnClose();

    protected abstract bool? OnUpdate();

    protected virtual void OnOpened(TerminalKeyboardEventArgs e)
    {
        Opened?.Invoke(this, e);
    }

    protected virtual void OnDone(TerminalKeyboardEventArgs e)
    {
        Done?.Invoke(this, e);
    }

    protected virtual void OnCanceled(EventArgs e)
    {
        Canceled?.Invoke(this, e);
    }

    protected virtual void OnChanged(TerminalKeyboardEventArgs e)
    {
        Changed?.Invoke(this, e);
    }
}
