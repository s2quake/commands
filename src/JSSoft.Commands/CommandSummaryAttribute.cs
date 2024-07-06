// <copyright file="CommandSummaryAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true)]
public class CommandSummaryAttribute(string summary) : Attribute
{
    public virtual string Summary { get; } = summary;

    public string Locale { get; set; } = string.Empty;
}
