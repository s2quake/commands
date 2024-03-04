#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace JSSoft.Terminals.Pty.Linux;

static class NativeMethods
{
    private const string LibSystem_arm64 = "runtimes/linux-arm/native/jspty.so";
    private const string LibSystem_x64 = "runtimes/linux-x64/native/jspty.so";

    public static int read(int fd, byte[] buf, int count)
        => IsArm64 ? read_arm64(fd, buf, count) : read_x64(fd, buf, count);

    public static int write(int fd, byte[] buf, int count)
        => IsArm64 ? write_arm64(fd, buf, count) : write_x64(fd, buf, count);

    public static int waitpid(int pid, ref int status, int options)
        => IsArm64 ? waitpid_arm64(pid, ref status, options) : waitpid_x64(pid, ref status, options);

    public static int peek(int pid)
        => IsArm64 ? peek_arm64(pid) : peek_x64(pid);

    public static int resize(int fd, ushort column, ushort row)
        => IsArm64 ? resize_arm64(fd, column, row) : resize_x64(fd, column, row);

    public static int close(int fd)
        => IsArm64 ? close_arm64(fd) : close_x64(fd);

    public static int init(ref int master, ushort column, ushort row)
        => IsArm64 ? ini_arm64(ref master, column, row) : ini_x64(ref master, column, row);

    public static int setenv(string name, string value, int overwrite)
        => IsArm64 ? setenv_arm64(name, value, overwrite) : setenv_x64(name, value, overwrite);

    [DllImport(LibSystem_arm64, SetLastError = true, EntryPoint = "pty_read")]
    private static extern int read_arm64(int fd, byte[] buf, int count);

    [DllImport(LibSystem_arm64, SetLastError = true, EntryPoint = "pty_write")]
    private static extern int write_arm64(int fd, byte[] buf, int count);

    [DllImport(LibSystem_arm64, SetLastError = true, EntryPoint = "pty_waitpid")]
    private static extern int waitpid_arm64(int pid, ref int status, int options);

    [DllImport(LibSystem_arm64, SetLastError = true, EntryPoint = "pty_peek")]
    private static extern int peek_arm64(int pid);

    [DllImport(LibSystem_arm64, SetLastError = true, EntryPoint = "pty_resize")]
    private static extern int resize_arm64(int fd, ushort column, ushort row);

    [DllImport(LibSystem_arm64, SetLastError = true, EntryPoint = "pty_close")]
    private static extern int close_arm64(int fd);

    [DllImport(LibSystem_arm64, EntryPoint = "pty_init")]
    private static extern int ini_arm64(ref int master, ushort column, ushort row);

    [DllImport(LibSystem_arm64, EntryPoint = "pty_setenv")]
    private static extern int setenv_arm64(string name, string value, int overwrite);

    [DllImport(LibSystem_x64, SetLastError = true, EntryPoint = "pty_read")]
    private static extern int read_x64(int fd, byte[] buf, int count);

    [DllImport(LibSystem_x64, SetLastError = true, EntryPoint = "pty_write")]
    private static extern int write_x64(int fd, byte[] buf, int count);

    [DllImport(LibSystem_x64, SetLastError = true, EntryPoint = "pty_waitpid")]
    private static extern int waitpid_x64(int pid, ref int status, int options);

    [DllImport(LibSystem_x64, SetLastError = true, EntryPoint = "pty_peek")]
    private static extern int peek_x64(int pid);

    [DllImport(LibSystem_x64, SetLastError = true, EntryPoint = "pty_resize")]
    private static extern int resize_x64(int fd, ushort column, ushort row);

    [DllImport(LibSystem_x64, SetLastError = true, EntryPoint = "pty_close")]
    private static extern int close_x64(int fd);

    [DllImport(LibSystem_x64, EntryPoint = "pty_init")]
    private static extern int ini_x64(ref int master, ushort column, ushort row);

    [DllImport(LibSystem_x64, EntryPoint = "pty_setenv")]
    private static extern int setenv_x64(string name, string value, int overwrite);

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

    [DllImport(LibSystem_arm64, SetLastError = true, EntryPoint = "pty_execvp")]
    private static extern int execvp(
        [MarshalAs(UnmanagedType.LPStr)] string file,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string?[] args);

    private static bool IsArm64 => RuntimeInformation.ProcessArchitecture == Architecture.Arm64;
}
