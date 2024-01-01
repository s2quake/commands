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

using System.Text.RegularExpressions;

namespace JSSoft.Commands;

static partial class ThrowUtility
{
    public static void ThrowIfEmpty(string argument, string paramName)
    {
        if (argument == string.Empty)
        {
            throw new ArgumentException("Empty string is not allowed.", paramName);
        }
    }

    public static void ThrowIfTypeFullNameIsNull(Type type) => ThrowIfTypeFullNameIsNull(type, paramName: null);

    public static void ThrowIfTypeFullNameIsNull(Type type, string? paramName)
    {
        if (type.FullName == null)
            throw new ArgumentException($"Property '{nameof(Type.FullName)}' of '{nameof(Type)}' cannot be null.", nameof(type));
    }

    public static void ThrowIfParameterInfoNameNull(ParameterInfo parameterInfo) => ThrowIfParameterInfoNameNull(parameterInfo, paramName: null);

    public static void ThrowIfParameterInfoNameNull(ParameterInfo parameterInfo, string? paramName)
    {
        if (parameterInfo.Name == null)
            throw new ArgumentException($"Property '{nameof(ParameterInfo.Name)}' of '{nameof(ParameterInfo)}' cannot be null.", paramName ?? nameof(parameterInfo));
    }

    public static void ThrowIfDeclaringTypeNull(PropertyInfo propertyInfo) => ThrowIfDeclaringTypeNull(propertyInfo, paramName: null);

    public static void ThrowIfDeclaringTypeNull(PropertyInfo propertyInfo, string? paramName)
    {
        if (propertyInfo.DeclaringType == null)
            throw new ArgumentException($"Property '{nameof(PropertyInfo.DeclaringType)}' of '{propertyInfo}' cannot be null.", paramName ?? nameof(propertyInfo));
    }

    public static void ThrowIfDeclaringTypeNull(MethodInfo methodInfo) => ThrowIfDeclaringTypeNull(methodInfo, paramName: null);

    public static void ThrowIfDeclaringTypeNull(MethodInfo methodInfo, string? paramName)
    {
        if (methodInfo.DeclaringType == null)
            throw new ArgumentException($"Property '{nameof(MethodInfo.DeclaringType)}' of '{methodInfo}' cannot be null.", paramName ?? nameof(methodInfo));
    }

    public static void ThrowIfDeclaringTypeNull(ParameterInfo parameterInfo) => ThrowIfDeclaringTypeNull(parameterInfo, paramName: null);

    public static void ThrowIfDeclaringTypeNull(ParameterInfo parameterInfo, string? paramName)
    {
        if (parameterInfo.Member.DeclaringType == null)
            throw new ArgumentException($"Property '{nameof(ParameterInfo.Member)}.{nameof(ParameterInfo.Member.DeclaringType)}' of '{parameterInfo}' cannot be null.", paramName ?? nameof(parameterInfo));
    }

    public static void ThrowIfInvalidShortName(char shortName) => ThrowIfInvalidShortName(shortName, paramName: null);

    public static void ThrowIfInvalidShortName(char shortName, string? paramName)
    {
        if (ShortNameRegex().IsMatch(shortName.ToString()) == false)
            throw new ArgumentException($"Short Name can only use alphabetical character.: '{shortName}'", paramName ?? nameof(shortName));
    }

    public static void ThrowIfInvalidName(string name) => ThrowIfInvalidName(name, paramName: null);

    public static void ThrowIfInvalidName(string name, string? paramName)
    {
        if (name.Length < 2)
            throw new ArgumentException($"Name length must be greater than 1.: '{name}'", paramName ?? nameof(name));
        if (IdentifierRegex().IsMatch(name) == false)
            throw new ArgumentException($"Name must be in SpinalCase form.: '{name}'", paramName ?? nameof(name));
    }

#if NET7_0_OR_GREATER
    [GeneratedRegex("^[a-zA-Z][-_a-zA-Z0-9]+")]
    private static partial Regex IdentifierRegex();

    [GeneratedRegex("[a-zA-Z]")]
    private static partial Regex ShortNameRegex();
#else
    private static Regex IdentifierRegex() => new Regex("^[a-zA-Z][-_a-zA-Z0-9]+");

    private static Regex ShortNameRegex() => new Regex("[a-zA-Z]");
#endif
}
