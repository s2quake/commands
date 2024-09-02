// <copyright file="RandomUtility.Dictionary.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections.Immutable;
using System.Security;

#if JSSOFT_COMMANDS
namespace JSSoft.Commands.Tests;
#endif

#if JSSOFT_TERMINALS
namespace JSSoft.Terminals.Tests;
#endif

/// <summary>
/// Provides methods for generating random dictionary values.
/// </summary>
public static partial class RandomUtility
{
    public static Dictionary<TKey, TValue?> Dictionary<TKey, TValue>()
        where TKey : notnull
        => Dictionary<TKey, TValue?>(settings: new());

    public static Dictionary<TKey, TValue?> Dictionary<TKey, TValue>(
        DictionarySettings<TKey, TValue> settings)
        where TKey : notnull
    {
        using var depthScope = new DepthScope();
        var count = settings.Count;
        var comparer = settings.Comparer;
        var dictionary = new Dictionary<TKey, TValue?>(count, comparer);
        for (var i = 0; i < count; i++)
        {
            var key = GetKey<TKey>();
            if (dictionary.ContainsKey(key) is true)
            {
                continue;
            }

            dictionary[key] = GetValue<TKey, TValue>(settings);
        }

        return dictionary;
    }

    public static ImmutableDictionary<TKey, TValue?> ImmutableDictionary<TKey, TValue>()
        where TKey : notnull
        => ImmutableDictionary<TKey, TValue?>(settings: new());

    public static ImmutableDictionary<TKey, TValue?> ImmutableDictionary<TKey, TValue>(
        DictionarySettings<TKey, TValue> settings)
        where TKey : notnull
    {
        DictionaryUtility.VerifyKeyType(typeof(TKey));

        using var depthScope = new DepthScope();
        var count = settings.Count;
        var comparer = settings.Comparer;
        var dictionary = new Dictionary<TKey, TValue?>(count, comparer);
        for (var i = 0; i < count; i++)
        {
            var key = GetKey<TKey>();
            if (dictionary.ContainsKey(key) is true)
            {
                continue;
            }

            dictionary[key] = GetValue<TKey, TValue>(settings);
        }

        var builder = System.Collections.Immutable.ImmutableDictionary.CreateBuilder<TKey, TValue?>(
            comparer);
        builder.AddRange(dictionary);
        return builder.ToImmutable();
    }

    private static TKey GetKey<TKey>()
        where TKey : notnull
    {
        if (typeof(TKey) == typeof(string))
        {
            return (TKey)(object)String();
        }

        if (typeof(TKey) == typeof(byte[]))
        {
            return (TKey)(object)Array(Byte);
        }

        if (typeof(TKey) == typeof(object))
        {
            return Boolean() ? (TKey)(object)String() : (TKey)(object)Array(Byte);
        }

        throw new NotSupportedException();
    }

    private static TValue? GetValue<TKey, TValue>(DictionarySettings<TKey, TValue> settings)
        where TKey : notnull
    {
        var isNull = Int32() % 10 == 0 && settings.IsNullable is true;
        if (isNull is true)
        {
            return default;
        }

        if (typeof(TValue) == typeof(object))
        {
            var r = DepthValue.Value < MaxDepth ? Int32(0, 100) : int.MaxValue;
            if (r < 5)
            {
                return (TValue)(object)Dictionary<TKey, TValue>(settings);
            }
            else if (r < 10)
            {
                return (TValue)(object)ImmutableDictionary<TKey, TValue>(settings);
            }
            else
            {
                var key = ValueByType.Keys.Random();
                return (TValue)ValueByType[key]();
            }
        }
        else
        {
            return (TValue)ValueByType[typeof(TValue)]();
        }
    }

    public record class DictionarySettings<TKey, TValue>
        where TKey : notnull
    {
        public int Count { get; init; } = 10;

        public bool IsNullable { get; init; } = true;

        public IEqualityComparer<TKey>? Comparer { get; init; }

        public Type ValueType => typeof(TValue);
    }
}
