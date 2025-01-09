// <copyright file="ParseContext.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands;

public sealed class ParseContext
{
    private readonly CommandMemberDescriptorCollection _memberDescriptors;
    private readonly ParseDescriptorCollection _descriptors;
    private readonly ParseDescriptor[] _implicitDescriptors;
    private readonly ParseDescriptor? _variablesDescriptor;
    private readonly List<string> _variableList = [];
    private int _countOfImplicit = 0;
    private bool _isVariableMode;

    public ParseContext(CommandMemberDescriptorCollection memberDescriptors)
    {
        _memberDescriptors = memberDescriptors;
        _descriptors = new ParseDescriptorCollection(memberDescriptors);
        _implicitDescriptors = GetImplicitDescriptors(_descriptors);
        _variablesDescriptor = GetVariableDescriptors(_descriptors);
        Descriptor = _implicitDescriptors.FirstOrDefault() ?? _variablesDescriptor;
    }

    public ParseDescriptor? Descriptor { get; private set; }

    public static ParseContext Create(
        CommandMemberDescriptorCollection memberDescriptors,
        string[] args,
        CommandSettings settings)
    {
        try
        {
            var parseContext = new ParseContext(memberDescriptors);
            parseContext.Next(args, settings);
            return parseContext;
        }
        catch (Exception e)
        {
            throw new CommandLineException("Failed to create a parse context.", e);
        }
    }

    public void Next(string[] args, CommandSettings settings)
    {
        foreach (var arg in args)
        {
            Next(arg, settings);
        }
    }

    public void SetValue(object instance, CommandSettings settings)
    {
        ThrowIfInvalidValue(_descriptors);

        var items = _descriptors;
        var supportInitialize = instance as ISupportInitialize;
        var customCommandDescriptor = instance as ICustomCommandDescriptor;
        foreach (var item in items)
        {
            var memberDescriptor = item.MemberDescriptor;
            if (item.IsValueSet is false && item.IsOptionSet is false)
            {
                continue;
            }

            memberDescriptor.VerifyTrigger(items);
        }

        supportInitialize?.BeginInit();
        foreach (var item in items)
        {
            var memberDescriptor = item.MemberDescriptor;
            var obj = customCommandDescriptor?.GetMemberOwner(memberDescriptor) ?? instance;
            memberDescriptor.SetValueInternal(obj, item.InitValue);
        }

        supportInitialize?.EndInit();
        foreach (var item in items)
        {
            var memberDescriptor = item.MemberDescriptor;
            var obj = customCommandDescriptor?.GetMemberOwner(memberDescriptor) ?? instance;
            var value = item.Value;
            if (value is not DBNull)
            {
                memberDescriptor.SetValueInternal(obj, value);
            }
        }

        foreach (var item in items)
        {
            var memberDescriptor = item.MemberDescriptor;
            var obj = customCommandDescriptor?.GetMemberOwner(memberDescriptor) ?? instance;
            var value = memberDescriptor.GetValueInternal(obj);
            var valueValidator = settings.ValueValidator;
            item.ValidateValue(valueValidator, obj, value);
        }
    }

    public IReadOnlyDictionary<string, object?> GetProperties()
    {
        var properties = new Dictionary<string, object?>(_descriptors.Count);
        foreach (var descriptor in _descriptors)
        {
            var memberDescriptor = descriptor.MemberDescriptor;
            if (descriptor.Value is not DBNull)
            {
                properties.Add(memberDescriptor.MemberName, descriptor.Value);
            }
        }

        return properties;
    }

    private static ParseDescriptor[] GetImplicitDescriptors(
        ParseDescriptorCollection parseDescriptors)
    {
        var query = from item in parseDescriptors
                    let memberDescriptor = item.MemberDescriptor
                    where item.IsRequired is true
                    where item.IsExplicit is false
                    select item;
        return [.. query];
    }

    private static ParseDescriptor? GetVariableDescriptors(
        ParseDescriptorCollection parseDescriptors)
        => parseDescriptors.FirstOrDefault(item => item.MemberDescriptor.IsVariables);

    private static void ThrowIfInvalidValue(ParseDescriptorCollection parseDescriptors)
    {
        foreach (var parseDescriptor in parseDescriptors)
        {
            parseDescriptor.ThrowIfValueMissing();
        }
    }

    private ParseDescriptor? FindNextDescriptor(ParseDescriptor descriptor)
    {
        if (Descriptor == descriptor)
        {
            _countOfImplicit++;
        }

        if (_countOfImplicit < _implicitDescriptors.Length)
        {
            return _implicitDescriptors[_countOfImplicit];
        }

        if (_descriptors.IsRequiredDone() is false)
        {
            return null;
        }

        return _variablesDescriptor;
    }

    private void Next(string arg, CommandSettings settings)
    {
        if (_memberDescriptors.FindByOptionName(arg) is { } memberDescriptor)
        {
            var parseDescriptor = _descriptors[memberDescriptor];
            if (memberDescriptor.IsSwitch is true)
            {
                parseDescriptor.SetSwitchValue(true);
                parseDescriptor.IsOptionSet = true;
                Descriptor = FindNextDescriptor(parseDescriptor);
            }
            else
            {
                if (memberDescriptor.IsRequired is false
                    && memberDescriptor.DefaultValue is not DBNull
                    && _descriptors.IsRequiredDone() is false)
                {
                    var optionName = memberDescriptor.DisplayName;
                    var message = $"'{optionName}'(with a default value) can be used only after " +
                                  $"all required options are set.";
                    throw new ArgumentException(message, nameof(arg));
                }

                if (memberDescriptor.IsRequired is true
                    && memberDescriptor.DefaultValue is not DBNull
                    && _countOfImplicit < _implicitDescriptors.Length)
                {
                    var optionName = memberDescriptor.DisplayName;
                    var message = $"'{optionName}'(with a default value) can be used only after " +
                                  $"all implicit options are set.";
                    throw new ArgumentException(message, nameof(arg));
                }

                parseDescriptor.IsOptionSet = true;
                Descriptor = parseDescriptor;
            }
        }
        else if (arg is "--")
        {
            if (_variablesDescriptor is null)
            {
                var message = "The '--' option cannot be used because the command does not have " +
                              "a variable option.";
                throw new ArgumentException(message, nameof(arg));
            }

            if (_isVariableMode is true)
            {
                var message = "The '--' option cannot be used because the mode is already set to "
                            + "'variable'.";
                throw new ArgumentException(message, nameof(arg));
            }

            if (Descriptor is { IsExplicit: true } descriptor && descriptor.Value is DBNull)
            {
                var optionName = descriptor.MemberDescriptor.DisplayName;
                var message = $"Cannot use '--'. Value of the '{optionName}' must be specified.";
                throw new ArgumentException(message, nameof(arg));
            }

            if (_descriptors.IsRequiredDone() is false)
            {
                var message = "Cannot use '--'. All required options must be set.";
                throw new ArgumentException(message, nameof(arg));
            }

            _isVariableMode = true;
            Descriptor = _variablesDescriptor;
        }
        else if (CommandUtility.IsMultipleSwitch(arg))
        {
            throw new NotSupportedException("Multiple switch is not supported.");
        }
        else if (CommandUtility.IsOption(arg) is true)
        {
            throw new ArgumentException($"Invalid option: '{arg}'", nameof(arg));
        }
        else if (Descriptor is { } descriptor)
        {
            if (descriptor == _variablesDescriptor)
            {
                _variableList.Add(arg);
                descriptor.SetVariablesValue([.. _variableList], settings);
            }
            else
            {
                descriptor.SetValue(arg, settings);
                Descriptor = FindNextDescriptor(descriptor);
            }
        }
        else
        {
            throw new ArgumentException($"Invalid argument: '{arg}'", nameof(arg));
        }
    }
}
