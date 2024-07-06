// <copyright file="TerminalCursorTimer.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;

namespace JSSoft.Terminals.Renderers;

sealed class TerminalCursorTimer : TerminalPropertyChangedBase, IDisposable
{
    private readonly SynchronizationContext? _synchronizationContext = SynchronizationContext.Current;

    private bool _isEnabled;
    private int _interval = 1000;
    private bool _isDisposed;
    private Timer? _timer;

    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (SetField(ref _isEnabled, value, nameof(IsEnabled)) == true)
            {
                if (_isEnabled == true)
                {
                    _timer = new(Timer_Callback, _synchronizationContext, 0, _interval);
                }
                else
                {
                    _timer!.Dispose();
                    _timer = null;
                }
            }
        }
    }

    public int Interval
    {
        get => _interval;
        set
        {
            if (SetField(ref _interval, value, nameof(Interval)) == true)
            {
                _timer?.Change(_interval, _interval);
            }
        }
    }

    public void Reset() => _timer?.Change(_interval, _interval);

    public void Dispose()
    {
        if (_isDisposed == true)
            throw new ObjectDisposedException($"{this}");

        IsEnabled = false;
        _isDisposed = true;
        GC.SuppressFinalize(this);
    }

    public event EventHandler? Tick;

    private void InvokeTickEvent(EventArgs e) => Tick?.Invoke(this, e);

    private void Timer_Callback(object? state)
    {
        if (state is SynchronizationContext synchronizationContext)
        {
            synchronizationContext.Post((state) => InvokeTickEvent(EventArgs.Empty), null);
        }
    }
}
