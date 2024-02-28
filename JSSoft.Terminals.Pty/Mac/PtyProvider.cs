// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static JSSoft.Terminals.Pty.Mac.NativeMethods;

namespace JSSoft.Terminals.Pty.Mac;

/// <summary>
/// Provides a pty connection for MacOS machines.
/// </summary>
internal class PtyProvider : Unix.PtyProvider
{
    /// <inheritdoc/>
    public override IPtyConnection StartTerminal(PtyOptions options, TraceSource trace)
    {
        var terminalArgs = GetExecvpArgs(options);
        var controller = 0;
        var pid = init(ref controller, (ushort)options.Cols, (ushort)options.Rows);
        if (pid == -1)
        {
            throw new InvalidOperationException($"forkpty(4) failed with error {Marshal.GetLastWin32Error()}");
        }
        // Console.WriteLine($"pid: {pid} {new StackTrace()}");
        if (pid == 0)
        {
            // We are in a forked process! See http://man7.org/linux/man-pages/man2/fork.2.html for details.
            // Only our thread is running. We inherited open file descriptors and get a copy of the parent process memory.
            Environment.CurrentDirectory = options.Cwd;
            execvpe(options.App, terminalArgs, options.Environment);

            // Unreachable code after execvpe()
        }

        // We have forked the terminal
        return new PtyConnection(controller, pid);
    }
}
