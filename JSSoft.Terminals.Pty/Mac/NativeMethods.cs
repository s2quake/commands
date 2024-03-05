// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
