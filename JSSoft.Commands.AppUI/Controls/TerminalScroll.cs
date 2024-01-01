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
using Avalonia;
using Avalonia.Controls.Primitives;
using JSSoft.Terminals;

namespace JSSoft.Commands.AppUI.Controls;

sealed class TerminalScroll : ITerminalScroll
{
    private readonly TerminalFieldSetter _setter;
    private ScrollBar? _scrollBar;

    private int _minimum;
    private int _maximum = 100;
    private int _smallChange = 1;
    private int _largeChange = 1;
    private int _viewportSize;
    private bool _isVisible;
    private int _value;

    public TerminalScroll()
    {
        _setter = new(this, InvokePropertyChangedEvent);
    }

    public ScrollBar? ScrollBar
    {
        get => _scrollBar;
        set
        {
            if (_scrollBar != null)
            {
                _scrollBar.ValueChanged -= ScrollBar_ValueChanged;
            }
            _scrollBar = value;
            if (_scrollBar != null)
            {
                _scrollBar.Minimum = _minimum;
                _scrollBar.Maximum = _maximum;
                _scrollBar.SmallChange = _smallChange;
                _scrollBar.LargeChange = _largeChange;
                _scrollBar.ViewportSize = _viewportSize;
                _scrollBar.IsVisible = _isVisible;
                _scrollBar.Value = _value;
                _scrollBar.ValueChanged += ScrollBar_ValueChanged;
            }
        }
    }

    public int Minimum
    {
        get => _minimum;
        set
        {
            if (_setter.SetField(ref _minimum, value, nameof(Minimum)) == true)
            {
                _scrollBar?.SetValue(RangeBase.MinimumProperty, _minimum);
            }
        }
    }

    public int Maximum
    {
        get => _maximum;
        set
        {
            if (_setter.SetField(ref _maximum, value, nameof(Maximum)) == true)
            {
                _scrollBar?.SetValue(RangeBase.MaximumProperty, _maximum);
            }
        }
    }

    public int SmallChange
    {
        get => _smallChange;
        set
        {
            if (_setter.SetField(ref _smallChange, value, nameof(SmallChange)) == true)
            {
                _scrollBar?.SetValue(RangeBase.SmallChangeProperty, _smallChange);
            }
        }
    }

    public int LargeChange
    {
        get => _largeChange;
        set
        {
            if (_setter.SetField(ref _largeChange, value, nameof(LargeChange)) == true)
            {
                _scrollBar?.SetValue(RangeBase.LargeChangeProperty, _largeChange);
            }
        }
    }

    public int ViewportSize
    {
        get => _viewportSize;
        set
        {
            if (_setter.SetField(ref _viewportSize, value, nameof(ViewportSize)) == true)
            {
                _scrollBar?.SetValue(ScrollBar.ViewportSizeProperty, _viewportSize);
            }
        }
    }

    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (_setter.SetField(ref _isVisible, value, nameof(IsVisible)) == true)
            {
                _scrollBar?.SetValue(Visual.IsVisibleProperty, _isVisible);
            }
        }
    }

    public int Value
    {
        get => _value;
        set
        {
            if (_setter.SetField(ref _value, value, nameof(Value)) == true)
            {
                _scrollBar?.SetValue(RangeBase.ValueProperty, _value);
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void InvokePropertyChangedEvent(PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }

    private void ScrollBar_ValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (sender is ScrollBar scrollBar)
        {
            _setter.SetField(ref _value, (int)scrollBar.Value, nameof(Value));
        }
    }
}
