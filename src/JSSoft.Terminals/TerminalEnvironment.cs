// <copyright file="TerminalEnvironment.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public static class TerminalEnvironment
{
    public static bool IsWindows()
    {
#if !NETSTANDARD && !NETFRAMEWORK && !NETCOREAPP
        return OperatingSystem.IsWindows();
#else
        return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
#endif
    }

    public static bool IsMacOS()
    {
#if !NETSTANDARD && !NETFRAMEWORK && !NETCOREAPP
        return OperatingSystem.IsMacOS();
#else
        return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);
#endif
    }

    public static bool IsLinux()
    {
#if !NETSTANDARD && !NETFRAMEWORK && !NETCOREAPP
        return OperatingSystem.IsLinux();
#else
        return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
#endif
    }
}
