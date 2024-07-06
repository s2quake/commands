// <copyright file="AttributeUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.Linq;
using System.ComponentModel;
using System.Reflection;

namespace JSSoft.Commands;

static class AttributeUtility
{
    public static T? GetCustomAttribute<T>(MemberInfo memberInfo) where T : Attribute
    {
        if (Attribute.GetCustomAttribute(memberInfo, typeof(T)) is T attribute)
        {
            return attribute;
        }
        return default;
    }

    public static T? GetCustomAttribute<T>(ParameterInfo parameterInfo) where T : Attribute
    {
        if (Attribute.GetCustomAttribute(parameterInfo, typeof(T)) is T attribute)
        {
            return attribute;
        }
        return default;
    }

    public static T? GetCustomAttribute<T>(MemberInfo memberInfo, bool inherit) where T : Attribute
    {
        if (Attribute.GetCustomAttribute(memberInfo, typeof(T), inherit) is T attribute)
        {
            return attribute;
        }
        return default;
    }

    public static T[] GetCustomAttributes<T>(MemberInfo memberInfo) where T : Attribute
    {
        return Attribute.GetCustomAttributes(memberInfo, typeof(T)).OfType<T>().ToArray();
    }

    public static T[] GetCustomAttributes<T>(MemberInfo memberInfo, bool inherit) where T : Attribute
    {
        return Attribute.GetCustomAttributes(memberInfo, typeof(T), inherit).OfType<T>().ToArray();
    }

    public static object? GetDefaultValue(MemberInfo memberInfo)
    {
        if (Attribute.GetCustomAttribute(memberInfo, typeof(DefaultValueAttribute)) is DefaultValueAttribute defaultValueAttribute)
        {
            return defaultValueAttribute.Value;
        }
        return DBNull.Value;
    }

    public static object? GetDefaultValue(Type type)
    {
        if (Attribute.GetCustomAttribute(type, typeof(DefaultValueAttribute)) is DefaultValueAttribute defaultValueAttribute)
        {
            return defaultValueAttribute.Value;
        }
        return DBNull.Value;
    }

    public static string GetDescription(MemberInfo memberInfo)
    {
        if (Attribute.GetCustomAttribute(memberInfo, typeof(DescriptionAttribute)) is DescriptionAttribute descriptionAttribute)
        {
            return descriptionAttribute.Description;
        }
        return string.Empty;
    }

    public static string GetDisplayName(MemberInfo memberInfo)
    {
        if (Attribute.GetCustomAttribute(memberInfo, typeof(DisplayNameAttribute)) is DisplayNameAttribute displayNameAttribute)
        {
            return displayNameAttribute.DisplayName;
        }
        return string.Empty;
    }

    public static bool TryGetDisplayName(MemberInfo memberInfo, out string displayName)
    {
        if (Attribute.GetCustomAttribute(memberInfo, typeof(DisplayNameAttribute)) is DisplayNameAttribute displayNameAttribute && displayNameAttribute.DisplayName != string.Empty)
        {
            displayName = displayNameAttribute.DisplayName;
            return true;
        }
        displayName = string.Empty;
        return false;
    }

    public static bool GetBrowsable(MemberInfo memberInfo)
    {
        if (Attribute.GetCustomAttribute(memberInfo, typeof(BrowsableAttribute)) is BrowsableAttribute browsableAttribute)
        {
            return browsableAttribute.Browsable;
        }
        return true;
    }

    public static string GetCategory(MemberInfo memberInfo)
    {
        if (Attribute.GetCustomAttribute(memberInfo, typeof(CategoryAttribute)) is CategoryAttribute categoryAttribute)
        {
            return categoryAttribute.Category;
        }
        return string.Empty;
    }

#if !JSSOFT_COMMANDS
    public static int GetOrder(MemberInfo memberInfo)
    {
        if (Attribute.GetCustomAttribute(memberInfo, typeof(OrderAttribute)) is OrderAttribute orderAttribute)
        {
            return orderAttribute.Order;
        }
        return 0;
    }
#endif // !JSSOFT_COMMANDS
}
