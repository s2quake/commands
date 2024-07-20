// <copyright file="ThrowUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable CA1513

namespace JSSoft.Terminals;

internal static class ThrowUtility
{
    public static void ThrowObjectDisposedException(bool condition, IDisposable instance)
    {
        if (condition == true)
        {
            throw new ObjectDisposedException(instance.GetType().FullName);
        }
    }
}
