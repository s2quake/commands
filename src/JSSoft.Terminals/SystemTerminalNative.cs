// <copyright file="SystemTerminalNative.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Runtime.InteropServices;

namespace JSSoft.Terminals;

internal static class SystemTerminalNative
{
    private const int STD_OUTPUT_HANDLE = -11;
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
    private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    public static extern uint GetLastError();

    public static void Initialize()
    {
        if (IsInitialized is false && TerminalEnvironment.IsWindows() is true && Console.IsOutputRedirected is false)
        {
            var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
            if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
            {
                throw new InvalidOperationException("failed to get output console mode");
            }

            outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
            if (SetConsoleMode(iStdOut, outConsoleMode) is false)
            {
                throw new InvalidOperationException($"failed to set output console mode, error code: {GetLastError()}");
            }

            IsInitialized = true;
        }
    }

    public static bool IsInitialized { get; private set; }
}
