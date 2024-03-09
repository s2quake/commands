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

namespace JSSoft.Terminals.Hosting;

sealed class TerminalModes : ITerminalModes
{
    private readonly Dictionary<TerminalMode, bool> _modes;

    public TerminalModes()
    {
        var types = Enum.GetValues(typeof(TerminalMode));
        var modes = new Dictionary<TerminalMode, bool>(types.Length);
        foreach (TerminalMode type in types)
        {
            modes.Add(type, false);
        }
        _modes = modes;
    }

    public bool this[TerminalMode mode]
    {
        get => _modes[mode];
        set
        {
            if (_modes[mode] != value)
            {
                _modes[mode] = value;
                ModeChanged?.Invoke(this, new TerminalModeChangedEventArgs(mode, value));
            }
        }
    }

    public event EventHandler<TerminalModeChangedEventArgs>? ModeChanged;
}
