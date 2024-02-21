// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Terminals.Pty.Unix;

internal abstract class PtyConnection : IPtyConnection
{
    private const int EINTR = 4;
    private const int ECHILD = 10;

    private readonly int _controller;
    private readonly int _pid;
    private readonly ManualResetEvent _terminalProcessTerminatedEvent = new(initialState: false);
    private int _exitCode;
    private int _exitSignal;

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

    public event EventHandler<PtyExitedEventArgs>? ProcessExited;

    public Stream ReaderStream { get; }

    public Stream WriterStream { get; }

    public int Pid => _pid;

    public int ExitCode => _exitCode;

    public void Dispose()
    {
        WriterStream.Write(Encoding.UTF8.GetBytes("exit\n"));
        WriterStream.Flush();
        ReaderStream.Dispose();
        WriterStream.Dispose();
        Kill();
    }

    public void Kill()
    {
        if (!Kill(_controller))
        {
            throw new InvalidOperationException($"Killing terminal failed with error {Marshal.GetLastWin32Error()}");
        }
    }

    public void Resize(int cols, int rows)
    {
        if (!Resize(_controller, cols, rows))
        {
            throw new InvalidOperationException($"Resizing terminal failed with error {Marshal.GetLastWin32Error()}");
        }
    }

    public bool WaitForExit(int milliseconds)
    {
        return _terminalProcessTerminatedEvent.WaitOne(milliseconds);
    }

    protected abstract bool Resize(int controller, int cols, int rows);

    protected abstract bool Kill(int controller);

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
