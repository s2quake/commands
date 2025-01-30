// <copyright file="PtyConnection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Pty.Mac;

internal sealed class PtyConnection(int controller, int pid)
    : Unix.PtyConnection(controller, pid)
{
    private readonly int _pid = pid;

    protected override bool Peek(int fd)
        => NativeMethods.peek(fd) == 1;

    protected override int Read(int fd, byte[] buffer, int count)
        => NativeMethods.read(fd, buffer, count);

    protected override int Write(int fd, byte[] buffer, int count)
        => NativeMethods.write(fd, buffer, count);

    protected override bool Kill(int fd)
        => NativeMethods.close(fd) != -1;

    protected override bool Resize(int fd, int cols, int rows)
        => NativeMethods.resize(fd, _pid, (ushort)cols, (ushort)rows) != -1;

    protected override bool WaitPid(int pid, ref int status)
        => NativeMethods.waitpid(pid, ref status, 0) != -1;
}
