// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Threading;
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
