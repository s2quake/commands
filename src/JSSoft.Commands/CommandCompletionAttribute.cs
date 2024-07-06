// <copyright file="CommandCompletionAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public class CommandCompletionAttribute : Attribute
{
    public CommandCompletionAttribute(string methodName)
    {
        MethodName = methodName;
    }

    public CommandCompletionAttribute(string staticTypeName, string methodName)
    {
        TypeUtility.ThrowIfTypeIsNotStaticClass(staticTypeName);

        StaticTypeName = staticTypeName;
        StaticType = Type.GetType(staticTypeName)!;
        MethodName = methodName;
    }

    public CommandCompletionAttribute(Type staticType, string methodName)
    {
        TypeUtility.ThrowIfTypeIsNotStaticClass(staticType);

        StaticType = staticType;
        StaticTypeName = staticType.AssemblyQualifiedName!;
        MethodName = methodName;
    }

    public string MethodName { get; }

    public string StaticTypeName { get; set; } = string.Empty;

    public Type? StaticType { get; set; }
}
