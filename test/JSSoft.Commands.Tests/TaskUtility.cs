// <copyright file="TaskUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Tests;

public static class TaskUtility
{
    public static Task WaitIfAsync(Func<bool> predicate)
        => Task.Run(() => WaitAction(predicate));

    public static Task WaitIfAsync(Func<bool> predicate, CancellationToken cancellationToken)
        => Task.Run(() => WaitAction(predicate), cancellationToken);

    private static void WaitAction(Func<bool> predicate)
    {
        while (predicate() is true)
        {
            // do nothing
        }
    }
}
