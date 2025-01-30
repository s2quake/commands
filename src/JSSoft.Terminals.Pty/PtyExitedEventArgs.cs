// <copyright file="PtyExitedEventArgs.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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
    /// Gets the exit code of the pty process.
    /// </summary>
    public int ExitCode { get; } = exitCode;
}
