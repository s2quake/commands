// <copyright file="ParseDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public sealed class ParseDescriptor(CommandMemberDescriptor memberDescriptor)
{
    private object? _value = DBNull.Value;

    public CommandMemberDescriptor MemberDescriptor { get; } = memberDescriptor;

    public bool IsRequired => MemberDescriptor.IsRequired;

    public bool IsExplicit => MemberDescriptor.IsExplicit;

    public bool IsValueSet { get; private set; }

    public bool IsOptionSet { get; internal set; }

    public bool HasValue => _value is not DBNull;

    public object? Value
    {
        get
        {
            if (_value is DBNull)
            {
                if (MemberDescriptor.IsExplicit == true && IsOptionSet == true && MemberDescriptor.DefaultValue is not DBNull)
                    return MemberDescriptor.DefaultValue;
                if (MemberDescriptor.IsExplicit == false && MemberDescriptor.DefaultValue is not DBNull)
                    return MemberDescriptor.DefaultValue;
                if (MemberDescriptor.InitValue is not DBNull)
                    return MemberDescriptor.InitValue;
            }
            return _value;
        }
    }

    public string? TextValue { get; private set; }

    public object? InitValue
    {
        get
        {
            if (MemberDescriptor.InitValue is not DBNull)
                return MemberDescriptor.InitValue;
            if (MemberDescriptor.IsNullable == true)
                return null;
            if (MemberDescriptor.MemberType.IsArray == true)
                return Array.CreateInstance(MemberDescriptor.MemberType.GetElementType()!, 0);
            if (MemberDescriptor.MemberType == typeof(string))
                return string.Empty;
            if (MemberDescriptor.MemberType.IsValueType == true)
                return Activator.CreateInstance(MemberDescriptor.MemberType);
            return null;
        }
    }

    public object? ActualValue => IsValueSet == true ? Value : InitValue;

    internal void SetValue(string textValue)
    {
        _value = ParseUtility.Parse(MemberDescriptor, textValue);
        TextValue = textValue;
        IsValueSet = true;
    }

    internal void SetVariablesValue(IReadOnlyList<string> args)
    {
        var textVariables = args.Select(item => CommandUtility.TryWrapDoubleQuotes(item, out var s) == true ? s : item);
        _value = ParseUtility.ParseArray(MemberDescriptor, [.. args]);
        TextValue = string.Join(" ", textVariables);
        IsValueSet = true;
    }

    internal void SetSwitchValue(bool value)
    {
        _value = value;
        IsValueSet = true;
    }
}
