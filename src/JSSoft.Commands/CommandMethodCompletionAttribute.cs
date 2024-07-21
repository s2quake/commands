// <copyright file="CommandMethodCompletionAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Method)]
public class CommandMethodCompletionAttribute : Attribute
{
    public CommandMethodCompletionAttribute(string methodName)
    {
        MethodName = methodName;
    }

    public CommandMethodCompletionAttribute(string staticTypeName, string methodName)
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
        MethodName = methodName;
    }

    public CommandMethodCompletionAttribute(Type staticType, string methodName)
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
        MethodName = methodName;
    }

    public string MethodName { get; }

    public string StaticTypeName { get; set; } = string.Empty;

    public Type? StaticType { get; set; }
}
