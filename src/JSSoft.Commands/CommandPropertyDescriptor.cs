// <copyright file="CommandPropertyDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using static JSSoft.Commands.AttributeUtility;

namespace JSSoft.Commands;

public sealed class CommandPropertyDescriptor : CommandMemberDescriptor
{
    private readonly PropertyInfo _propertyInfo;
    private readonly CommandPropertyConditionAttribute[] _conditionsAttributes;
    private readonly CommandPropertyCompletionAttribute? _completionAttribute;

    public CommandPropertyDescriptor(PropertyInfo propertyInfo)
        : base(GetCustomAttribute<CommandPropertyBaseAttribute>(propertyInfo)!, propertyInfo.Name)
    {
        CommandDefinitionException.ThrowIfPropertyNotReadWrite(propertyInfo);
        CommandDefinitionException.ThrowIfPropertyUnsupportedType(propertyInfo);
        CommandDefinitionException.ThrowIfPropertyNotRightTypeForVariables(
            CommandType, propertyInfo);
        CommandDefinitionException.ThrowIfPropertyNotRightTypeForSwitch(CommandType, propertyInfo);

        _propertyInfo = propertyInfo;
        _conditionsAttributes = GetCustomAttributes<CommandPropertyConditionAttribute>(
            propertyInfo, inherit: true);
        _completionAttribute = GetCustomAttribute<CommandPropertyCompletionAttribute>(propertyInfo);
        MemberType = propertyInfo.PropertyType;
        InitValue = Attribute.InitValue;
        DefaultValue = Attribute.DefaultValue;
        UsageDescriptor = CommandDescriptor.GetUsageDescriptor(propertyInfo);
        IsNullable = CommandUtility.IsNullable(propertyInfo);
    }

    public override string DisplayName
    {
        get
        {
            var propertyInfo = _propertyInfo;
            var displayName
                = TryGetDisplayName(propertyInfo, out var value) == true ? value : base.DisplayName;
            return CommandType == CommandType.Variables ? $"{displayName}..." : displayName;
        }
    }

    public override Type MemberType { get; }

    public override object? InitValue { get; }

    public override object? DefaultValue { get; }

    public override bool IsNullable { get; }

    public override CommandUsageDescriptorBase UsageDescriptor { get; }

    protected override void SetValue(object instance, object? value)
    {
        _propertyInfo.SetValue(instance, value, null);
    }

    protected override object? GetValue(object instance)
    {
        return _propertyInfo.GetValue(instance, null);
    }

    protected override void OnVerifyTrigger(ParseDescriptorCollection parseDescriptors)
    {
        if (_conditionsAttributes.Length == 0)
        {
            return;
        }

        var groups = from item in _conditionsAttributes
                     group item by item.Group into @group
                     select @group;

        foreach (var group in groups)
        {
            foreach (var attribute in group)
            {
                var propertyName = attribute.PropertyName;
                if (parseDescriptors.TryGetValue(propertyName, out var parseDescriptor) != true)
                {
                    var message = $"Property '{attribute.PropertyName}' does not exists.";
                    throw new InvalidOperationException(message);
                }

                var memberDescriptor = parseDescriptor.MemberDescriptor;
                if (memberDescriptor is not CommandPropertyDescriptor)
                {
                    var message = $"'{attribute.PropertyName}' is not property.";
                    throw new InvalidOperationException(message);
                }

                var value1 = attribute.OnSet != true
                    || parseDescriptor.IsOptionSet == true ? parseDescriptor.ActualValue : null;
                var value2 = attribute.Value;

                if (attribute.IsNot != true)
                {
                    if (Equals(value1, value2) != true)
                    {
                        if (memberDescriptor.IsSwitch == true)
                        {
                            var message = $"""
                                '{DisplayName}' cannot be used. Cannot be used with switch 
                                '{memberDescriptor.DisplayName}'.
                                """;
                            throw new CommandPropertyConditionException(message, this);
                        }
                        else
                        {
                            var message = $"""
                                '{DisplayName}' can not use. property 
                                '{memberDescriptor.DisplayName}' value must be '{value2:R}'.
                                """;
                            throw new CommandPropertyConditionException(message, this);
                        }
                    }
                }
                else
                {
                    if (Equals(value1, value2) == true)
                    {
                        if (memberDescriptor.IsSwitch == true)
                        {
                            var message = $"""
                                '{DisplayName}' cannot be used because switch 
                                '{memberDescriptor.DisplayName}' is not specified.
                                """;
                            throw new CommandPropertyConditionException(message, this);
                        }
                        else
                        {
                            var message = $"""
                                '{DisplayName}' can not use. property 
                                '{memberDescriptor.DisplayName}' value must be not '{value2:R}'.
                                """;
                            throw new CommandPropertyConditionException(message, this);
                        }
                    }
                }
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
}
