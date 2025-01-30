// <copyright file="PtyConnection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

    protected PtyConnection(int controller, int pid)
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

    public event EventHandler<PtyExitedEventArgs>? Exited;

    public int Pid => _pid;

    public int ExitCode => _exitCode;

    bool IPtyConnection.CanRead => Peek(_fd);

    public void Dispose()
    {
        IPtyConnectionExtensions.Write(this, "exit\n");
        Kill();
    }

    public void Kill()
    {
        if (!Kill(_fd))
        {
            throw new InvalidOperationException(
                $"Killing terminal failed with error {Marshal.GetLastWin32Error()}");
        }
    }

    public void Resize(int cols, int rows)
    {
        if (!Resize(_fd, cols, rows))
        {
            throw new InvalidOperationException(
                $"Resizing terminal failed with error {Marshal.GetLastWin32Error()}");
        }
    }

    public bool WaitForExit(int milliseconds)
    {
        return _terminalProcessTerminatedEvent.WaitOne(milliseconds);
    }

    int IPtyConnection.Read(byte[] buffer, int count) => Read(_fd, buffer, count);

    int IPtyConnection.Write(byte[] buffer, int count) => Write(_fd, buffer, count);

    protected abstract bool Peek(int fd);

    protected abstract int Read(int fd, byte[] buffer, int count);

    protected abstract int Write(int fd, byte[] buffer, int count);

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
                // log that waitpid(3) failed with error {Marshal.GetLastWin32Error()}
            }

            return;
        }

        Console.WriteLine($"Wait succeeded");
        _exitSignal = status & SignalMask;
        _exitCode = _exitSignal is 0 ? (status >> 8) & ExitCodeMask : 0;
        _terminalProcessTerminatedEvent.Set();
        Exited?.Invoke(this, new PtyExitedEventArgs(_exitCode));
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NativeOptions
    {
        public ushort Width;
        public ushort Height;
        public string App;
        public IntPtr CommandLine;
        public IntPtr EnvironmentVariables;

        private static readonly int SizeOfIntPtr = Marshal.SizeOf(typeof(IntPtr));

        public static IntPtr Marshalling(params string[] items)
        {
            var ppEnv = Marshal.AllocHGlobal((items.Length + 1) * SizeOfIntPtr);
            var offset = 0;
            foreach (var item in items)
            {
                var pEnv = Marshal.StringToHGlobalAnsi(item);
                Marshal.WriteIntPtr(ppEnv, offset, pEnv);
                offset += SizeOfIntPtr;
            }

            Marshal.WriteIntPtr(ppEnv, offset, IntPtr.Zero);
            return ppEnv;
        }

        public static IntPtr Marshalling(IEnumerable<KeyValuePair<string, string>> items)
            => Marshalling(items.Select(item => $"{item.Key}={item.Value}").ToArray());
    }
}
