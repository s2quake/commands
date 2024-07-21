// <copyright file="DictionaryUtility.cs" company="JSSoft">
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

public static class DictionaryUtility
{
    public static bool IsDictionary(Type type)
    {
        if (type.IsGenericType == true)
        {
            var genericTypeDefinition = type.GetGenericTypeDefinition();
            if (genericTypeDefinition == typeof(Dictionary<,>))
            {
                var genericArguments = type.GetGenericArguments();
                var keyType = genericArguments[0];
                return IsKeyType(keyType);
            }
        }

        return false;
    }

    public static bool IsImmutableDictionary(Type type)
    {
        if (type.IsGenericType == true)
        {
            var genericTypeDefinition = type.GetGenericTypeDefinition();
            if (genericTypeDefinition == typeof(ImmutableDictionary<,>))
            {
                var genericArguments = type.GetGenericArguments();
                var keyType = genericArguments[0];
                return IsKeyType(keyType);
            }
        }

        return false;
    }

    public static bool IsSupportedByteArray(object obj)
    {
        var comparer = GetComparer(obj);
        var x1 = new byte[] { 1, 2, 3 };
        var x2 = new byte[] { 1, 2, 3 };
        var y1 = new byte[] { };
        var y2 = new byte[] { };

        if (comparer is IEqualityComparer<object> objectComparer)
        {
            return objectComparer.Equals(x1, x2) && objectComparer.Equals(x2, x1) &&
                objectComparer.Equals(y1, y2) && objectComparer.Equals(y2, y1);
        }
        else if (comparer is IEqualityComparer<byte[]> bytesComparer)
        {
            return bytesComparer.Equals(x1, x2) && bytesComparer.Equals(x2, x1) &&
                bytesComparer.Equals(y1, y2) && bytesComparer.Equals(y2, y1);
        }

        return false;
    }

    internal static bool IsAssignableFrom(Type baseType, Type? type)
    {
        if (type is null)
        {
            return false;
        }

        if (baseType.IsGenericTypeDefinition == true && type.IsGenericType == true)
        {
            var genericArguments = type.GetGenericArguments();
            if (genericArguments.Length != 2)
            {
                return false;
            }

            if (IsKeyType(genericArguments[0]) != true)
            {
                return false;
            }

            return baseType.MakeGenericType(genericArguments).IsAssignableFrom(type);
        }

        return baseType.IsAssignableFrom(type);
    }

    internal static bool IsKeyType(Type type)
    {
        if (type == typeof(object))
        {
            return true;
        }

        if (type == typeof(string))
        {
            return true;
        }

        if (type == typeof(byte[]))
        {
            return true;
        }

        return false;
    }

    internal static void VerifyKeyType(Type type)
    {
        if (IsKeyType(type) != true)
        {
            throw new ArgumentException($"The key type '{type}' is not supported.");
        }
    }

    internal static object GetComparer(object obj)
    {
        var type = obj.GetType();
        if (IsDictionary(type) == true)
        {
            var propertyName = nameof(Dictionary<object, object>.Comparer);
            var genericArguments = type.GetGenericArguments();
            var genericDictionaryType = typeof(Dictionary<,>).MakeGenericType(genericArguments);
            var propertyInfo = genericDictionaryType.GetProperty(propertyName);
            if (propertyInfo is null)
            {
                throw new InvalidOperationException($"The property '{propertyName}' is not found.");
            }

            return propertyInfo.GetValue(obj)!;
        }

        if (IsImmutableDictionary(type) == true)
        {
            var propertyName = nameof(ImmutableDictionary<object, object>.KeyComparer);
            var arguments = type.GetGenericArguments();
            var dictionaryType = typeof(ImmutableDictionary<,>).MakeGenericType(arguments);
            var propertyInfo = dictionaryType.GetProperty(propertyName);
            if (propertyInfo is null)
            {
                throw new InvalidOperationException($"The property '{propertyName}' is not found.");
            }

            return propertyInfo.GetValue(obj)!;
        }

        throw new NotSupportedException("The object is not a dictionary.");
    }
}
