// <copyright file="RandomUtility.Enum.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections.Immutable;

#if JSSOFT_COMMANDS
namespace JSSoft.Commands.Tests;
#endif

#if JSSOFT_TERMINALS
namespace JSSoft.Terminals.Tests;
#endif

/// <summary>
/// Provides methods for generating random enum values.
/// </summary>
public static partial class RandomUtility
{
    public static T Enum<T>()
        where T : Enum
    {
        if (Attribute.GetCustomAttribute(typeof(Enum), typeof(FlagsAttribute)) is FlagsAttribute)
        {
            throw new InvalidOperationException("flags enum is not supported.");
        }

        var values = System.Enum.GetValues(typeof(T));
        var index = Int32(0, values.Length);
        return (T)values.GetValue(index)!;
    }

    public static T NextEnum<T>()
        where T : Enum
    {
        if (Attribute.GetCustomAttribute(typeof(T), typeof(FlagsAttribute)) is not null)
        {
            throw new InvalidOperationException("Flag type cannot be used.");
        }

        var values = System.Enum.GetValues(typeof(T));
        var index = Int32(0, values.Length);
        return (T)values.GetValue(index)!;
    }

    public static T NextUnspecifiedEnum<T>()
        where T : Enum
    {
        if (Attribute.GetCustomAttribute(typeof(T), typeof(FlagsAttribute)) is not null)
        {
            throw new InvalidOperationException("Flag type cannot be used.");
        }

        var values = System.Enum.GetValues(typeof(T)).OfType<T>().ToArray();
        for (var i = 0; i < AttemptCount; i++)
        {
            var value = GetRandomValue(typeof(T));
            if (values.Contains(value) is false)
            {
                return value;
            }
        }

        throw new InvalidOperationException("No value was found that matches the condition.");

        static T GetRandomValue(Type enumType) => enumType switch
        {
            Type t when System.Enum.GetUnderlyingType(t) == typeof(SByte) => (T)(object)SByte(),
            Type t when System.Enum.GetUnderlyingType(t) == typeof(Byte) => (T)(object)Byte(),
            Type t when System.Enum.GetUnderlyingType(t) == typeof(Int16) => (T)(object)Int16(),
            Type t when System.Enum.GetUnderlyingType(t) == typeof(UInt16) => (T)(object)UInt16(),
            Type t when System.Enum.GetUnderlyingType(t) == typeof(Int32) => (T)(object)Int32(),
            Type t when System.Enum.GetUnderlyingType(t) == typeof(UInt32) => (T)(object)UInt32(),
            Type t when System.Enum.GetUnderlyingType(t) == typeof(Int64) => (T)(object)Int64(),
            Type t when System.Enum.GetUnderlyingType(t) == typeof(UInt64) => (T)(object)UInt64(),
            _ => throw new NotSupportedException(),
        };
    }
}
