// <copyright file="RandomUtility.Nullable.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#if JSSOFT_COMMANDS
namespace JSSoft.Commands.Tests;
#endif

#if JSSOFT_TERMINALS
namespace JSSoft.Terminals.Tests;
#endif

/// <summary>
/// Provides methods for generating random nullable values.
/// </summary>
public static partial class RandomUtility
{
    public static T? Nullable<T>(Func<T> generator)
        where T : struct
    {
        if (Boolean() is true)
        {
            return generator();
        }

        return null;
    }

    public static T? NullableObject<T>(Func<T> generator)
        where T : class
    {
        if (Boolean() is true)
        {
            return generator();
        }

        return null;
    }

    public static T?[] NullableArray<T>(Func<T> generator)
        where T : struct
    {
        if (Boolean() is true)
        {
            return [];
        }

        var length = Length();
        var items = new T?[length];
        for (var i = 0; i < length; i++)
        {
            items[i] = Nullable(generator);
        }

        return items;
    }

    public static T?[] NullableObjectArray<T>(Func<T> generator)
        where T : class
    {
        if (Boolean() is true)
        {
            return [];
        }

        var length = Length();
        var items = new T?[length];
        for (var i = 0; i < length; i++)
        {
            items[i] = NullableObject(generator);
        }

        return items;
    }
}
