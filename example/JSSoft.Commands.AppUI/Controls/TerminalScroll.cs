// <copyright file="TerminalScroll.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using Avalonia;
using Avalonia.Controls.Primitives;
using JSSoft.Terminals;

namespace JSSoft.Commands.AppUI.Controls;

internal sealed class TerminalScroll : ITerminalScroll
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

    public event PropertyChangedEventHandler? PropertyChanged;

    public ScrollBar? ScrollBar
    {
        get => _scrollBar;
        set
        {
            if (_scrollBar is not null)
            {
                _scrollBar.ValueChanged -= ScrollBar_ValueChanged;
            }

            _scrollBar = value;
            if (_scrollBar is not null)
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
            if (_setter.SetField(ref _minimum, value, nameof(Minimum)) is true)
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
            if (_setter.SetField(ref _maximum, value, nameof(Maximum)) is true)
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
            if (_setter.SetField(ref _smallChange, value, nameof(SmallChange)) is true)
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
            if (_setter.SetField(ref _largeChange, value, nameof(LargeChange)) is true)
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
            if (_setter.SetField(ref _viewportSize, value, nameof(ViewportSize)) is true)
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
            if (_setter.SetField(ref _isVisible, value, nameof(IsVisible)) is true)
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
            if (_setter.SetField(ref _value, value, nameof(Value)) is true)
            {
                _scrollBar?.SetValue(RangeBase.ValueProperty, _value);
            }
        }
    }

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
