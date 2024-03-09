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
