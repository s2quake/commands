#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace JSSoft.Terminals.Pty.Mac;

static class NativeMethods
{
    private const string LibSystem = "runtimes/osx/native/jspty.dylib";
    private static readonly int SizeOfIntPtr = Marshal.SizeOf(typeof(IntPtr));

    [DllImport(LibSystem, SetLastError = true, EntryPoint = "pty_read")]
    public static extern int read(int fd, byte[] buf, int count);

    [DllImport(LibSystem, SetLastError = true, EntryPoint = "pty_write")]
    public static extern int write(int fd, byte[] buf, int count);

    [DllImport(LibSystem, SetLastError = true, EntryPoint = "pty_waitpid")]
    public static extern int waitpid(int pid, ref int status, int options);

    [DllImport(LibSystem, SetLastError = true, EntryPoint = "pty_peek")]
    public static extern int peek(int pid);

    [DllImport(LibSystem, SetLastError = true, EntryPoint = "pty_resize")]
    public static extern int resize(int fd, ushort column, ushort row);

    [DllImport(LibSystem, SetLastError = true, EntryPoint = "pty_close")]
    public static extern int close(int fd);

    [DllImport(LibSystem, EntryPoint = "pty_init")]
    public static extern int init(ref int master, ref Options options);

    [StructLayout(LayoutKind.Sequential)]
    public struct Options
    {
        public ushort Width;
        public ushort Height;
        public string App;
        public IntPtr CommandLine;
        public IntPtr EnvironmentVariables;

        public static IntPtr Marshalling(string[] items)
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
}
