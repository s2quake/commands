#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time

using System.Runtime.InteropServices;
using JSSoft.Terminals.Pty.Unix;

namespace JSSoft.Terminals.Pty.Mac;

static class NativeMethods
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
    public static extern int resize(int fd, ushort column, ushort row);

    [DllImport(LibSystem, SetLastError = true, EntryPoint = "pty_close")]
    public static extern int close(int fd);

    [DllImport(LibSystem, EntryPoint = "pty_init")]
    public static extern int init(ref int master, ref PtyNativeOptions options);
}
