// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace JSSoft.Terminals.Pty.Mac;

sealed class PtyConnection(int controller, int pid)
    : Unix.PtyConnection(controller, pid)
{
    protected override bool Peek(int fd)
        => NativeMethods.peek(fd) == 1;

    protected override int Read(int fd, byte[] buffer, int count)
        => NativeMethods.read(fd, buffer, count);

    protected override int Write(int fd, byte[] buffer, int count)
        => NativeMethods.write(fd, buffer, count);

    protected override bool Kill(int fd)
        => NativeMethods.close(fd) != -1;

    protected override bool Resize(int fd, int cols, int rows)
        => NativeMethods.resize(fd, (ushort)cols, (ushort)rows) != -1;

    protected override bool WaitPid(int pid, ref int status)
        => NativeMethods.waitpid(pid, ref status, 0) != -1;
}
