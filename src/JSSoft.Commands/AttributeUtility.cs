// <copyright file="AttributeUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands;

internal static class AttributeUtility
{
    public static T? GetAttribute<T>(Assembly assembly)
        where T : Attribute
        => Attribute.GetCustomAttribute(assembly, typeof(T)) is T attr ? attr : default;

    public static T? GetAttribute<T>(Assembly assembly, bool inherit)
        where T : Attribute
        => Attribute.GetCustomAttribute(assembly, typeof(T), inherit) is T attr ? attr : default;

    public static T? GetAttribute<T>(MemberInfo memberInfo)
        where T : Attribute
        => Attribute.GetCustomAttribute(memberInfo, typeof(T)) is T attr ? attr : default;

    public static T? GetAttribute<T>(MemberInfo memberInfo, bool inherit)
        where T : Attribute
        => Attribute.GetCustomAttribute(memberInfo, typeof(T), inherit) is T attr ? attr : default;

    public static T? GetAttribute<T>(ParameterInfo parameterInfo)
        where T : Attribute
        => Attribute.GetCustomAttribute(parameterInfo, typeof(T)) is T attr ? attr : default;

    public static T? GetAttribute<T>(ParameterInfo parameterInfo, bool inherit)
        where T : Attribute
        => Attribute.GetCustomAttribute(parameterInfo, typeof(T), inherit) is T v ? v : default;

    public static T[] GetAttributes<T>(MemberInfo memberInfo)
        where T : Attribute
        => Attribute.GetCustomAttributes(memberInfo, typeof(T)).OfType<T>().ToArray();

    public static T[] GetAttributes<T>(MemberInfo memberInfo, bool inherit)
        where T : Attribute
        => Attribute.GetCustomAttributes(memberInfo, typeof(T), inherit).OfType<T>().ToArray();

    public static T[] GetAttributes<T>(ParameterInfo parameterInfo)
        where T : Attribute
        => Attribute.GetCustomAttributes(parameterInfo, typeof(T)).OfType<T>().ToArray();

    public static T[] GetAttributes<T>(ParameterInfo parameterInfo, bool inherit)
        where T : Attribute
        => Attribute.GetCustomAttributes(parameterInfo, typeof(T), inherit).OfType<T>().ToArray();

    public static object? GetDefaultValue(MemberInfo memberInfo)
        => GetValue<DefaultValueAttribute, object?>(memberInfo, a => a.Value, DBNull.Value);

    public static object? GetDefaultValue(Type type)
        => GetValue<DefaultValueAttribute, object?>(type, a => a.Value, DBNull.Value);

    public static string GetDescription(MemberInfo memberInfo)
        => GetValue<DescriptionAttribute, string>(memberInfo, a => a.Description, string.Empty);

    public static string GetDisplayName(MemberInfo memberInfo)
        => GetValue<DisplayNameAttribute, string>(memberInfo, a => a.DisplayName, string.Empty);

    public static bool TryGetDisplayName(MemberInfo memberInfo, out string displayName)
    {
        var attribute = GetAttribute<DisplayNameAttribute>(memberInfo);
        if (attribute is not null && attribute.DisplayName != string.Empty)
        {
            displayName = attribute.DisplayName;
            return true;
        }

        displayName = string.Empty;
        return false;
    }

    public static string GetCategory(MemberInfo memberInfo)
        => GetValue<CategoryAttribute, string>(memberInfo, a => a.Category, string.Empty);

#if !JSSOFT_COMMANDS
    public static int GetOrder(MemberInfo memberInfo)
        => GetValue<OrderAttribute, int>(memberInfo, a => a.Order, 0);
#endif // !JSSOFT_COMMANDS

    public static TV GetValue<TA, TV>(
        Assembly assembly, Func<TA, TV> getter, TV defaultValue)
        where TA : Attribute
        => GetAttribute<TA>(assembly) is TA attr ? getter(attr) : defaultValue;

    public static TV GetValue<TA, TV>(
        Assembly assembly, Func<TA, TV> getter, Func<Assembly, TV> defaultGetter)
        where TA : Attribute
        => GetAttribute<TA>(assembly) is TA attr ? getter(attr) : defaultGetter(assembly);

    public static string GetValue<TA>(
        Assembly assembly, Func<TA, string> getter)
        where TA : Attribute
        => GetValue(assembly, getter, string.Empty);

    public static string GetValue<TA>(
        Assembly assembly, Func<TA, string> getter, string defaultValue)
        where TA : Attribute
    {
        if (Attribute.GetCustomAttribute(assembly, typeof(TA)) is TA attribute)
        {
            var value = getter(attribute);
            if (value != string.Empty)
            {
                return value;
            }
        }

        return defaultValue;
    }

    public static string GetValue<TA>(
        Assembly assembly, Func<TA, string> getter, Func<Assembly, string> defaultGetter)
        where TA : Attribute
    {
        if (Attribute.GetCustomAttribute(assembly, typeof(TA)) is TA attribute)
        {
            var value = getter(attribute);
            if (value != string.Empty)
            {
                return value;
            }
        }

        return defaultGetter(assembly);
    }

    public static TV GetValue<TA, TV>(
        MemberInfo memberInfo, Func<TA, TV> getter, TV defaultValue)
        where TA : Attribute
        => GetAttribute<TA>(memberInfo) is TA attr ? getter(attr) : defaultValue;

    public static TV GetValue<TA, TV>(
        MemberInfo memberInfo, Func<TA, TV> getter, Func<MemberInfo, TV> defaultGetter)
        where TA : Attribute
        => GetAttribute<TA>(memberInfo) is TA attr ? getter(attr) : defaultGetter(memberInfo);

    public static string GetValue<TA>(
        MemberInfo memberInfo, Func<TA, string> getter)
        where TA : Attribute
        => GetValue(memberInfo, getter, string.Empty);

    public static string GetValue<TA>(
        MemberInfo memberInfo, Func<TA, string> getter, string defaultValue)
        where TA : Attribute
    {
        if (Attribute.GetCustomAttribute(memberInfo, typeof(TA)) is TA attribute)
        {
            var value = getter(attribute);
            if (value != string.Empty)
            {
                return value;
            }
        }

        return defaultValue;
    }

    public static string GetValue<TA>(
        MemberInfo memberInfo, Func<TA, string> getter, Func<MemberInfo, string> defaultGetter)
        where TA : Attribute
    {
        if (Attribute.GetCustomAttribute(memberInfo, typeof(TA)) is TA attribute)
        {
            var value = getter(attribute);
            if (value != string.Empty)
            {
                return value;
            }
        }

        return defaultGetter(memberInfo);
    }

    public static TV GetValue<TA, TV>(
        ParameterInfo parameterInfo, Func<TA, TV> getter, TV defaultValue)
        where TA : Attribute
        => GetAttribute<TA>(parameterInfo) is TA attr ? getter(attr) : defaultValue;

    public static TV GetValue<TA, TV>(
        ParameterInfo parameterInfo, Func<TA, TV> getter, Func<ParameterInfo, TV> defaultGetter)
        where TA : Attribute
        => GetAttribute<TA>(parameterInfo) is TA attr
            ? getter(attr)
            : defaultGetter(parameterInfo);

    public static string GetValue<TA>(
        ParameterInfo parameterInfo, Func<TA, string> getter)
        where TA : Attribute
        => GetValue(parameterInfo, getter, string.Empty);

    public static string GetValue<TA>(
        ParameterInfo parameterInfo, Func<TA, string> getter, string defaultValue)
        where TA : Attribute
    {
        if (Attribute.GetCustomAttribute(parameterInfo, typeof(TA)) is TA attribute)
        {
            var value = getter(attribute);
            if (value != string.Empty)
            {
                return value;
            }
        }

        return defaultValue;
    }

    public static string GetValue<TA>(
        ParameterInfo parameterInfo,
        Func<TA, string> getter,
        Func<ParameterInfo, string> defaultGetter)
        where TA : Attribute
    {
        if (Attribute.GetCustomAttribute(parameterInfo, typeof(TA)) is TA attribute)
        {
            var value = getter(attribute);
            if (value != string.Empty)
            {
                return value;
            }
        }

        return defaultGetter(parameterInfo);
    }

    public static bool IsDefined<TA>(MemberInfo memberInfo)
        where TA : Attribute
        => Attribute.IsDefined(memberInfo, typeof(TA));

    public static bool IsDefined<TA>(MemberInfo memberInfo, bool inherit)
        where TA : Attribute
        => Attribute.IsDefined(memberInfo, typeof(TA), inherit);

    public static bool IsDefined<TA>(ParameterInfo parameterInfo)
        where TA : Attribute
        => Attribute.IsDefined(parameterInfo, typeof(TA));

    public static bool IsDefined<TA>(ParameterInfo parameterInfo, bool inherit)
        where TA : Attribute
        => Attribute.IsDefined(parameterInfo, typeof(TA), inherit);

    public static bool IsNotDefined<TA>(MemberInfo memberInfo)
        where TA : Attribute
        => !Attribute.IsDefined(memberInfo, typeof(TA));

    public static bool IsNotDefined<TA>(MemberInfo memberInfo, bool inherit)
        where TA : Attribute
        => !Attribute.IsDefined(memberInfo, typeof(TA), inherit);

    public static bool IsNotDefined<TA>(ParameterInfo parameterInfo)
        where TA : Attribute
        => !Attribute.IsDefined(parameterInfo, typeof(TA));

    public static bool IsNotDefined<TA>(ParameterInfo parameterInfo, bool inherit)
        where TA : Attribute
        => !Attribute.IsDefined(parameterInfo, typeof(TA), inherit);
}
