// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace JSSoft.Terminals.Pty;

/// <summary>
/// Event arguments that encapsulate data about the pty process exit.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PtyExitedEventArgs"/> class.
/// </remarks>
/// <param name="exitCode">Exit code of the pty process.</param>
public class PtyExitedEventArgs(int exitCode) : EventArgs
{
    /// <summary>
    /// Gets or sets the exit code of the pty process.
    /// </summary>
    public int ExitCode { get; } = exitCode;
}
