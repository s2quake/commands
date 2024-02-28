// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace JSSoft.Terminals.Pty.Mac;

/// <summary>
/// Defines native types and methods for interop with Mac OS system APIs.
/// </summary>
internal static class NativeMethods
{
    private const string PtyLibSystem = "runtimes/osx-arm64/jspty.dylib";

    [DllImport(PtyLibSystem, SetLastError = true, EntryPoint = "pty_read")]
    public static extern int read(int fd, byte[] buf, int count);

    [DllImport(PtyLibSystem, SetLastError = true, EntryPoint = "pty_write")]
    public static extern int write(int fd, byte[] buf, int count);

    [DllImport(PtyLibSystem, SetLastError = true, EntryPoint = "pty_waitpid")]
    public static extern int waitpid(int pid, ref int status, int options);

    [DllImport(PtyLibSystem, SetLastError = true, EntryPoint = "pty_peek")]
    public static extern int peek(int pid);

    [DllImport(PtyLibSystem, SetLastError = true, EntryPoint = "pty_resize")]
    public static extern int resize(int fd, ushort column, ushort row);

    [DllImport(PtyLibSystem, SetLastError = true, EntryPoint = "pty_close")]
    public static extern int close(int fd);

    [DllImport(PtyLibSystem, EntryPoint = "pty_init")]
    public static extern int init(ref int master, ushort column, ushort row);

    [DllImport(PtyLibSystem, EntryPoint = "pty_setenv")]
    public static extern int setenv(string name, string value, int overwrite);

    public static void execvpe(string file, string?[] args, IDictionary<string, string> environment)
    {
        foreach (var kvp in environment)
        {
            setenv(kvp.Key, kvp.Value, 1);
        }

        if (execvp(file, args) == -1)
        {
            Environment.Exit(Marshal.GetLastWin32Error());
        }
        else
        {
            Environment.Exit(-1);
        }
    }

    [DllImport(PtyLibSystem, SetLastError = true, EntryPoint = "pty_execvp")]
    private static extern int execvp(string file, string?[] args);
}
