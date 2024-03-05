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
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

    public event EventHandler<PtyExitedEventArgs>? Exited;

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
                // TODO: log that waitpid(3) failed with error {Marshal.GetLastWin32Error()}
            }

            return;
        }

        Console.WriteLine($"Wait succeeded");
        _exitSignal = status & SignalMask;
        _exitCode = _exitSignal == 0 ? (status >> 8) & ExitCodeMask : 0;
        _terminalProcessTerminatedEvent.Set();
        Exited?.Invoke(this, new PtyExitedEventArgs(_exitCode));
    }

    #region NativeOptions

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

    #endregion

    #region IPtyConnection

    int IPtyConnection.Read(byte[] buffer, int count) => Read(_fd, buffer, count);

    int IPtyConnection.Write(byte[] buffer, int count) => Write(_fd, buffer, count);

    bool IPtyConnection.CanRead => Peek(_fd);

    #endregion
}
