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

using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace JSSoft.Terminals;

public sealed class TerminalStyleProperty<T> : IDisposable
{
    private readonly ITerminal _terminal;
    private readonly string _propertyName;
    private readonly PropertyInfo _propertyInfo;
    private ITerminalStyle _style;
    private bool _isDisposed;

    public TerminalStyleProperty(ITerminal terminal, string propertyName)
    {
        _terminal = terminal;
        _style = terminal.ActualStyle;
        _propertyName = propertyName;
        _propertyInfo = typeof(ITerminalStyle).GetProperty(propertyName) ?? throw new ArgumentException($"Style does not have Property '{propertyName}'.", nameof(propertyName));
        _terminal.PropertyChanged += Terminal_PropertyChanged;
        _style.PropertyChanged += Style_PropertyChanged;
    }

    public TerminalStyleProperty(ITerminal terminal, string propertyName, EventHandler<TerminalStylePropertyChangedEventArgs<T>> action)
    {
        _terminal = terminal;
        _style = terminal.ActualStyle;
        _propertyName = propertyName;
        _propertyInfo = typeof(ITerminalStyle).GetProperty(propertyName) ?? throw new ArgumentException($"Style does not have Property '{propertyName}'.", nameof(propertyName));
        _terminal.PropertyChanged += Terminal_PropertyChanged;
        _style.PropertyChanged += Style_PropertyChanged;
        Changed += action;
        action.Invoke(this, new TerminalStylePropertyChangedEventArgs<T>(Value));
    }

    public T Value => _propertyInfo.GetValue(_style) is T v ? v : throw new UnreachableException();

    public event EventHandler<TerminalStylePropertyChangedEventArgs<T>>? Changed;

    public void Dispose()
    {
        ThrowUtility.ThrowObjectDisposedException(condition: _isDisposed, this);

        _style.PropertyChanged -= Style_PropertyChanged;
        _terminal.PropertyChanged -= Terminal_PropertyChanged;
        _isDisposed = true;
    }

    private void Style_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == _propertyName)
        {
            InvokeChangedEvent();
        }
    }

    private void Terminal_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ITerminal.ActualStyle))
        {
            _style.PropertyChanged -= Style_PropertyChanged;
            _style = _terminal.ActualStyle;
            _style.PropertyChanged += Style_PropertyChanged;
            InvokeChangedEvent();
        }
    }

    private void InvokeChangedEvent()
    {
        Changed?.Invoke(this, new TerminalStylePropertyChangedEventArgs<T>(Value));
    }
}
