// <copyright file="IPtyProvider.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;
using System.Threading.Tasks;

namespace JSSoft.Terminals.Pty;

/// <summary>
/// A provider of pseudoterminal connections.
/// </summary>
public interface IPtyProvider
{
    /// <summary>
    /// Spawns a process as a pseudoterminal.
    /// </summary>
    /// <param name="options">The options for spawning the pty.</param>
    /// <param name="trace">The tracer to trace execution with.</param>
    /// <param name="cancellationToken">A token to cancel the task early.</param>
    /// <returns>A <see cref="Task"/> that completes once the process has spawned.</returns>
    IPtyConnection StartTerminal(
        PtyOptions options,
        TraceSource trace);
}
