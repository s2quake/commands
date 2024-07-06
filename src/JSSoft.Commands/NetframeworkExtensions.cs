// <copyright file="NetframeworkExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#if NETFRAMEWORK || NETSTANDARD
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Parameter)]
class MaybeNullWhenAttribute : Attribute
{
    public MaybeNullWhenAttribute(bool b)
    {
    }
}

class SwitchExpressionException : InvalidOperationException
{
}

static class NetframeworkExtensions
{
    public static bool TryPeek<T>(this Queue<T> @this, out T? result)
    {
        if (@this.Count == 0)
        {
            result = default;
            return false;
        }

        result = @this.Peek();
        return true;
    }

    public static bool TryDequeue<T>(this Queue<T> @this, out T? result)
    {
        if (@this.Count == 0)
        {
            result = default;
            return false;
        }

        result = @this.Dequeue();
        return true;
    }
}
#endif // NETFRAMEWORK
