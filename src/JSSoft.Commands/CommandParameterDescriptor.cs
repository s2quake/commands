// <copyright file="CommandParameterDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public sealed class CommandParameterDescriptor : CommandMemberDescriptor
{
    private object? _value;
    private readonly CommandParameterCompletionAttribute? _completionAttribute;

    internal CommandParameterDescriptor(ParameterInfo parameterInfo)
        : base(new CommandPropertyRequiredAttribute(), parameterInfo.Name!)
    {
        ThrowUtility.ThrowIfParameterInfoNameNull(parameterInfo);
        CommandDefinitionException.ThrowIfParameterUnsupportedType(parameterInfo);

        _value = parameterInfo.DefaultValue;
        _completionAttribute = AttributeUtility.GetCustomAttribute<CommandParameterCompletionAttribute>(parameterInfo);
        DefaultValue = parameterInfo.DefaultValue;
        MemberType = parameterInfo.ParameterType;
        UsageDescriptor = CommandDescriptor.GetUsageDescriptor(parameterInfo);
        IsNullable = CommandUtility.IsNullable(parameterInfo);
    }

    public override object? DefaultValue { get; }

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

    protected override string[]? GetCompletion(object instance, string find)
    {
        if (_completionAttribute != null)
            return GetCompletion(instance, find, _completionAttribute);
        return base.GetCompletion(instance, find);
    }
}
