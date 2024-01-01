// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

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
