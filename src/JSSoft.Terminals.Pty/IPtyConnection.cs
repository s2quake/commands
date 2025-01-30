// <copyright file="IPtyConnection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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
