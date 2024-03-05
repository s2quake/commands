// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace JSSoft.Terminals.Pty.Unix;

[StructLayout(LayoutKind.Sequential)]
public struct PtyNativeOptions(PtyOptions options)
{
    private static readonly int SizeOfIntPtr = Marshal.SizeOf(typeof(IntPtr));

    public ushort Width = (ushort)options.Width;
    public ushort Height = (ushort)options.Height;
    public string App = options.App;
    public IntPtr CommandLine = Marshalling([options.App]);
    public IntPtr EnvironmentVariables = Marshalling(options.EnvironmentVariables);

    public static IntPtr Marshalling(params string[] items)
    {
        var ppEnv = Marshal.AllocHGlobal((items.Length + 1) * SizeOfIntPtr);
        var offset = 0;
        foreach (var item in items)
        {
            var pEnv = Marshal.StringToHGlobalAnsi(item);
            Marshal.WriteIntPtr(ppEnv, offset, pEnv);
            offset += SizeOfIntPtr;
        }
        Marshal.WriteIntPtr(ppEnv, offset, IntPtr.Zero);
        return ppEnv;
    }

    public static IntPtr Marshalling(IEnumerable<KeyValuePair<string, string>> items)
        => Marshalling(items.Select(item => $"{item.Key}={item.Value}").ToArray());
}
