// <copyright file="MainWindow.axaml.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using JSSoft.Commands.AppUI.Controls;

namespace JSSoft.Commands.AppUI;

/// <summary>
/// This class contains the main window of the application.
/// </summary>
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
        _pseudoTerminal.Exited += PseudoTerminal_Exited;
        _terminal.IsReadOnly = false;
        _terminal.Focus();
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        _pseudoTerminal.Exited -= PseudoTerminal_Exited;
        _pseudoTerminal.Close();
        base.OnUnloaded(e);
    }

    private void Terminal_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == TerminalControl.BufferSizeProperty)
        {
            var width = (int)_terminal.BufferSize.Width;
            var height = (int)_terminal.BufferSize.Height;
            Title = $"{_originTitle} — {width}x{height}";
            Console.WriteLine(Title);
            _pseudoTerminal.Size = _terminal.BufferSize;
        }
    }

    private async void PseudoTerminal_Exited(object? sender, EventArgs e)
    {
        await Dispatcher.UIThread.InvokeAsync(Close);
    }
}
