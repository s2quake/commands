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


namespace JSSoft.Terminals.Pty.Linux;

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
