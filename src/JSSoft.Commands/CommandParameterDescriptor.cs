// <copyright file="CommandParameterDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using JSSoft.Commands.Exceptions;
using static JSSoft.Commands.AttributeUtility;

namespace JSSoft.Commands;

public sealed class CommandParameterDescriptor : CommandMemberDescriptor
{
    private readonly CommandParameterCompletionAttribute? _completionAttribute;
    private object? _value;

    internal CommandParameterDescriptor(
        ParameterInfo parameterInfo, CommandParameterBaseAttribute attribute)
        : base(parameterInfo, attribute, parameterInfo.Name!)
    {
        if (parameterInfo.Name is null)
        {
            throw new CommandParameterNameNullException(parameterInfo);
        }

        _value = parameterInfo.DefaultValue;
        _completionAttribute
            = GetAttribute<CommandParameterCompletionAttribute>(parameterInfo);
        MemberType = parameterInfo.ParameterType;
        UsageDescriptor = CommandDescriptor.GetUsageDescriptor(parameterInfo);
        IsRequired = attribute is not CommandParameterArrayAttribute;
        IsSwitch = parameterInfo.HasDefaultValue is true
            && parameterInfo.ParameterType == typeof(bool);
        IsExplicit = IsSwitch;
        IsVariables = attribute is CommandParameterArrayAttribute;
        IsGeneral = false;
        IsNullable = CommandUtility.IsNullable(parameterInfo);
        DefaultValue = parameterInfo.DefaultValue;
        InitValue = GetInitValue(parameterInfo, attribute);
    }

    public override Type MemberType { get; }

    public override object? InitValue { get; } = DBNull.Value;

    public override object? DefaultValue { get; }

    public override bool IsRequired { get; }

    public override bool IsExplicit { get; }

    public override bool IsSwitch { get; }

    public override bool IsVariables { get; }

    public override bool IsGeneral { get; }

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

    private static object GetInitValue(
        ParameterInfo parameterInfo, CommandParameterBaseAttribute attribute)
    {
        if (attribute is CommandParameterArrayAttribute)
        {
            return Array.CreateInstance(parameterInfo.ParameterType.GetElementType()!, 0);
        }

        return DBNull.Value;
    }
}
