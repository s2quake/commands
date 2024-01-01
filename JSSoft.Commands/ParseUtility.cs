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

using System.ComponentModel;

namespace JSSoft.Commands;

static class ParseUtility
{
    public static object Parse(CommandMemberDescriptor memberDescriptor, string arg)
    {
        if (memberDescriptor.MemberType.IsArray == true || typeof(System.Collections.IList).IsAssignableFrom(memberDescriptor.MemberType) == true)
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

    public static object ParseArray(CommandMemberDescriptor memberDescriptor, string[] args) => ParseArray(memberDescriptor.MemberType, args);

    public static object ParseArray(Type propertyType, string arg) => ParseArray(propertyType, args: arg.Split([CommandUtility.ItemSperator]));

    public static object ParseArray(Type propertyType, string[] args)
    {
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

    private static object ParseArray(CommandMemberDescriptor memberDescriptor, string arg) => ParseArray(memberDescriptor, arg.Split([CommandUtility.ItemSperator]));

    private static object ParseBoolean(CommandMemberDescriptor memberDescriptor, string arg) => ParseDefault(memberDescriptor, arg);

    private static object ParseEnum(CommandMemberDescriptor memberDescriptor, string arg)
    {
        var segments = arg.Split([CommandUtility.ItemSperator]);
        var names = Enum.GetNames(memberDescriptor.MemberType).ToDictionary(item => CommandUtility.ToSpinalCase(item), item => item);
        var nameList = new List<string>(segments.Length);
        foreach (var item in segments)
        {
            if (names.ContainsKey(item) == false)
                throw new InvalidOperationException($"The value '{arg}' is not available.");

            nameList.Add(names[item]);
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
            var message = $"Value '{arg}' cannot be used for option '{memberDescriptor.DisplayName}'.";
            throw new CommandLineException(error, message, memberDescriptor, e);
        }
    }

    private static Type GetItemType(Type propertyType)
    {
        if (propertyType.IsArray == true)
        {
            if (propertyType.HasElementType == false)
                throw new ArgumentException($"Property '{nameof(Type.HasElementType)}' of '{nameof(Type)}' must be true.", nameof(propertyType));
            return propertyType.GetElementType()!;
        }

        throw new NotSupportedException();
    }
}
