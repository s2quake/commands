// <copyright file="NativeMethods.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Runtime.InteropServices;
using JSSoft.Terminals.Pty.Unix;

namespace JSSoft.Terminals.Pty.Mac;

internal static class NativeMethods
{
    private const string LibSystem = "runtimes/osx/native/jspty.dylib";

    [DllImport(LibSystem, SetLastError = true, EntryPoint = "pty_read")]
    public static extern int read(int fd, byte[] buf, int count);

    [DllImport(LibSystem, SetLastError = true, EntryPoint = "pty_write")]
    public static extern int write(int fd, byte[] buf, int count);

    [DllImport(LibSystem, SetLastError = true, EntryPoint = "pty_waitpid")]
    public static extern int waitpid(int pid, ref int status, int options);

    [DllImport(LibSystem, SetLastError = true, EntryPoint = "pty_peek")]
    public static extern int peek(int pid);

    [DllImport(LibSystem, SetLastError = true, EntryPoint = "pty_resize")]
    public static extern int resize(int fd, int pid, ushort column, ushort row);

    [DllImport(LibSystem, SetLastError = true, EntryPoint = "pty_close")]
    public static extern int close(int fd);

    [DllImport(LibSystem, EntryPoint = "pty_init")]
    public static extern int init(ref int master, ref PtyNativeOptions options);
}
