﻿// <copyright file="PtyProvider.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using JSSoft.Terminals.Pty.Unix;

namespace JSSoft.Terminals.Pty.Mac;

/// <summary>
/// Provides a pty connection for MacOS machines.
/// </summary>
internal class PtyProvider : Unix.PtyProvider
{
    /// <inheritdoc/>
    public override IPtyConnection StartTerminal(PtyOptions options, TraceSource trace)
    {
        var controller = 0;
        var nativeOptions = new PtyNativeOptions(options);
        var pid = NativeMethods.init(ref controller, ref nativeOptions);
        if (pid == -1)
        {
            throw new InvalidOperationException(
                $"forkpty(4) failed with error {Marshal.GetLastWin32Error()}");
        }

        Console.WriteLine($"pid: {pid}");
        return new PtyConnection(controller, pid);
    }
}
