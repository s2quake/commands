// <copyright file="TerminalFieldSetter.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Terminals;

public sealed class TerminalFieldSetter
{
    private readonly EventScope _defaultScope;
    private readonly object _obj;
    private Action<PropertyChangedEventArgs> _action;
    private EventScope? _scope;

    public TerminalFieldSetter(INotifyPropertyChanged obj, Action<PropertyChangedEventArgs> action)
    {
        _defaultScope = new EventScope(this, action);
        _obj = obj;
        _action = action;
    }

    public bool SetField(ref string oldField, string newField, string propertyName)
    {
        if (newField is null)
        {
            throw new ArgumentNullException(nameof(newField));
        }

        if (Equals(oldField, newField) != true)
        {
#if !NETSTANDARD && !NETFRAMEWORK
            System.ComponentModel.DataAnnotations.Validator.ValidateProperty(newField, new System.ComponentModel.DataAnnotations.ValidationContext(_obj) { MemberName = propertyName });
#endif // !NETSTANDARD
            oldField = newField;
            InvokePropertyChangedEvent(propertyName);
            return true;
        }

        return false;
    }

    public bool SetField<T>(ref T oldField, T newField, string propertyName)
    {
        if (Equals(oldField, newField) != true)
        {
#if !NETSTANDARD && !NETFRAMEWORK
            System.ComponentModel.DataAnnotations.Validator.ValidateProperty(newField, new System.ComponentModel.DataAnnotations.ValidationContext(_obj) { MemberName = propertyName });
#endif // !NETSTANDARD
            oldField = newField;
            InvokePropertyChangedEvent(propertyName);
            return true;
        }

        return false;
    }

    public IDisposable LockEvent()
    {
        if (_scope is not null)
        {
            throw new InvalidOperationException();
        }

        _scope = _defaultScope;
        _action = _scope.InvokePropertyChangedEvent;
        return _scope;
    }

    private void InvokePropertyChangedEvent(string propertyName)
        => _action.Invoke(new PropertyChangedEventArgs(propertyName));

    private sealed class EventScope(TerminalFieldSetter setter, Action<PropertyChangedEventArgs> action) : IDisposable
    {
        private readonly List<PropertyChangedEventArgs> _argList = [];
        private readonly TerminalFieldSetter _setter = setter;
        private readonly Action<PropertyChangedEventArgs> _action = action;

        public void InvokePropertyChangedEvent(PropertyChangedEventArgs e)
            => _argList.Add(e);

        public void Dispose()
        {
            foreach (var item in _argList)
            {
                _action.Invoke(item);
            }

            _setter._action = _action;
            _setter._scope = null;
            _argList.Clear();
        }
    }
}
