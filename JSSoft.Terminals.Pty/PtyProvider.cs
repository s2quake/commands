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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JSSoft.Terminals.Pty;

/// <summary>
/// Provides the ability to spawn new processes under a pseudoterminal.
/// </summary>
public static class PtyProvider
{
    private static readonly TraceSource Trace = new(nameof(PtyProvider));

    /// <summary>
    /// Spawn a new process connected to a pseudoterminal.
    /// </summary>
    /// <param name="options">The set of options for creating the pseudoterminal.</param>
    /// <param name="cancellationToken">The token to cancel process creation early.</param>
    /// <returns>A <see cref="Task{IPtyConnection}"/> that completes once the process has spawned.</returns>
    public static IPtyConnection Spawn(PtyOptions options)
    {
        var actualOptions = new PtyOptions
        {
            App = options.App == string.Empty ? PlatformServices.DefaultApp : options.App,
            CommandLine = options.CommandLine,
            EnvironmentVariables = Merge(
                GetSystemEnvironmentVariables(),
                PlatformServices.EnvironmentVariables,
                options.EnvironmentVariables
            ),
            Height = options.Height == 0 ? PlatformServices.DefaultHeight : options.Height,
            Width = options.Width == 0 ? PlatformServices.DefaultWidth : options.Width,
            WorkingDirectory = options.WorkingDirectory == string.Empty ? Environment.CurrentDirectory : options.WorkingDirectory,
        };

        return PlatformServices.PtyProvider.StartTerminal(actualOptions, Trace);
    }

    private static Dictionary<string, string> GetSystemEnvironmentVariables()
    {
        var defaultVariables = Environment.GetEnvironmentVariables();
        var dictionary = new Dictionary<string, string>(defaultVariables.Count, StringComparer.OrdinalIgnoreCase);
        foreach (var item in defaultVariables.Keys)
        {
            dictionary.Add($"{item}", $"{defaultVariables[item]}");
        }
        return dictionary;
    }

    private static Dictionary<string, string> Merge(params IReadOnlyDictionary<string, string>[] dictionaries)
    {
        var capacity = dictionaries.Sum(item => item.Count);
        var dictionary = new Dictionary<string, string>(
            capacity: capacity,
            comparer: PlatformServices.EnvironmentVariableComparer
            );

        var valuePairs = dictionaries.SelectMany(item => item);
        foreach (var item in valuePairs)
        {
            dictionary[item.Key] = item.Value;
        }
        return dictionary;
    }
}
