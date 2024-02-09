// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JSSoft.Terminals.Pty.Windows
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using static JSSoft.Terminals.Pty.Windows.NativeMethods;
    using static JSSoft.Terminals.Pty.Windows.WinptyNativeInterop;

    /// <summary>
    /// A connection to a pseudoterminal spawned via winpty.
    /// </summary>
    internal class WinPtyConnection : IPtyConnection
    {
        private readonly IntPtr handle;
        private readonly SafeProcessHandle processHandle;
        private readonly Process process;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinPtyConnection"/> class.
        /// </summary>
        /// <param name="readerStream">The reading side of the pty connection.</param>
        /// <param name="writerStream">The writing side of the pty connection.</param>
        /// <param name="handle">A handle to the winpty instance.</param>
        /// <param name="processHandle">A handle to the spawned process.</param>
        public WinPtyConnection(Stream readerStream, Stream writerStream, IntPtr handle, SafeProcessHandle processHandle)
        {
            ReaderStream = readerStream;
            WriterStream = writerStream;
            Pid = NativeMethods.GetProcessId(processHandle);

            this.handle = handle;
            this.processHandle = processHandle;
            process = Process.GetProcessById(Pid);
            process.Exited += Process_Exited;
            process.EnableRaisingEvents = true;
        }

        /// <inheritdoc/>
        public event EventHandler<PtyExitedEventArgs>? ProcessExited;

        /// <inheritdoc/>
        public Stream ReaderStream { get; }

        /// <inheritdoc/>
        public Stream WriterStream { get; }

        /// <inheritdoc/>
        public int Pid { get; }

        /// <inheritdoc/>
        public int ExitCode => process.ExitCode;

        /// <inheritdoc/>
        public void Dispose()
        {
            ReaderStream?.Dispose();
            WriterStream?.Dispose();

            processHandle.Close();
            winpty_free(handle);
        }

        /// <inheritdoc/>
        public void Kill()
        {
            process.Kill();
        }

        /// <inheritdoc/>
        public void Resize(int cols, int rows)
        {
            winpty_set_size(handle, cols, rows, out var err);
        }

        /// <inheritdoc/>
        public bool WaitForExit(int milliseconds)
        {
            return process.WaitForExit(milliseconds);
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            ProcessExited?.Invoke(this, new PtyExitedEventArgs(process.ExitCode));
        }
    }
}
