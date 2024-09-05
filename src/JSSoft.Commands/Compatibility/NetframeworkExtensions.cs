// <copyright file="NetframeworkExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#if NETFRAMEWORK || NETSTANDARD
#pragma warning disable SA1402
#pragma warning disable SA1649

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace JSSoft.Commands;

internal static class NetframeworkExtensions
{
    public static bool TryPeek<T>(this Queue<T> @this, out T? result)
    {
        if (@this.Count is 0)
        {
            result = default;
            return false;
        }

        result = @this.Peek();
        return true;
    }

    public static bool TryDequeue<T>(this Queue<T> @this, out T? result)
    {
        if (@this.Count is 0)
        {
            result = default;
            return false;
        }

        result = @this.Dequeue();
        return true;
    }
}

[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class MaybeNullWhenAttribute : Attribute
{
    public MaybeNullWhenAttribute(bool b)
    {
    }
}

internal sealed class SwitchExpressionException : InvalidOperationException
{
}
#endif // NETFRAMEWORK
