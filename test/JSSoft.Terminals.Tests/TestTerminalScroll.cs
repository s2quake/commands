// <copyright file="TestTerminalScroll.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Terminals.Tests;

public sealed class TestTerminalScroll : ITerminalScroll
{
    private readonly TerminalFieldSetter _setter;

    private int _minimum;
    private int _maximum = 100;
    private int _smallChange = 1;
    private int _largeChange = 1;
    private int _viewportSize;
    private bool _isVisible;
    private int _value;

    public TestTerminalScroll()
    {
        _setter = new(this, InvokePropertyChangedEvent);
    }

    public int Minimum
    {
        get => _minimum;
        set => _setter.SetField(ref _minimum, value, nameof(Minimum));
    }

    public int Maximum
    {
        get => _maximum;
        set => _setter.SetField(ref _maximum, value, nameof(Maximum));
    }

    public int SmallChange
    {
        get => _smallChange;
        set => _setter.SetField(ref _smallChange, value, nameof(SmallChange));
    }

    public int LargeChange
    {
        get => _largeChange;
        set => _setter.SetField(ref _largeChange, value, nameof(LargeChange));
    }

    public int ViewportSize
    {
        get => _viewportSize;
        set => _setter.SetField(ref _viewportSize, value, nameof(ViewportSize));
    }

    public bool IsVisible
    {
        get => _isVisible;
        set => _setter.SetField(ref _isVisible, value, nameof(IsVisible));
    }

    public int Value
    {
        get => _value;
        set => _setter.SetField(ref _value, value, nameof(Value));
    }

    public static TestTerminalScroll Default { get; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    private void InvokePropertyChangedEvent(PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }
}
