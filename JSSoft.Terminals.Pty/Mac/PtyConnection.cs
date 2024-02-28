// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using static JSSoft.Terminals.Pty.Mac.NativeMethods;

namespace JSSoft.Terminals.Pty.Mac;

internal class PtyConnection(int controller, int pid)
    : Unix.PtyConnection(controller, pid)
{
    protected override bool Peek(int fd)
        => peek(fd) == 0;

    protected override int Read(int fd, byte[] buffer, int count)
        => read(fd, buffer, count);

    protected override void Write(int fd, byte[] buffer, int count)
        => write(fd, buffer, count);

    protected override bool Kill(int fd)
        => close(fd) != -1;

    protected override bool Resize(int fd, int cols, int rows)
        => resize(fd, (ushort)cols, (ushort)rows) != -1;

    protected override bool WaitPid(int pid, ref int status)
    {
        var r = waitpid(pid, ref status, 0);
        return r != -1;
    }
}
