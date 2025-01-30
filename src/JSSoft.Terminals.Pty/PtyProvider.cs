// <copyright file="PtyProvider.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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
    /// <returns>A <see cref="Task{IPtyConnection}"/> that completes once the process has spawned.
    /// </returns>
    public static IPtyConnection Spawn(PtyOptions options)
    {
        var actualOptions = new PtyOptions
        {
            App = options.App == string.Empty ? PlatformServices.DefaultApp : options.App,
            CommandLine = options.CommandLine,
            EnvironmentVariables = Merge(
                GetSystemEnvironmentVariables(),
                PlatformServices.EnvironmentVariables,
                options.EnvironmentVariables),
            Height = options.Height is 0 ? PlatformServices.DefaultHeight : options.Height,
            Width = options.Width is 0 ? PlatformServices.DefaultWidth : options.Width,
            WorkingDirectory = options.WorkingDirectory == string.Empty
                ? Environment.CurrentDirectory : options.WorkingDirectory,
        };

        return PlatformServices.PtyProvider.StartTerminal(actualOptions, Trace);
    }

    private static Dictionary<string, string> GetSystemEnvironmentVariables()
    {
        var defaultVariables = Environment.GetEnvironmentVariables();
        var dictionary = new Dictionary<string, string>(
            defaultVariables.Count, StringComparer.OrdinalIgnoreCase);
        foreach (var item in defaultVariables.Keys)
        {
            dictionary.Add($"{item}", $"{defaultVariables[item]}");
        }

        return dictionary;
    }

    private static Dictionary<string, string> Merge(
        params IReadOnlyDictionary<string, string>[] dictionaries)
    {
        var capacity = dictionaries.Sum(item => item.Count);
        var dictionary = new Dictionary<string, string>(
            capacity: capacity,
            comparer: PlatformServices.EnvironmentVariableComparer);

        var valuePairs = dictionaries.SelectMany(item => item);
        foreach (var item in valuePairs)
        {
            dictionary[item.Key] = item.Value;
        }

        return dictionary;
    }
}
