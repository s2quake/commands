// <copyright file="ThrowUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Text.RegularExpressions;

namespace JSSoft.Commands;

internal static partial class ThrowUtility
{
    public static void ThrowIfEmpty(string argument, string paramName)
    {
        if (argument == string.Empty)
        {
            throw new ArgumentException("Empty string is not allowed.", paramName);
        }
    }

    public static void ThrowIfTypeFullNameIsNull(Type type)
        => ThrowIfTypeFullNameIsNull(type, paramName: null);

    public static void ThrowIfTypeFullNameIsNull(Type type, string? paramName)
    {
        if (type.FullName is null)
        {
            var message = $"Property '{nameof(Type.FullName)}' of '{nameof(Type)}' cannot be null.";
            throw new ArgumentException(message, paramName ?? nameof(type));
        }
    }

    public static void ThrowIfParameterInfoNameNull(ParameterInfo parameterInfo)
        => ThrowIfParameterInfoNameNull(parameterInfo, paramName: null);

    public static void ThrowIfParameterInfoNameNull(ParameterInfo parameterInfo, string? paramName)
    {
        if (parameterInfo.Name is null)
        {
            var message = $"""
                '{nameof(ParameterInfo.Name)}' of '{nameof(ParameterInfo)}' cannot be null.
                """;
            throw new ArgumentException(message, paramName ?? nameof(parameterInfo));
        }
    }

    public static void ThrowIfDeclaringTypeNull(MemberInfo memberInfo)
        => ThrowIfDeclaringTypeNull(memberInfo, paramName: null);

    public static void ThrowIfDeclaringTypeNull(MemberInfo memberInfo, string? paramName)
    {
        if (memberInfo.DeclaringType is null)
        {
            var message = $"""
                '{nameof(MemberInfo.DeclaringType)}' of '{memberInfo}' cannot be null.
                """;
            throw new ArgumentException(message, paramName ?? nameof(memberInfo));
        }
    }

    public static void ThrowIfDeclaringTypeNull(ParameterInfo parameterInfo)
        => ThrowIfDeclaringTypeNull(parameterInfo, paramName: null);

    public static void ThrowIfDeclaringTypeNull(ParameterInfo parameterInfo, string? paramName)
    {
        if (parameterInfo.Member.DeclaringType is null)
        {
            var message = $"""
                Property '{nameof(ParameterInfo.Member)}.
                {nameof(ParameterInfo.Member.DeclaringType)}' of '{parameterInfo}' cannot be null.
                """;
            throw new ArgumentException(message, paramName ?? nameof(parameterInfo));
        }
    }

    public static void ThrowIfInvalidShortName(char shortName)
        => ThrowIfInvalidShortName(shortName, paramName: null);

    public static void ThrowIfInvalidShortName(char shortName, string? paramName)
    {
        if (ShortNameRegex().IsMatch(shortName.ToString()) != true)
        {
            var message = $"Short Name can only use alphabetical character.: '{shortName}'.";
            throw new ArgumentException(message, paramName ?? nameof(shortName));
        }
    }

    public static void ThrowIfInvalidName(string name) => ThrowIfInvalidName(name, paramName: null);

    public static void ThrowIfInvalidName(string name, string? paramName)
    {
        if (name.Length < 2)
        {
            var message = $"Name length must be greater than 1.: '{name}'.";
            throw new ArgumentException(message, paramName ?? nameof(name));
        }

        if (IdentifierRegex().IsMatch(name) != true)
        {
            var message = $"Name must be in SpinalCase form.: '{name}'.";
            throw new ArgumentException(message, paramName ?? nameof(name));
        }
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
