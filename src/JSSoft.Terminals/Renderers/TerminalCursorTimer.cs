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
