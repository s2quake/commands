// <copyright file="CommandStaticPropertyAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class CommandStaticPropertyAttribute : CommandStaticTypeAttribute
{
    public CommandStaticPropertyAttribute(string staticTypeName, params string[] propertyNames)
        : base(staticTypeName)
    {
        PropertyNames = propertyNames;
    }

    public CommandStaticPropertyAttribute(Type staticType, params string[] propertyNames)
        : base(staticType)
    {
        PropertyNames = propertyNames;
    }

    public string[] PropertyNames { get; }
}
