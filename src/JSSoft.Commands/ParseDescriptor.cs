// <copyright file="ParseDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections;
using System.ComponentModel;

namespace JSSoft.Commands;

public sealed class ParseDescriptor(CommandMemberDescriptor memberDescriptor)
{
    private object? _value = DBNull.Value;

    public CommandMemberDescriptor MemberDescriptor { get; } = memberDescriptor;

    public bool IsRequired => MemberDescriptor.IsRequired;

    public bool IsExplicit => MemberDescriptor.IsExplicit;

    public bool IsValueSet { get; private set; }

    public bool IsOptionSet { get; internal set; }

    public object? Value
    {
        get
        {
            if (_value is not DBNull)
            {
                return _value;
            }

            if (MemberDescriptor.IsExplicit is true
                && IsOptionSet is true
                && MemberDescriptor.DefaultValue is not DBNull)
            {
                return MemberDescriptor.DefaultValue;
            }

            if (MemberDescriptor.IsExplicit is false
                && MemberDescriptor.DefaultValue is not DBNull)
            {
                return MemberDescriptor.DefaultValue;
            }

            if (MemberDescriptor.InitValue is not DBNull)
            {
                return MemberDescriptor.InitValue;
            }

            return _value;
        }
    }

    public string? TextValue { get; private set; }

    public object? InitValue { get; } = GetInitValue(memberDescriptor);

    public override string ToString() => MemberDescriptor.DisplayName;

    internal void ValidateValue(
        ICommandValueValidator valueValidator, object instance, object? value)
    {
        try
        {
            valueValidator.Validate(this, instance, value);
        }
        catch (Exception e)
        {
            throw new CommandLineException(e.Message, MemberDescriptor, e);
        }
    }

    internal void ThrowIfValueMissing()
    {
        if (_value is DBNull && MemberDescriptor.DefaultValue is DBNull)
        {
            if (IsOptionSet is true)
            {
                CommandLineException.ThrowIfValueMissing(MemberDescriptor);
            }

            if (MemberDescriptor.IsRequired is true)
            {
                CommandLineException.ThrowIfValueMissing(MemberDescriptor);
            }
        }
    }

    internal void SetValue(string textValue, CommandSettings settings)
    {
        _value = Parse(MemberDescriptor, textValue, settings);
        TextValue = textValue;
        IsValueSet = true;
    }

    internal void SetVariablesValue(IReadOnlyList<string> args, CommandSettings settings)
    {
        _value = ParseArray(MemberDescriptor, [.. args], settings);
        TextValue = string.Join(" ", args.Select(WrapDoubleQuotes));
        IsValueSet = true;

        static string WrapDoubleQuotes(string text)
            => CommandUtility.TryWrapDoubleQuotes(text, out var s) is true ? s : text;
    }

    internal void SetSwitchValue(bool value)
    {
        _value = value;
        IsValueSet = true;
    }

    private static object? GetInitValue(CommandMemberDescriptor memberDescriptor)
    {
        if (memberDescriptor.InitValue is not DBNull)
        {
            return memberDescriptor.InitValue;
        }

        if (memberDescriptor.IsNullable is true)
        {
            return null;
        }

        if (memberDescriptor.MemberType.IsArray is true)
        {
            return Array.CreateInstance(memberDescriptor.MemberType.GetElementType()!, 0);
        }

        if (memberDescriptor.MemberType == typeof(string))
        {
            return string.Empty;
        }

        if (memberDescriptor.MemberType.IsValueType is true)
        {
            return Activator.CreateInstance(memberDescriptor.MemberType);
        }

        return null;
    }

    private static object Parse(
        CommandMemberDescriptor memberDescriptor, string arg, CommandSettings settings)
    {
        var isListType = typeof(IList).IsAssignableFrom(memberDescriptor.MemberType);
        if (memberDescriptor.MemberType.IsArray is true || isListType is true)
        {
            return ParseArray(memberDescriptor, arg, settings);
        }
        else if (memberDescriptor.MemberType == typeof(bool))
        {
            return ParseBoolean(memberDescriptor, arg, settings);
        }
        else if (memberDescriptor.MemberType.IsEnum)
        {
            return ParseEnum(memberDescriptor, arg);
        }
        else
        {
            return ParseDefault(memberDescriptor, arg, settings);
        }
    }

    private static Array ParseArray(
        CommandMemberDescriptor memberDescriptor, string[] args, CommandSettings settings)
    {
        try
        {
            var propertyType = memberDescriptor.MemberType;
            var itemType = GetItemType(propertyType);
            var array = Array.CreateInstance(itemType, args.Length);
            var context = settings.CreateTypeDescriptorContext();
            var cultureInfo = settings.CultureInfo;
            var converter = TypeDescriptor.GetConverter(itemType);
            for (var i = 0; i < args.Length; i++)
            {
                var value = converter.ConvertFromString(context, cultureInfo, args[i]);
                array.SetValue(value, i);
            }

            return array;
        }
        catch (Exception e)
        {
            var error = CommandLineError.InvalidValue;
            var items = string.Join(", ", args);
            var message = $"Value '{items}' cannot be used for option '{memberDescriptor.Name}'.";
            throw new CommandLineException(error, message, memberDescriptor, e);
        }
    }

    private static Array ParseArray(
        CommandMemberDescriptor memberDescriptor, string arg, CommandSettings settings)
        => ParseArray(memberDescriptor, arg.Split(CommandUtility.ItemSperator), settings);

    private static object ParseBoolean(
        CommandMemberDescriptor memberDescriptor, string arg, CommandSettings settings)
        => ParseDefault(memberDescriptor, arg, settings);

    private static object ParseEnum(CommandMemberDescriptor memberDescriptor, string arg)
    {
        var items = arg.Split(CommandUtility.ItemSperator);
        var names = Enum.GetNames(memberDescriptor.MemberType)
                        .ToDictionary(CommandUtility.ToSpinalCase, item => item);
        var nameList = new List<string>(items.Length);
        foreach (var item in items)
        {
            if (names.TryGetValue(item, out var value) is false)
            {
                throw new InvalidOperationException($"The value '{arg}' is not available.");
            }

            nameList.Add(value);
        }

        return Enum.Parse(memberDescriptor.MemberType, string.Join(", ", nameList));
    }

    private static object ParseDefault(
        CommandMemberDescriptor memberDescriptor, string arg, CommandSettings settings)
    {
        try
        {
            var context = settings.CreateTypeDescriptorContext();
            var cultureInfo = settings.CultureInfo;
            var converter = TypeDescriptor.GetConverter(memberDescriptor.MemberType);
            return converter.ConvertFrom(context, cultureInfo, arg)!;
        }
        catch (Exception e)
        {
            var error = CommandLineError.InvalidValue;
            var message = $"Value '{arg}' cannot be used for option " +
                          $"'{memberDescriptor.DisplayName}'.";
            throw new CommandLineException(error, message, memberDescriptor, e);
        }
    }

    private static Type GetItemType(Type propertyType)
    {
        if (propertyType.IsArray is true)
        {
            if (propertyType.HasElementType is false)
            {
                var message = $"Property '{nameof(Type.HasElementType)}' of '{nameof(Type)}' " +
                              $"must be true.";
                throw new ArgumentException(message, nameof(propertyType));
            }

            return propertyType.GetElementType()!;
        }

        throw new NotSupportedException();
    }
}
