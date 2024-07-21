// <copyright file="TerminalStyleProperty.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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
        => Changed?.Invoke(this, new TerminalStylePropertyChangedEventArgs<T>(Value));
}
