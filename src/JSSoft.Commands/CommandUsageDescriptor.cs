// <copyright file="CommandUsageDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

internal sealed class CommandUsageDescriptor(CommandMemberInfo memberInfo)
    : CommandUsageDescriptorBase(new CommandUsageAttribute(), memberInfo)
{
    private readonly CommandMemberInfo _memberInfo = memberInfo;

    public override string Summary => _memberInfo.Summary;

    public override string Description => _memberInfo.Description;

    public override string Example => _memberInfo.Example;
}
