// <copyright file="CommandPropertyDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using JSSoft.Commands.Exceptions;
using static JSSoft.Commands.AttributeUtility;

namespace JSSoft.Commands;

public sealed class CommandPropertyDescriptor : CommandMemberDescriptor
{
    private readonly PropertyInfo _propertyInfo;
    private readonly CommandPropertyDependencyAttribute[] _dependencyAttributes;
    private readonly CommandPropertyExclusionAttribute[] _exclusionAttributes;
    private readonly CommandPropertyCompletionAttribute? _completionAttribute;

    public CommandPropertyDescriptor(
        PropertyInfo propertyInfo, CommandPropertyBaseAttribute attribute)
        : base(propertyInfo, attribute, propertyInfo.Name)
    {
        if (propertyInfo.CanWrite is false)
        {
            throw new CommandPropertyNotWritableException(propertyInfo);
        }

        if (propertyInfo.CanRead is false)
        {
            throw new CommandPropertyNotReadableException(propertyInfo);
        }

        if (CommandUtility.IsSupportedType(propertyInfo.PropertyType) is false)
        {
            throw new CommandPropertyNotSupportedTypeException(propertyInfo);
        }

        _propertyInfo = propertyInfo;
        _dependencyAttributes = GetAttributes<CommandPropertyDependencyAttribute>(
            propertyInfo, inherit: true);
        _exclusionAttributes = GetAttributes<CommandPropertyExclusionAttribute>(
            propertyInfo, inherit: true);
        _completionAttribute = GetAttribute<CommandPropertyCompletionAttribute>(propertyInfo);
        MemberType = propertyInfo.PropertyType;
        UsageDescriptor = CommandDescriptor.GetUsageDescriptor(propertyInfo);
        IsRequired = CommandPropertyBaseAttribute.IsRequired(attribute, propertyInfo);
        IsExplicit = CommandPropertyBaseAttribute.IsExplicit(attribute, propertyInfo);
        IsSwitch = CommandPropertyBaseAttribute.IsSwitch(attribute, propertyInfo);
        IsVariables = CommandPropertyBaseAttribute.IsVariables(attribute, propertyInfo);
        IsGeneral = CommandPropertyBaseAttribute.IsGeneral(attribute, propertyInfo);
        IsNullable = CommandUtility.IsNullable(propertyInfo);
        DefaultValue = GetDefaultValue(attribute);
        InitValue = GetInitValue(attribute);
    }

    public override Type MemberType { get; }

    public override object? InitValue { get; }

    public override object? DefaultValue { get; }

    public override bool IsRequired { get; }

    public override bool IsExplicit { get; }

    public override bool IsSwitch { get; }

    public override bool IsVariables { get; }

    public override bool IsGeneral { get; }

    public override bool IsNullable { get; }

    public override CommandUsageDescriptorBase UsageDescriptor { get; }

    protected override void SetValue(object instance, object? value)
        => _propertyInfo.SetValue(instance, value, null);

    protected override object? GetValue(object instance)
        => _propertyInfo.GetValue(instance, null);

    protected override void OnVerifyTrigger(ParseDescriptorCollection parseDescriptors)
    {
        foreach (var propertyName in _dependencyAttributes.Select(item => item.PropertyName))
        {
            if (parseDescriptors.TryGetValue(propertyName, out var parseDescriptor) is false)
            {
                var message = $"Property '{propertyName}' does not exists.";
                throw new InvalidOperationException(message);
            }

            var memberDescriptor = parseDescriptor.MemberDescriptor;
            if (memberDescriptor is not CommandPropertyDescriptor)
            {
                var message = $"'{propertyName}' is not property.";
                throw new InvalidOperationException(message);
            }

            if (parseDescriptor.IsOptionSet is false)
            {
                var message = $"'{DisplayName}' can not use. property " +
                              $"'{memberDescriptor.DisplayName}' is not set.";
                throw new InvalidOperationException(message);
            }
        }

        foreach (var propertyName in _exclusionAttributes.Select(item => item.PropertyName))
        {
            if (parseDescriptors.TryGetValue(propertyName, out var parseDescriptor) is false)
            {
                var message = $"Property '{propertyName}' does not exists.";
                throw new InvalidOperationException(message);
            }

            var memberDescriptor = parseDescriptor.MemberDescriptor;
            if (memberDescriptor is not CommandPropertyDescriptor)
            {
                var message = $"'{propertyName}' is not property.";
                throw new InvalidOperationException(message);
            }

            if (parseDescriptor.IsOptionSet is true)
            {
                var message = $"'{DisplayName}' can not use. property " +
                              $"'{memberDescriptor.DisplayName}' is set.";
                throw new InvalidOperationException(message);
            }
        }
    }

    protected override string[]? GetCompletion(object instance, string find)
    {
        if (_completionAttribute is not null)
        {
            return GetCompletion(instance, find, _completionAttribute);
        }

        return base.GetCompletion(instance, find);
    }

    private static object GetDefaultValue(CommandPropertyBaseAttribute attribute)
    {
        if (attribute is CommandPropertyExplicitRequiredAttribute explicitAttribute)
        {
            return explicitAttribute.DefaultValue;
        }

        if (attribute is CommandPropertyRequiredAttribute requiredAttribute)
        {
            return requiredAttribute.DefaultValue;
        }

        if (attribute is CommandPropertyAttribute generalAttribute)
        {
            return generalAttribute.DefaultValue;
        }

        return DBNull.Value;
    }

    private static object GetInitValue(CommandPropertyBaseAttribute attribute)
    {
        if (attribute is CommandPropertyArrayAttribute arrayAttribute)
        {
            return arrayAttribute.InitValue;
        }

        if (attribute is CommandPropertyAttribute generalAttribute)
        {
            return generalAttribute.InitValue;
        }

        return DBNull.Value;
    }
}
