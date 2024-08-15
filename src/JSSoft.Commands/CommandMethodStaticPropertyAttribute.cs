// <copyright file="CommandMethodStaticPropertyAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CommandMethodStaticPropertyAttribute : CommandStaticTypeAttribute
{
    public CommandMethodStaticPropertyAttribute(
        string staticTypeName, params string[] propertyNames)
        : base(staticTypeName)
    {
        PropertyNames = propertyNames;
    }

    public CommandMethodStaticPropertyAttribute(Type staticType, params string[] propertyNames)
        : base(staticType)
    {
        PropertyNames = propertyNames;
    }

    public string[] PropertyNames { get; }
}
