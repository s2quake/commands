// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static JSSoft.Terminals.Pty.Linux.NativeMethods;

namespace JSSoft.Terminals.Pty.Linux;

/// <summary>
/// Provides a pty connection for linux machines.
/// </summary>
internal class PtyProvider : Unix.PtyProvider
{
    /// <inheritdoc/>
    public override IPtyConnection StartTerminal(PtyOptions options, TraceSource trace)
    {
        var terminalArgs = GetExecvpArgs(options);
        var controller = 0;
        var isArm64 = RuntimeInformation.ProcessArchitecture == Architecture.Arm64;
        Console.WriteLine($"init");
        var pid = isArm64 == true ?
            NativeMethods.init(ref controller, (ushort)options.Width, (ushort)options.Height) :
            NativeMethodsAmd64.init(ref controller, (ushort)options.Width, (ushort)options.Height);
        if (pid == -1)
        {
            throw new InvalidOperationException($"forkpty(4) failed with error {Marshal.GetLastWin32Error()}");
        }
        Console.WriteLine($"pid: {pid}");
        if (pid == 0)
        {
            // We are in a forked process! See http://man7.org/linux/man-pages/man2/fork.2.html for details.
            // Only our thread is running. We inherited open file descriptors and get a copy of the parent process memory.
            Environment.CurrentDirectory = options.WorkingDirectory;
            Console.WriteLine(terminalArgs.Length);
            if (isArm64 == true)
                NativeMethods.execvpe(options.App, terminalArgs, options.EnvironmentVariables);
            else
                NativeMethodsAmd64.execvpe(options.App, terminalArgs, options.EnvironmentVariables);

            // Unreachable code after execvpe()
        }

        // We have forked the terminal
        return new PtyConnection(controller, pid);
    }
}
