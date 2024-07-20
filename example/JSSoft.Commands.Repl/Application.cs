// <copyright file="Application.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using JSSoft.Commands.Applications;

namespace JSSoft.Commands.Repl;

internal sealed class Application : IApplication, IDisposable
{
    private readonly CompositionContainer _container;
    private string _currentDirectory = Directory.GetCurrentDirectory();
    private CancellationTokenSource? _cancellationTokenSource;
    private bool _isDisposed;
    private SystemTerminal? _terminal;

    public Application()
    {
        _container = new(new AssemblyCatalog(typeof(Application).Assembly));
        _container.ComposeExportedValue<IApplication>(this);
        _container.ComposeExportedValue(this);
    }

    public event EventHandler? DirectoryChanged;

    public string CurrentDirectory
    {
        get => _currentDirectory;
        set
        {
            _currentDirectory = value;
            DirectoryChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Cancel()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = null;
    }

    public string? ReadString(string prompt, string command)
    {
        if (_terminal is null)
        {
            throw new InvalidOperationException("Application has not started.");
        }

        return _terminal.ReadString(prompt, command);
    }

    public SecureString? ReadSecureString(string prompt)
    {
        if (_terminal is null)
        {
            throw new InvalidOperationException("Application has not started.");
        }

        return _terminal.ReadSecureString(prompt);
    }

    public Task StartAsync()
    {
        if (_terminal is not null)
        {
            throw new InvalidOperationException("Application has already been started.");
        }

        _terminal = _container.GetExportedValue<SystemTerminal>();
        _cancellationTokenSource = new();
        return _terminal.StartAsync(_cancellationTokenSource.Token);
    }

    public void Dispose()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        if (_cancellationTokenSource is not null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        _terminal = null;
        _container.Dispose();
        _isDisposed = true;
    }
}
