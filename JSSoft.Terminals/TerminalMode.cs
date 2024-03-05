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

public sealed class TerminalMode : INotifyPropertyChanged
{
    private readonly Dictionary<TerminalModeType, bool> _modes;

    internal TerminalMode()
    {
        var types = Enum.GetValues(typeof(TerminalModeType));
        var modes = new Dictionary<TerminalModeType, bool>(types.Length);
        foreach (TerminalModeType type in types)
        {
            modes.Add(type, false);
        }
        _modes = modes;
    }

    public bool this[TerminalModeType mode]
    {
        get => _modes[mode];
        set
        {
            if (_modes[mode] != value)
            {
                _modes[mode] = value;
                InvokePropertyChangedEvent(new PropertyChangedEventArgs($"{mode}"));
            }
        }
    }

    public bool TryGetValue(TerminalModeType mode, out bool value)
        => _modes.TryGetValue(mode, out value);

    public event PropertyChangedEventHandler? PropertyChanged;

    private void InvokePropertyChangedEvent(PropertyChangedEventArgs e)
        => PropertyChanged?.Invoke(this, e);
}
