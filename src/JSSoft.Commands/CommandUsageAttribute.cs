// <copyright file="CommandUsageAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Class)]
public abstract class CommandUsageAttribute : Attribute
{
    public abstract CommandUsage GetUsage(CommandMemberInfo memberInfo);
}
