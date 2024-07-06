// <copyright file="CommandMemberCompletionAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;

namespace JSSoft.Commands;

public abstract class CommandMemberCompletionAttribute : Attribute
{
    protected CommandMemberCompletionAttribute(string methodName)
    {
        MethodName = methodName;
    }

    protected CommandMemberCompletionAttribute(string staticTypeName, string methodName)
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

    protected CommandMemberCompletionAttribute(Type staticType, string methodName)
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
