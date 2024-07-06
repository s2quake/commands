// <copyright file="TypeUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.Diagnostics;
using System.Linq;

namespace JSSoft.Commands;

static class TypeUtility
{
    public static bool IsStaticClass(Type type)
    {
        return type.GetConstructor(Type.EmptyTypes) == null && type.IsAbstract == true && type.IsSealed == true;
    }

    public static bool IsStaticClass(string typeName)
    {
        return Type.GetType(typeName) is { } type && IsStaticClass(type) == true;
    }

    public static void ThrowIfTypeIsNotStaticClass(Type type)
    {
        if (IsStaticClass(type) == false)
            throw new ArgumentException($"Type '{type}' is not a static class.", nameof(type));
    }

    public static void ThrowIfTypeIsNotStaticClass(string typeName)
    {
        if (Type.GetType(typeName) is Type type == false)
            throw new ArgumentException($"Type '{typeName}' not found.", nameof(typeName));
        if (IsStaticClass(type) == false)
            throw new ArgumentException($"Type '{typeName}' is not a static class.", nameof(typeName));
    }

    public static void ThrowIfTypeDoesNotHaveInterface(Type type, Type interfaceType)
    {
        if (interfaceType.IsInterface == false)
            throw new ArgumentException($"Type '{interfaceType}' must be a interface", nameof(interfaceType));
        if (interfaceType.IsAssignableFrom(type) == false)
            throw new ArgumentException($"Type '{type}' does not have interface '{interfaceType}' implemented.", nameof(type));
    }

    public static void ThrowIfTypeDoesNotHaveInterface(string typeName, Type interfaceType)
    {
        if (Type.GetType(typeName) is null)
            throw new ArgumentException($"'{typeName}' is not a valid type name.", nameof(typeName));
        if (interfaceType.IsInterface == false)
            throw new ArgumentException($"Type '{interfaceType}' must be a interface", nameof(interfaceType));
        if (Type.GetType(typeName) is { } type && interfaceType.IsAssignableFrom(type) == false)
            throw new ArgumentException($"Type '{type}' does not have interface '{interfaceType}' implemented.", nameof(typeName));
    }

    public static void ThrowIfTypeDoesNotHavePublicConstructor(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) == null)
            throw new ArgumentException($"Type '{type}' does not have a default constructor of public.", nameof(type));
    }

    public static void ThrowIfTypeDoesNotHavePublicConstructor(string typeName)
    {
        if (Type.GetType(typeName) is null)
            throw new ArgumentException($"'{typeName}' is not a valid type name.", nameof(typeName));
        if (Type.GetType(typeName) is { } type && type.GetConstructor(Type.EmptyTypes) == null)
            throw new ArgumentException($"Type '{type}' does not have a default constructor of public.", nameof(typeName));
    }

    public static void ThrowIfTypeDoesNotHavePublicConstructor(Type type, Type[] argumentTypes)
    {
        if (type.GetConstructor(argumentTypes) == null)
            throw new ArgumentException($"Type '{type}' does not have a public constructor with args ({string.Join(", ", argumentTypes.Select(item => item.Name))}).", nameof(type));
    }

    public static void ThrowIfTypeDoesNotHavePublicConstructor(string typeName, Type[] argumentTypes)
    {
        if (Type.GetType(typeName) is null)
            throw new ArgumentException($"'{typeName}' is not a valid type name.", nameof(typeName));
        if (Type.GetType(typeName) is { } type && type.GetConstructor(argumentTypes) == null)
            throw new ArgumentException($"Type '{type}' does not have a public constructor with args ({string.Join(", ", argumentTypes.Select(item => item.Name))}).", nameof(typeName));
    }

    public static void ThrowIfTypeIsNotSubclassOf(Type type, Type baseType)
    {
        if (type.IsSubclassOf(baseType) == false)
            throw new ArgumentException($"Type '{type}' is not subclass of '{baseType}'", nameof(type));
    }

    public static void ThrowIfTypeIsNotSubclassOf(string typeName, Type baseType)
    {
        if (Type.GetType(typeName) is null)
            throw new ArgumentException($"'{typeName}' is not a valid type name.", nameof(typeName));
        if (Type.GetType(typeName) is { } type && baseType.IsAssignableFrom(type) == false)
            throw new ArgumentException($"Type '{type}' is not subclass of '{baseType}'", nameof(typeName));
    }
}
