// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Pty.Net.Unix;
/// <summary>
/// A connection to a Unix-style pseudoterminal.
/// </summary>
internal abstract class PtyConnection : IPtyConnection
{
    private const int EINTR = 4;
    private const int ECHILD = 10;

    private readonly int _controller;
    private readonly int _pid;
    private readonly ManualResetEvent _terminalProcessTerminatedEvent = new(initialState: false);
    private int _exitCode;
    private int _exitSignal;

    /// <summary>
    /// Initializes a new instance of the <see cref="PtyConnection"/> class.
    /// </summary>
    /// <param name="controller">The fd of the pty controller.</param>
    /// <param name="pid">The id of the spawned process.</param>
    public PtyConnection(int controller, int pid)
    {
        ReaderStream = new PtyStream(controller, FileAccess.Read);
        WriterStream = new PtyStream(controller, FileAccess.Write);

        _controller = controller;
        _pid = pid;
        var childWatcherThread = new Thread(ChildWatcherThreadProc)
        {
            IsBackground = true,
            Priority = ThreadPriority.Lowest,
            Name = $"Watcher thread for child process {pid}",
        };

        childWatcherThread.Start();
    }

    /// <inheritdoc/>
    public event EventHandler<PtyExitedEventArgs>? ProcessExited;

    /// <inheritdoc/>
    public Stream ReaderStream { get; }

    /// <inheritdoc/>
    public Stream WriterStream { get; }

    /// <inheritdoc/>
    public int Pid => _pid;

    /// <inheritdoc/>
    public int ExitCode => _exitCode;

    /// <inheritdoc/>
    public void Dispose()
    {
        ReaderStream?.Dispose();
        WriterStream?.Dispose();
        Kill();
    }

    /// <inheritdoc/>
    public void Kill()
    {
        if (!Kill(_controller))
        {
            throw new InvalidOperationException($"Killing terminal failed with error {Marshal.GetLastWin32Error()}");
        }
    }

    /// <inheritdoc/>
    public void Resize(int cols, int rows)
    {
        if (!Resize(_controller, cols, rows))
        {
            throw new InvalidOperationException($"Resizing terminal failed with error {Marshal.GetLastWin32Error()}");
        }
    }

    /// <inheritdoc/>
    public bool WaitForExit(int milliseconds)
    {
        return _terminalProcessTerminatedEvent.WaitOne(milliseconds);
    }

    /// <summary>
    /// OS-specific implementation of the pty-resize function.
    /// </summary>
    /// <param name="controller">The fd of the pty controller.</param>
    /// <param name="cols">The number of columns to resize to.</param>
    /// <param name="rows">The number of rows to resize to.</param>
    /// <returns>True if the function suceeded to resize the pty, false otherwise.</returns>
    protected abstract bool Resize(int controller, int cols, int rows);

    /// <summary>
    /// Kills the terminal process.
    /// </summary>
    /// <param name="controller">The fd of the pty controller.</param>
    /// <returns>True if the function succeeded in killing the process, false otherwise.</returns>
    protected abstract bool Kill(int controller);

    /// <summary>
    /// OS-specific implementation of waiting on the given process id.
    /// </summary>
    /// <param name="pid">The process id to wait on.</param>
    /// <param name="status">The status of the process.</param>
    /// <returns>True if the function succeeded to get the status of the process, false otherwise.</returns>
    protected abstract bool WaitPid(int pid, ref int status);

    private void ChildWatcherThreadProc()
    {
        Console.WriteLine($"Waiting on {_pid}");
        const int SignalMask = 127;
        const int ExitCodeMask = 255;

        int status = 0;
        if (!WaitPid(_pid, ref status))
        {
            int errno = Marshal.GetLastWin32Error();
            Console.WriteLine($"Wait failed with {errno}");
            if (errno == EINTR)
            {
                ChildWatcherThreadProc();
            }
            else if (errno == ECHILD)
            {
                // waitpid is already handled elsewhere.
                // Not an error.
            }
            else
            {
                // TODO: log that waitpid(3) failed with error {Marshal.GetLastWin32Error()}
            }

            return;
        }

        Console.WriteLine($"Wait succeeded");
        _exitSignal = status & SignalMask;
        _exitCode = _exitSignal == 0 ? (status >> 8) & ExitCodeMask : 0;
        _terminalProcessTerminatedEvent.Set();
        ProcessExited?.Invoke(this, new PtyExitedEventArgs(_exitCode));
    }
}
