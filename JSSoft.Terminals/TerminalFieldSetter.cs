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
            throw new ArgumentNullException(nameof(newField));

        if (Equals(oldField, newField) == false)
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
        if (Equals(oldField, newField) == false)
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
        if (_scope != null)
            throw new InvalidOperationException();

        _scope = _defaultScope;
        _action = _scope.InvokePropertyChangedEvent;
        return _scope;
    }

    private void InvokePropertyChangedEvent(string propertyName)
    {
        _action.Invoke(new PropertyChangedEventArgs(propertyName));
    }

    #region EventScope

    sealed class EventScope(TerminalFieldSetter setter, Action<PropertyChangedEventArgs> action) : IDisposable
    {
        private readonly List<PropertyChangedEventArgs> _argList = [];
        private readonly TerminalFieldSetter _setter = setter;
        private readonly Action<PropertyChangedEventArgs> _action = action;

        public void InvokePropertyChangedEvent(PropertyChangedEventArgs e)
        {
            _argList.Add(e);
        }

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

    #endregion
}
