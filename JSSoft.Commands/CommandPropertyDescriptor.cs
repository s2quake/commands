// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

namespace JSSoft.Commands;

public sealed class CommandPropertyDescriptor : CommandMemberDescriptor
{
    private readonly PropertyInfo _propertyInfo;
    private readonly CommandPropertyConditionAttribute[] _conditionsAttributes;
    private readonly CommandPropertyCompletionAttribute? _completionAttribute;

    public CommandPropertyDescriptor(PropertyInfo propertyInfo)
        : base(AttributeUtility.GetCustomAttribute<CommandPropertyBaseAttribute>(propertyInfo)!, propertyInfo.Name)
    {
        CommandDefinitionException.ThrowIfPropertyNotReadWrite(propertyInfo);
        CommandDefinitionException.ThrowIfPropertyUnsupportedType(propertyInfo);
        CommandDefinitionException.ThrowIfPropertyNotRightTypeForVariables(CommandType, propertyInfo);
        CommandDefinitionException.ThrowIfPropertyNotRightTypeForSwtich(CommandType, propertyInfo);

        _propertyInfo = propertyInfo;
        _conditionsAttributes = AttributeUtility.GetCustomAttributes<CommandPropertyConditionAttribute>(propertyInfo, inherit: true);
        _completionAttribute = propertyInfo.GetCustomAttribute<CommandPropertyCompletionAttribute>();
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
            var displayName = AttributeUtility.TryGetDisplayName(_propertyInfo, out var v) == true ? v : base.DisplayName;
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

    protected override void OnValidateTrigger(ParseDescriptorCollection parseDescriptors)
    {
        if (_conditionsAttributes.Length != 0 == false)
            return;

        var query = from item in _conditionsAttributes
                    group item by item.Group into @group
                    select @group;

        var memberDescriptorByMemberName = parseDescriptors.ToDictionary(item => item.MemberDescriptor.MemberName, item => item.MemberDescriptor);
        var parseDescriptorByMemberDescriptor = parseDescriptors.ToDictionary(item => item.MemberDescriptor);

        foreach (var group in query)
        {
            foreach (var item in group)
            {
                if (memberDescriptorByMemberName.ContainsKey(item.PropertyName) == false)
                    throw new InvalidOperationException($"Property '{item.PropertyName}' does not exists.");

                var memberDescriptor = memberDescriptorByMemberName[item.PropertyName];
                if (memberDescriptor is not CommandPropertyDescriptor)
                    throw new InvalidOperationException($"'{item.PropertyName}' is not property.");

                var parseDescriptor1 = parseDescriptorByMemberDescriptor[memberDescriptor];
                var value1 = parseDescriptor1.ActualValue;
                var value2 = item.Value;

                if (item.IsInvert == false)
                {
                    if (object.Equals(value1, value2) == false)
                    {
                        if (memberDescriptor.IsSwitch == true)
                            throw new CommandPropertyConditionException($"'{DisplayName}' cannot be used. Cannot be used with switch '{memberDescriptor.DisplayName}'.", this);
                        else
                            throw new CommandPropertyConditionException($"'{DisplayName}' can not use. property '{memberDescriptor.DisplayName}' value must be '{value2:R}'.", this);
                    }
                }
                else
                {
                    if (object.Equals(value1, value2) == true)
                    {
                        if (memberDescriptor.IsSwitch == true)
                            throw new CommandPropertyConditionException($"'{DisplayName}' cannot be used because switch '{memberDescriptor.DisplayName}' is not specified.", this);
                        else
                            throw new CommandPropertyConditionException($"'{DisplayName}' can not use. property '{memberDescriptor.DisplayName}' value must be not '{value2:R}'.", this);
                    }
                }
            }
        }
    }

    protected override string[]? GetCompletion(object instance, string find)
    {
        if (_completionAttribute != null)
            return GetCompletion(instance, find, _completionAttribute);
        return base.GetCompletion(instance, find);
    }
}
