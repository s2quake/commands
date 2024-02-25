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

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using JSSoft.Commands.AppUI.Controls;

namespace JSSoft.Commands.AppUI;

public partial class MainWindow : Window
{
    private readonly CommandContext _commandContext;
    private readonly string _originTitle;

    private readonly PseudoTerminal _pseudoTerminal;

    public MainWindow()
    {
        InitializeComponent();
        App.Current.RegisterService(_terminal);
        _commandContext = App.Current.GetService<CommandContext>()!;
        _commandContext.Owner = this;
        _pseudoTerminal = new PseudoTerminal(_terminal);
        _terminal.PropertyChanged += Terminal_PropertyChanged;
        _originTitle = $"{Title}";
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        _terminal.IsReadOnly = true;
        _pseudoTerminal.Size = _terminal.BufferSize;
        _pseudoTerminal.Open();
        _terminal.IsReadOnly = false;
        _terminal.Focus();
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        _pseudoTerminal.Close();
        base.OnUnloaded(e);
    }

    private void Terminal_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == TerminalControl.BufferSizeProperty)
        {
            Title = $"{_originTitle} — {(int)_terminal.BufferSize.Width}x{(int)_terminal.BufferSize.Height}";
            Console.WriteLine(Title);
        }
    }
}
