// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JSSoft.Terminals.Pty.Linux;
using static JSSoft.Terminals.Pty.Linux.NativeMethods;

/// <summary>
/// A connection to a pseudoterminal on linux machines.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PtyConnection"/> class.
/// </remarks>
/// <param name="controller">The fd of the pty controller.</param>
/// <param name="pid">The id of the spawned process.</param>
internal class PtyConnection(int controller, int pid)
    : Unix.PtyConnection(controller, pid)
{
    protected override int Read(int fd, byte[] buffer, int count)
        => read(fd, buffer, count);

    protected override void Write(int fd, byte[] buffer, int count)
        => write(fd, buffer, count);

    /// <inheritdoc/>
    protected override bool Kill(int controller)
        => kill(Pid, SIGHUP) != -1;

    /// <inheritdoc/>
    protected override bool Resize(int fd, int cols, int rows)
    {
        var size = new WinSize((ushort)rows, (ushort)cols);
        return ioctl(fd, TIOCSWINSZ, ref size) != -1;
    }

    /// <inheritdoc/>
    protected override bool WaitPid(int pid, ref int status)
        => waitpid(pid, ref status, 0) != -1;
}
