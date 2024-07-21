// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using static JSSoft.Terminals.Pty.Windows.NativeMethods;
using static JSSoft.Terminals.Pty.Windows.WinptyNativeInterop;

namespace JSSoft.Terminals.Pty.Windows;

/// <summary>
/// A connection to a pseudoterminal spawned via winpty.
/// </summary>
internal class WinPtyConnection : IPtyConnection
{
    private readonly IntPtr _handle;
    private readonly SafeProcessHandle _processHandle;
    private readonly Process _process;
    private readonly Stream _readerStream;
    private readonly Stream _writerStream;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinPtyConnection"/> class.
    /// </summary>
    /// <param name="readerStream">The reading side of the pty connection.</param>
    /// <param name="writerStream">The writing side of the pty connection.</param>
    /// <param name="handle">A handle to the winpty instance.</param>
    /// <param name="processHandle">A handle to the spawned process.</param>
    public WinPtyConnection(Stream readerStream, Stream writerStream, IntPtr handle, SafeProcessHandle processHandle)
    {
        _readerStream = readerStream;
        _writerStream = writerStream;
        Pid = GetProcessId(processHandle);

        _handle = handle;
        _processHandle = processHandle;
        _process = Process.GetProcessById(Pid);
        _process.Exited += Process_Exited;
        _process.EnableRaisingEvents = true;
    }

    /// <inheritdoc/>
    public event EventHandler<PtyExitedEventArgs>? Exited;

    /// <inheritdoc/>
    public int Pid { get; }

    /// <inheritdoc/>
    public int ExitCode => _process.ExitCode;

    /// <inheritdoc/>
    public void Dispose()
    {
        _readerStream?.Dispose();
        _writerStream?.Dispose();

        _processHandle.Close();
        winpty_free(_handle);
    }

    /// <inheritdoc/>
    public void Kill()
    {
        _process.Kill();
    }

    /// <inheritdoc/>
    public void Resize(int cols, int rows)
    {
        winpty_set_size(_handle, cols, rows, out var err);
    }

    /// <inheritdoc/>
    public bool WaitForExit(int milliseconds)
    {
        return _process.WaitForExit(milliseconds);
    }

    private void Process_Exited(object? sender, EventArgs e)
    {
        Exited?.Invoke(this, new PtyExitedEventArgs(_process.ExitCode));
    }

    int IPtyConnection.Read(byte[] buffer, int count)
    {
        return _readerStream.Read(buffer, 0, count);
    }

    int IPtyConnection.Write(byte[] buffer, int count)
    {
        _writerStream.Write(buffer, 0, count);
        _writerStream.Flush();
        return count;
    }
}
