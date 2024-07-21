// <copyright file="TerminalPropertyChangedBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Terminals;

public abstract class TerminalPropertyChangedBase : INotifyPropertyChanged
{
    private readonly TerminalFieldSetter _setter;

    protected TerminalPropertyChangedBase()
        => _setter = new TerminalFieldSetter(this, OnPropertyChanged);

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        => PropertyChanged?.Invoke(this, e);

    protected void InvokePropertyChanged(string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

    protected bool SetField<T>(ref T oldField, T newField, string propertyName)
        => _setter.SetField(ref oldField, newField, propertyName);
}
