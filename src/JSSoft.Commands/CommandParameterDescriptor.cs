// <copyright file="CommandParameterDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using static JSSoft.Commands.AttributeUtility;

namespace JSSoft.Commands;

public sealed class CommandParameterDescriptor : CommandMemberDescriptor
{
    private readonly CommandParameterCompletionAttribute? _completionAttribute;
    private object? _value;

    internal CommandParameterDescriptor(ParameterInfo parameterInfo)
        : base(new CommandPropertyRequiredAttribute(), parameterInfo.Name!)
    {
        ThrowUtility.ThrowIfParameterInfoNameNull(parameterInfo);
        CommandDefinitionException.ThrowIfParameterUnsupportedType(parameterInfo);

        _value = parameterInfo.DefaultValue;
        _completionAttribute
            = GetCustomAttribute<CommandParameterCompletionAttribute>(parameterInfo);
        DefaultValue = parameterInfo.DefaultValue;
        MemberType = parameterInfo.ParameterType;
        UsageDescriptor = CommandDescriptor.GetUsageDescriptor(parameterInfo);
        IsNullable = CommandUtility.IsNullable(parameterInfo);
    }

    public override object? DefaultValue { get; }

    public override Type MemberType { get; }

    public override bool IsNullable { get; }

    public override CommandUsageDescriptorBase UsageDescriptor { get; }

    protected override void SetValue(object instance, object? value) => _value = value;

    protected override object? GetValue(object instance) => _value;

    protected override string[]? GetCompletion(object instance, string find)
    {
        if (_completionAttribute is not null)
        {
            return GetCompletion(instance, find, _completionAttribute);
        }

        return base.GetCompletion(instance, find);
    }
}
