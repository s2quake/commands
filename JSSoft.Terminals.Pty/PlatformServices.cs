// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace JSSoft.Terminals.Pty;

/// <summary>
/// Provides platform specific functionality.
/// </summary>
internal static class PlatformServices
{
    private static readonly Lazy<IPtyProvider> WindowsProviderLazy = new(() => new Windows.PtyProvider());
    private static readonly Lazy<IPtyProvider> LinuxProviderLazy = new(() => new Linux.PtyProvider());
    private static readonly Lazy<IPtyProvider> MacProviderLazy = new(() => new Mac.PtyProvider());
    private static readonly Lazy<IPtyProvider> PtyProviderLazy;
    private static readonly Dictionary<string, string> WindowsPtyEnvironment = [];
    private static readonly Dictionary<string, string> UnixPtyEnvironment = new(StringComparer.Ordinal)
    {
        { "TERM", "xterm-256color" },

        // Make sure we didn't start our server from inside tmux.
        { "TMUX", string.Empty },
        { "TMUX_PANE", string.Empty },

        // Make sure we didn't start our server from inside screen.
        // http://web.mit.edu/gnu/doc/html/screen_20.html
        { "STY", string.Empty },
        { "WINDOW", string.Empty },

        // These variables that might confuse our terminal
        { "WINDOWID", string.Empty },
        { "TERMCAP", string.Empty },
        { "COLUMNS", string.Empty },
        { "LINES", string.Empty },
    };

    static PlatformServices()
    {
        if (IsWindows)
        {
            PtyProviderLazy = WindowsProviderLazy;
            EnvironmentVariableComparer = StringComparer.OrdinalIgnoreCase;
            EnvironmentVariables = WindowsPtyEnvironment;
        }
        else if (IsMac)
        {
            PtyProviderLazy = MacProviderLazy;
            EnvironmentVariableComparer = StringComparer.Ordinal;
            EnvironmentVariables = UnixPtyEnvironment;
        }
        else if (IsLinux)
        {
            PtyProviderLazy = LinuxProviderLazy;
            EnvironmentVariableComparer = StringComparer.Ordinal;
            EnvironmentVariables = UnixPtyEnvironment;
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }

    /// <summary>
    /// Gets the <see cref="IPtyProvider"/> for the current platform.
    /// </summary>
    public static IPtyProvider PtyProvider => PtyProviderLazy.Value;

    /// <summary>
    /// Gets the comparer to determine if two environment variable keys are equivalent on the current platform.
    /// </summary>
    public static StringComparer EnvironmentVariableComparer { get; }

    /// <summary>
    /// Gets specific environment variables that are needed when spawning the PTY.
    /// </summary>
    public static IReadOnlyDictionary<string, string> EnvironmentVariables { get; }

    public static string DefaultApp
    {
        get
        {
            if (IsWindows == true)
                return Path.Combine(Environment.SystemDirectory, "cmd.exe");
            if (IsMac == true)
                return "zsh";
            return "bash";
        }
    }

    public static int DefaultWidth => 80;

    public static int DefaultHeight => 22;

    private static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    private static bool IsMac => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    private static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
}
