// <copyright file="CommandMethodParameterAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Method)]
public sealed class CommandMethodParameterAttribute(params string[] parameterNames) : Attribute
{
    public string[] ParameterNames { get; } = parameterNames;
}
