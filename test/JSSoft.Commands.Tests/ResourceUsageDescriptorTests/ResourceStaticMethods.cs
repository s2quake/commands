// <copyright file="ResourceStaticMethods.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Tests.ResourceUsageDescriptorTests;

[ResourceUsage]
internal static class ResourceStaticMethods
{
    [CommandMethod]
    public static void Method1(string text, int value)
    {
    }

    [CommandMethod]
    public static Task Method2Async(string text, CancellationToken cancellation)
    {
        return Task.CompletedTask;
    }
}
