// <copyright file="TypeUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

internal static class TypeUtility
{
    public static bool IsStaticClass(Type type)
    {
        return type.GetConstructor(Type.EmptyTypes) is null
            && type.IsAbstract == true
            && type.IsSealed == true;
    }

    public static bool IsStaticClass(string typeName)
        => Type.GetType(typeName) is { } type && IsStaticClass(type) == true;

    public static void ThrowIfTypeIsNotStaticClass(Type type)
    {
        if (IsStaticClass(type) != true)
        {
            var message = $"Type '{type}' is not a static class.";
            throw new ArgumentException(message, nameof(type));
        }
    }

    public static void ThrowIfTypeIsNotStaticClass(string typeName)
    {
        if (Type.GetType(typeName) is Type type != true)
        {
            var message = $"Type '{typeName}' not found.";
            throw new ArgumentException(message, nameof(typeName));
        }

        if (IsStaticClass(type) != true)
        {
            var message = $"Type '{typeName}' is not a static class.";
            throw new ArgumentException(message, nameof(typeName));
        }
    }

    public static void ThrowIfTypeDoesNotHaveInterface(Type type, Type interfaceType)
    {
        if (interfaceType.IsInterface != true)
        {
            var message = $"Type '{interfaceType}' must be a interface.";
            throw new ArgumentException(message, nameof(interfaceType));
        }

        if (interfaceType.IsAssignableFrom(type) != true)
        {
            var message = $"Type '{type}' does not have interface '{interfaceType}' implemented.";
            throw new ArgumentException(message, nameof(type));
        }
    }

    public static void ThrowIfTypeDoesNotHaveInterface(string typeName, Type interfaceType)
    {
        if (Type.GetType(typeName) is null)
        {
            var message = $"'{typeName}' is not a valid type name.";
            throw new ArgumentException(message, nameof(typeName));
        }

        if (interfaceType.IsInterface != true)
        {
            var message = $"Type '{interfaceType}' must be a interface.";
            throw new ArgumentException(message, nameof(interfaceType));
        }

        if (Type.GetType(typeName) is { } type && interfaceType.IsAssignableFrom(type) != true)
        {
            var message = $"Type '{type}' does not have interface '{interfaceType}' implemented.";
            throw new ArgumentException(message, nameof(typeName));
        }
    }

    public static void ThrowIfTypeDoesNotHavePublicConstructor(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) is null)
        {
            var message = $"Type '{type}' does not have a default constructor of public.";
            throw new ArgumentException(message, nameof(type));
        }
    }

    public static void ThrowIfTypeDoesNotHavePublicConstructor(string typeName)
    {
        if (Type.GetType(typeName) is null)
        {
            var message = $"'{typeName}' is not a valid type name.";
            throw new ArgumentException(message, nameof(typeName));
        }

        if (Type.GetType(typeName) is { } type && type.GetConstructor(Type.EmptyTypes) is null)
        {
            var message = $"Type '{type}' does not have a default constructor of public.";
            throw new ArgumentException(message, nameof(typeName));
        }
    }

    public static void ThrowIfTypeDoesNotHavePublicConstructor(Type type, Type[] argumentTypes)
    {
        if (type.GetConstructor(argumentTypes) is null)
        {
            var args = string.Join(", ", argumentTypes.Select(item => item.Name));
            var message = $"Type '{type}' does not have a public constructor with args ({args}).";
            throw new ArgumentException(message, nameof(type));
        }
    }

    public static void ThrowIfTypeDoesNotHavePublicConstructor(
        string typeName, Type[] argumentTypes)
    {
        if (Type.GetType(typeName) is null)
        {
            var message = $"'{typeName}' is not a valid type name.";
            throw new ArgumentException(message, nameof(typeName));
        }

        if (Type.GetType(typeName) is { } type && type.GetConstructor(argumentTypes) is null)
        {
            var args = string.Join(", ", argumentTypes.Select(item => item.Name));
            var message = $"Type '{type}' does not have a public constructor with args ({args}).";
            throw new ArgumentException(message, nameof(typeName));
        }
    }

    public static void ThrowIfTypeIsNotSubclassOf(Type type, Type baseType)
    {
        if (type.IsSubclassOf(baseType) != true)
        {
            var message = $"Type '{type}' is not subclass of '{baseType}'.";
            throw new ArgumentException(message, nameof(type));
        }
    }

    public static void ThrowIfTypeIsNotSubclassOf(string typeName, Type baseType)
    {
        if (Type.GetType(typeName) is null)
        {
            var message = $"'{typeName}' is not a valid type name.";
            throw new ArgumentException(message, nameof(typeName));
        }

        if (Type.GetType(typeName) is { } type && baseType.IsAssignableFrom(type) != true)
        {
            var message = $"Type '{type}' is not subclass of '{baseType}'.";
            throw new ArgumentException(message, nameof(typeName));
        }
    }
}
