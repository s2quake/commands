// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace JSSoft.Terminals.Pty.Mac;

sealed class PtyConnection(int controller, int pid)
    : Unix.PtyConnection(controller, pid)
{
    protected override bool Peek(int fd)
    {
        if (IsArm64 == true)
            return NativeMethods.peek(fd) == 0;
        return NativeMethodsAmd64.peek(fd) == 0;
    }

    protected override int Read(int fd, byte[] buffer, int count)
    {
        if (IsArm64 == true)
            return NativeMethods.read(fd, buffer, count);
        return NativeMethodsAmd64.read(fd, buffer, count);
    }

    protected override int Write(int fd, byte[] buffer, int count)
    {
        if (IsArm64 == true)
            return NativeMethods.write(fd, buffer, count);
        return NativeMethodsAmd64.write(fd, buffer, count);
    }

    protected override bool Kill(int fd)
    {
        if (IsArm64 == true)
            return NativeMethods.close(fd) != -1;
        return NativeMethodsAmd64.close(fd) != -1;
    }

    protected override bool Resize(int fd, int cols, int rows)
    {
        if (IsArm64 == true)
            return NativeMethods.resize(fd, (ushort)cols, (ushort)rows) != -1;
        return NativeMethodsAmd64.resize(fd, (ushort)cols, (ushort)rows) != -1;
    }

    protected override bool WaitPid(int pid, ref int status)
    {
        if (IsArm64 == true)
            return NativeMethods.waitpid(pid, ref status, 0) != -1;
        return NativeMethodsAmd64.waitpid(pid, ref status, 0) != -1;
    }

    private static bool IsArm64 => RuntimeInformation.ProcessArchitecture == Architecture.Arm64;
}
