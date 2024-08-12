// <copyright file="ParseUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections;
using System.ComponentModel;

namespace JSSoft.Commands;

internal static class ParseUtility
{
    public static object Parse(CommandMemberDescriptor memberDescriptor, string arg)
    {
        var isListType = typeof(IList).IsAssignableFrom(memberDescriptor.MemberType);
        if (memberDescriptor.MemberType.IsArray == true || isListType == true)
        {
            return ParseArray(memberDescriptor, arg);
        }
        else if (memberDescriptor.MemberType == typeof(bool))
        {
            return ParseBoolean(memberDescriptor, arg);
        }
        else if (memberDescriptor.MemberType.IsEnum)
        {
            return ParseEnum(memberDescriptor, arg);
        }
        else
        {
            return ParseDefault(memberDescriptor, arg);
        }
    }

    public static object ParseArray(CommandMemberDescriptor memberDescriptor, string[] args)
    {
        try
        {
            var propertyType = memberDescriptor.MemberType;
            var itemType = GetItemType(propertyType);
            var array = Array.CreateInstance(itemType, args.Length);
            var converter = TypeDescriptor.GetConverter(itemType);
            for (var i = 0; i < args.Length; i++)
            {
                var value = converter.ConvertFromString(args[i]);
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

    private static object ParseArray(CommandMemberDescriptor memberDescriptor, string arg)
        => ParseArray(memberDescriptor, arg.Split(CommandUtility.ItemSperator));

    private static object ParseBoolean(CommandMemberDescriptor memberDescriptor, string arg)
        => ParseDefault(memberDescriptor, arg);

    private static object ParseEnum(CommandMemberDescriptor memberDescriptor, string arg)
    {
        var items = arg.Split(CommandUtility.ItemSperator);
        var names = Enum.GetNames(memberDescriptor.MemberType)
                        .ToDictionary(CommandUtility.ToSpinalCase, item => item);
        var nameList = new List<string>(items.Length);
        foreach (var item in items)
        {
            if (names.TryGetValue(item, out var value) != true)
            {
                throw new InvalidOperationException($"The value '{arg}' is not available.");
            }

            nameList.Add(value);
        }

        return Enum.Parse(memberDescriptor.MemberType, string.Join(", ", nameList));
    }

    private static object ParseDefault(CommandMemberDescriptor memberDescriptor, string arg)
    {
        try
        {
            var converter = TypeDescriptor.GetConverter(memberDescriptor.MemberType);
            return converter.ConvertFrom(arg)!;
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
        if (propertyType.IsArray == true)
        {
            if (propertyType.HasElementType != true)
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
