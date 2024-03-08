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
