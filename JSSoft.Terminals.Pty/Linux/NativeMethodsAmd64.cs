#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace JSSoft.Terminals.Pty.Linux;

static class NativeMethodsAmd64
{
    private const string PtyLibSystem = "runtimes/linux-x64/native/jspty.so";

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

    public static void execvpe(string file, string?[] args, IReadOnlyDictionary<string, string> environmentVariables)
    {
        foreach (var item in environmentVariables)
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
            setenv(item.Key, item.Value, 1);
        }

        Console.WriteLine($"execvp: {file}: {args.Length}");
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
    private static extern int execvp(
        [MarshalAs(UnmanagedType.LPStr)] string file,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string?[] args);
}
