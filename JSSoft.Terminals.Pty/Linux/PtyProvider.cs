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

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using JSSoft.Terminals.Pty.Unix;

namespace JSSoft.Terminals.Pty.Linux;

/// <summary>
/// Provides a pty connection for linux machines.
/// </summary>
internal class PtyProvider : Unix.PtyProvider
{
    /// <inheritdoc/>
    public override IPtyConnection StartTerminal(PtyOptions options, TraceSource trace)
    {
        var controller = 0;
        var nativeOptions = new PtyNativeOptions(options);
        var pid = NativeMethods.init(ref controller, ref nativeOptions);
        if (pid == -1)
            throw new InvalidOperationException($"forkpty(4) failed with error {Marshal.GetLastWin32Error()}");

        Console.WriteLine($"pid: {pid}");
        return new PtyConnection(controller, pid);
    }
}
