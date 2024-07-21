// <copyright file="CommandParameterArrayDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public sealed class CommandParameterArrayDescriptor : CommandMemberDescriptor
{
    private object? _value;

    internal CommandParameterArrayDescriptor(ParameterInfo parameterInfo)
        : base(new CommandPropertyArrayAttribute(), parameterInfo.Name!)
    {
        ThrowUtility.ThrowIfParameterInfoNameNull(parameterInfo);

        _value = parameterInfo.DefaultValue;
        InitValue = Array.CreateInstance(parameterInfo.ParameterType.GetElementType()!, 0);
        MemberType = parameterInfo.ParameterType;
        UsageDescriptor = CommandDescriptor.GetUsageDescriptor(parameterInfo);
        IsNullable = CommandUtility.IsNullable(parameterInfo);
    }

    public override object InitValue { get; }

    public override Type MemberType { get; }

    public override bool IsNullable { get; }

    public override CommandUsageDescriptorBase UsageDescriptor { get; }

    protected override void SetValue(object instance, object? value)
    {
        _value = value;
    }

    protected override object? GetValue(object instance)
    {
        return _value;
    }
}
