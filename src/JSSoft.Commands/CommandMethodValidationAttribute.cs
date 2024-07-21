// <copyright file="CommandMethodValidationAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class CommandMethodValidationAttribute : Attribute
{
    public CommandMethodValidationAttribute(string propertyName)
    {
        PropertyName = propertyName;
    }

    public CommandMethodValidationAttribute(string staticTypeName, string propertyName)
    {
        TypeUtility.ThrowIfTypeIsNotStaticClass(staticTypeName);

        StaticTypeName = staticTypeName;
        StaticType = Type.GetType(staticTypeName)!;
        PropertyName = propertyName;
    }

    public CommandMethodValidationAttribute(Type staticType, string propertyName)
    {
        TypeUtility.ThrowIfTypeIsNotStaticClass(staticType);

        StaticType = staticType;
        StaticTypeName = staticType.AssemblyQualifiedName!;
        PropertyName = propertyName;
    }

    public string PropertyName { get; }

    public string StaticTypeName { get; } = string.Empty;

    public Type? StaticType { get; }
}
