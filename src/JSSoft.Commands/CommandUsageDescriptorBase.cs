// <copyright file="CommandUsageDescriptorBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public abstract class CommandUsageDescriptorBase(
    CommandUsageAttribute attribute, CommandMemberInfo memberInfo)
{
    public abstract string Summary { get; }

    public abstract string Description { get; }

    public abstract string Example { get; }

    protected CommandUsageAttribute Attribute { get; } = attribute;

    protected CommandMemberInfo MemberInfo { get; } = memberInfo;
}
