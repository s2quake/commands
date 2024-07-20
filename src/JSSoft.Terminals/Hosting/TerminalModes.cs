// <copyright file="TerminalModes.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting;

internal sealed class TerminalModes : ITerminalModes
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
