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

namespace JSSoft.Terminals.Pty;

/// <summary>
/// Connection to a running pseudoterminal process.
/// </summary>
public interface IPtyConnection : IDisposable
{
    /// <summary>
    /// Event fired when the pty process exits.
    /// </summary>
    event EventHandler<PtyExitedEventArgs>? Exited;

    bool CanRead => false;

    /// <summary>
    /// Gets the pty process ID.
    /// </summary>
    int Pid { get; }

    /// <summary>
    /// Gets the pty process exit code.
    /// </summary>
    int ExitCode { get; }

    int Read(byte[] buffer, int count);

    int Write(byte[] buffer, int count);

    /// <summary>
    /// Wait for the pty process to exit up to a given timeout.
    /// </summary>
    /// <param name="milliseconds">Timeout to wait for the process to exit.</param>
    /// <returns>True if the process exists within the timeout, false otherwise.</returns>
    bool WaitForExit(int milliseconds);

    /// <summary>
    /// Immediately terminates the pty process.
    /// </summary>
    void Kill();

    /// <summary>
    /// Change the size of the pty.
    /// </summary>
    /// <param name="cols">The number of columns.</param>
    /// <param name="rows">The number of rows.</param>
    void Resize(int cols, int rows);
}
