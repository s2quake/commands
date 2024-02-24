// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using JSSoft.Terminals.Pty.Extensions;

namespace JSSoft.Terminals.Pty.Unix;

internal abstract partial class PtyConnection : IPtyConnection
{
    private const int EINTR = 4;
    private const int ECHILD = 10;

    private readonly int _fd;
    private readonly int _pid;
    private readonly ManualResetEvent _terminalProcessTerminatedEvent = new(initialState: false);
    private int _exitCode;
    private int _exitSignal;

    public PtyConnection(int controller, int pid)
    {
        _fd = controller;
        _pid = pid;
        var childWatcherThread = new Thread(ChildWatcherThreadProc)
        {
            IsBackground = true,
            Priority = ThreadPriority.Lowest,
            Name = $"Watcher thread for child process {pid}",
        };

        childWatcherThread.Start();
    }

    public event EventHandler<PtyExitedEventArgs>? ProcessExited;

    public int Pid => _pid;

    public int ExitCode => _exitCode;

    public void Dispose()
    {
        IPtyConnectionExtensions.Write(this, "exit\n");
        Kill();
    }

    public void Kill()
    {
        if (!Kill(_fd))
        {
            throw new InvalidOperationException($"Killing terminal failed with error {Marshal.GetLastWin32Error()}");
        }
    }

    public void Resize(int cols, int rows)
    {
        if (!Resize(_fd, cols, rows))
        {
            throw new InvalidOperationException($"Resizing terminal failed with error {Marshal.GetLastWin32Error()}");
        }
    }

    public bool WaitForExit(int milliseconds)
    {
        return _terminalProcessTerminatedEvent.WaitOne(milliseconds);
    }

    protected abstract int Read(int fd, byte[] buffer, int count);

    protected abstract void Write(int fd, byte[] buffer, int count);

    protected abstract bool Resize(int fd, int cols, int rows);

    protected abstract bool Kill(int fd);

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

    #region IPtyConnection

    int IPtyConnection.Read(byte[] buffer, int count) => Read(_fd, buffer, count);

    void IPtyConnection.Write(byte[] buffer, int count) => Write(_fd, buffer, count);

    #endregion
}
