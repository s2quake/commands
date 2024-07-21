// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JSSoft.Terminals.Pty.Windows;

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static JSSoft.Terminals.Pty.Windows.NativeMethods;

/// <summary>
/// A connection to a pseudoterminal spawned by native windows APIs.
/// </summary>
internal sealed class PseudoConsoleConnection : IPtyConnection
{
    private readonly Process _process;
    private PseudoConsoleConnectionHandles _handles;
    private readonly Stream _readerStream;
    private readonly Stream _writerStream;

    /// <summary>
    /// Initializes a new instance of the <see cref="PseudoConsoleConnection"/> class.
    /// </summary>
    /// <param name="handles">The set of handles associated with the pseudoconsole.</param>
    public PseudoConsoleConnection(PseudoConsoleConnectionHandles handles)
    {
        _readerStream = new AnonymousPipeClientStream(PipeDirection.In, new Microsoft.Win32.SafeHandles.SafePipeHandle(handles.OutPipeOurSide.Handle, ownsHandle: false));
        _writerStream = new AnonymousPipeClientStream(PipeDirection.Out, new Microsoft.Win32.SafeHandles.SafePipeHandle(handles.InPipeOurSide.Handle, ownsHandle: false));

        _handles = handles;
        _process = Process.GetProcessById(Pid);
        _process.Exited += Process_Exited;
        _process.EnableRaisingEvents = true;
    }

    /// <inheritdoc/>
    public event EventHandler<PtyExitedEventArgs>? Exited;

    /// <inheritdoc/>
    public int Pid => _handles.Pid;

    /// <inheritdoc/>
    public int ExitCode => _process.ExitCode;

    /// <inheritdoc/>
    public void Dispose()
    {
        _readerStream?.Dispose();
        _writerStream?.Dispose();

        if (_handles is not null)
        {
            _handles.PseudoConsoleHandle.Close();
            _handles.MainThreadHandle.Close();
            _handles.ProcessHandle.Close();
            _handles.InPipeOurSide.Close();
            _handles.InPipePseudoConsoleSide.Close();
            _handles.OutPipePseudoConsoleSide.Close();
            _handles.OutPipeOurSide.Close();
        }
    }

    /// <inheritdoc/>
    public void Kill()
    {
        _process.Kill();
    }

    /// <inheritdoc/>
    public void Resize(int cols, int rows)
    {
        int hr = ResizePseudoConsole(_handles.PseudoConsoleHandle, new Coord(cols, rows));
        if (hr != S_OK)
        {
            Marshal.ThrowExceptionForHR(hr);
        }
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

    /// <summary>
    /// handles to resources creates when a pseudoconsole is spawned.
    /// </summary>
    internal sealed class PseudoConsoleConnectionHandles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PseudoConsoleConnectionHandles"/> class.
        /// </summary>
        /// <param name="inPipePseudoConsoleSide">the input pipe on the pseudoconsole side.</param>
        /// <param name="outPipePseudoConsoleSide">the output pipe on the pseudoconsole side.</param>
        /// <param name="inPipeOurSide"> the input pipe on the local side.</param>
        /// <param name="outPipeOurSide"> the output pipe on the local side.</param>
        /// <param name="pseudoConsoleHandle">the handle to the pseudoconsole.</param>
        /// <param name="processHandle">the handle to the spawned process.</param>
        /// <param name="pid">the process ID.</param>
        /// <param name="mainThreadHandle">the handle to the main thread.</param>
        public PseudoConsoleConnectionHandles(
            SafePipeHandle inPipePseudoConsoleSide,
            SafePipeHandle outPipePseudoConsoleSide,
            SafePipeHandle inPipeOurSide,
            SafePipeHandle outPipeOurSide,
            SafePseudoConsoleHandle pseudoConsoleHandle,
            SafeProcessHandle processHandle,
            int pid,
            SafeThreadHandle mainThreadHandle)
        {
            InPipePseudoConsoleSide = inPipePseudoConsoleSide;
            OutPipePseudoConsoleSide = outPipePseudoConsoleSide;
            InPipeOurSide = inPipeOurSide;
            OutPipeOurSide = outPipeOurSide;
            PseudoConsoleHandle = pseudoConsoleHandle;
            ProcessHandle = processHandle;
            Pid = pid;
            MainThreadHandle = mainThreadHandle;
        }

        /// <summary>
        /// Gets the input pipe on the pseudoconsole side.
        /// </summary>
        /// <remarks>
        /// This pipe is connected to <see cref="OutPipeOurSide"/>.
        /// </remarks>
        internal SafePipeHandle InPipePseudoConsoleSide { get; }

        /// <summary>
        /// Gets the output pipe on the pseudoconsole side.
        /// </summary>
        /// <remarks>
        /// This pipe is connected to <see cref="InPipeOurSide"/>.
        /// </remarks>
        internal SafePipeHandle OutPipePseudoConsoleSide { get; }

        /// <summary>
        /// Gets the input pipe on the local side.
        /// </summary>
        /// <remarks>
        /// This pipe is connected to <see cref="OutPipePseudoConsoleSide"/>.
        /// </remarks>
        internal SafePipeHandle InPipeOurSide { get; }

        /// <summary>
        /// Gets the output pipe on the local side.
        /// </summary>
        /// <remarks>
        /// This pipe is connected to <see cref="InPipePseudoConsoleSide"/>.
        /// </remarks>
        internal SafePipeHandle OutPipeOurSide { get; }

        /// <summary>
        /// Gets the handle to the pseudoconsole.
        /// </summary>
        internal SafePseudoConsoleHandle PseudoConsoleHandle { get; }

        /// <summary>
        /// Gets the handle to the spawned process.
        /// </summary>
        internal SafeProcessHandle ProcessHandle { get; }

        /// <summary>
        /// Gets the process ID.
        /// </summary>
        internal int Pid { get; }

        /// <summary>
        /// Gets the handle to the main thread.
        /// </summary>
        internal SafeThreadHandle MainThreadHandle { get; }
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
