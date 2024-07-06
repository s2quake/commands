// <copyright file="CommandMethodStaticPropertyAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CommandMethodStaticPropertyAttribute : Attribute
{
    public CommandMethodStaticPropertyAttribute(string staticTypeName, params string[] propertyNames)
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
        PropertyNames = propertyNames;
    }

    public CommandMethodStaticPropertyAttribute(Type staticType, params string[] propertyNames)
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
        PropertyNames = propertyNames;
    }

    public string[] PropertyNames { get; }

    public string StaticTypeName { get; }

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
