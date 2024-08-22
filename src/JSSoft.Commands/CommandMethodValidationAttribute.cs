// <copyright file="CommandMethodValidationAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class CommandMethodValidationAttribute : CommandStaticTypeAttribute
{
    public CommandMethodValidationAttribute(string propertyName)
    {
        PropertyName = propertyName;
    }

    public CommandMethodValidationAttribute(string staticTypeName, string propertyName)
        : base(staticTypeName)
    {
        PropertyName = propertyName;
    }

    public CommandMethodValidationAttribute(Type staticType, string propertyName)
        : base(staticType)
    {
        PropertyName = propertyName;
    }

    public string PropertyName { get; }
}
