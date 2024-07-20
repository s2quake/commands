// <copyright file="CommandStaticMethodAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class CommandStaticMethodAttribute : Attribute
{
    public CommandStaticMethodAttribute(string staticTypeName, params string[] methodNames)
    {
        try
        {
            TypeUtility.ThrowIfTypeIsNotStaticClass(staticTypeName);
        }
        catch (Exception e)
        {
            Trace.TraceWarning(e.Message);
        }

        StaticTypeName = staticTypeName;
        StaticType = Type.GetType(staticTypeName)!;
        MethodNames = methodNames;
    }

    public CommandStaticMethodAttribute(Type staticType, params string[] methodNames)
    {
        try
        {
            TypeUtility.ThrowIfTypeIsNotStaticClass(staticType);
        }
        catch (Exception e)
        {
            Trace.TraceWarning(e.Message);
        }

        StaticType = staticType;
        StaticTypeName = staticType.AssemblyQualifiedName!;
        MethodNames = methodNames;
    }

    public string StaticTypeName { get; }

    public string[] MethodNames { get; }

    public Type? StaticType { get; }

    internal Type GetStaticType(Type requestType)
    {
        try
        {
            TypeUtility.ThrowIfTypeIsNotStaticClass(StaticTypeName);
        }
        catch (Exception e)
        {
            throw new CommandDefinitionException(e.Message, requestType, innerException: e);
        }

        return StaticType!;
    }
}
