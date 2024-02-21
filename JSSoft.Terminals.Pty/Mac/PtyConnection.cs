// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace JSSoft.Terminals.Pty.Mac;

internal class PtyConnection(int controller, int pid)
    : Unix.PtyConnection(controller, pid)
{
    protected override bool Kill(int fd)
    {
        return NativeMethods.close(fd) != -1;
    }

    protected override bool Resize(int fd, int cols, int rows)
    {
        var size = new NativeMethods.WinSize((ushort)rows, (ushort)cols);
        return NativeMethods.ioctl(fd, NativeMethods.TIOCSWINSZ, ref size) != -1;
    }

    protected override bool WaitPid(int pid, ref int status)
    {
        return NativeMethods.waitpid(pid, ref status, 0) != -1;
    }
}
