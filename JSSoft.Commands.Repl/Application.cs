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
using System.ComponentModel.Composition;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.Composition.Hosting;
using JSSoft.Commands.Applications;

namespace JSSoft.Commands.Repl;

sealed class Application : IApplication, IDisposable
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

    public void Cancel()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = null;
    }

    public string? ReadString(string prompt, string command)
    {
        if (_terminal == null)
            throw new InvalidOperationException("Application has not started.");

        return _terminal.ReadString(prompt, command);
    }

    public SecureString? ReadSecureString(string prompt)
    {
        if (_terminal == null)
            throw new InvalidOperationException("Application has not started.");

        return _terminal.ReadSecureString(prompt);
    }

    public Task StartAsync()
    {
        if (_terminal != null)
            throw new InvalidOperationException("Application has already been started.");

        _terminal = _container.GetExportedValue<SystemTerminal>();
        _cancellationTokenSource = new();
        return _terminal.StartAsync(_cancellationTokenSource.Token);
    }

    public void Dispose()
    {
        if (_isDisposed == true)
            throw new ObjectDisposedException($"{this}");

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = null;
        _terminal = null;
        _container.Dispose();
        _isDisposed = true;
    }

    public string CurrentDirectory
    {
        get => _currentDirectory;
        set
        {
            _currentDirectory = value;
            DirectoryChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? DirectoryChanged;
}
