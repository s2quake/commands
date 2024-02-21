// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

namespace JSSoft.Terminals.Pty.Unix;

internal abstract class PtyProvider : IPtyProvider
{
    public abstract IPtyConnection StartTerminal(PtyOptions options, TraceSource trace);

    protected static string?[] GetExecvpArgs(PtyOptions options)
    {
        if (options.CommandLine.Length == 0)
        {
            return [options.App, null];
        }

        var result = new string?[options.CommandLine.Length + 2];
        Array.Copy(options.CommandLine, 0, result, 1, options.CommandLine.Length);
        result[0] = options.App;
        return result;
    }
}
