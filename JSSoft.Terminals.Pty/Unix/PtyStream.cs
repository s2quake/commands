// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Win32.SafeHandles;

namespace JSSoft.Terminals.Pty.Unix;

internal sealed class PtyStream(int fd, FileAccess fileAccess)
    : FileStream(new SafeFileHandle((IntPtr)fd, ownsHandle: false), fileAccess, bufferSize: 1024, isAsync: false)
{
    public override bool CanSeek => false;
}
