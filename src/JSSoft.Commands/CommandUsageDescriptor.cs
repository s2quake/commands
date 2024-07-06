// <copyright file="CommandUsageDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;

namespace JSSoft.Commands;

sealed class CommandUsageDescriptor : CommandUsageDescriptorBase
{
    public CommandUsageDescriptor(object target)
        : base(new CommandUsageAttribute(), target)
    {
        if (target is Type type)
        {
            Summary = CommandAttributeUtility.GetSummary(type);
            Description = CommandAttributeUtility.GetDescription(type);
            Example = CommandAttributeUtility.GetExample(type);
        }
        else if (target is MemberInfo memberInfo)
        {
            Summary = CommandAttributeUtility.GetSummary(memberInfo);
            Description = CommandAttributeUtility.GetDescription(memberInfo);
            Example = CommandAttributeUtility.GetExample(memberInfo);
        }
        else if (target is MethodInfo methodInfo)
        {
            Summary = CommandAttributeUtility.GetSummary(methodInfo);
            Description = CommandAttributeUtility.GetDescription(methodInfo);
            Example = CommandAttributeUtility.GetExample(methodInfo);
        }
        else if (target is ParameterInfo parameterInfo)
        {
            Summary = CommandAttributeUtility.GetSummary(parameterInfo);
            Description = CommandAttributeUtility.GetDescription(parameterInfo);
            Example = CommandAttributeUtility.GetExample(parameterInfo);
        }
        else
        {
            throw new UnreachableException();
        }
    }

    public override string Summary { get; }

    public override string Description { get; }

    public override string Example { get; }
}
