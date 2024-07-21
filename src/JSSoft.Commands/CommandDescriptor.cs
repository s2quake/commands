// <copyright file="CommandDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

// Reflection should not be used to increase accessibility of classes, methods, or fields
#pragma warning disable S3011

using static JSSoft.Commands.AttributeUtility;
using static JSSoft.Commands.CommandAttributeUtility;
using static JSSoft.Commands.CommandMethodUtility;

namespace JSSoft.Commands;

public static class CommandDescriptor
{
    private static readonly Dictionary<MemberInfo, CommandMemberDescriptorCollection>
        MemberDescriptorsByMemberInfo = [];

    private static readonly Dictionary<MemberInfo, CommandMethodDescriptorCollection>
        MethodDescriptorsByMemberInfo = [];

    private static readonly Dictionary<object, CommandUsageDescriptorBase>
        UsageDescriptorByMemberInfo = [];

    public static CommandUsageDescriptorBase GetUsageDescriptor(MemberInfo memberInfo)
    {
        if (GetUsageDescriptor(memberInfo, memberInfo) is { } usageDescriptor1)
        {
            return usageDescriptor1;
        }
        else if (memberInfo.DeclaringType is not null
            && GetUsageDescriptor(memberInfo.DeclaringType, memberInfo) is { } usageDescriptor2)
        {
            return usageDescriptor2;
        }

        if (UsageDescriptorByMemberInfo.TryGetValue(memberInfo, out var value) != true)
        {
            value = new CommandUsageDescriptor(memberInfo);
            UsageDescriptorByMemberInfo.Add(memberInfo, value);
        }

        return value;
    }

    public static CommandUsageDescriptorBase GetUsageDescriptor(ParameterInfo parameterInfo)
    {
        if (GetUsageDescriptor(parameterInfo, parameterInfo) is { } usageDescriptor1)
        {
            return usageDescriptor1;
        }
        else if (GetUsageDescriptor(parameterInfo.Member, parameterInfo) is { } usageDescriptor2)
        {
            return usageDescriptor2;
        }

        if (UsageDescriptorByMemberInfo.TryGetValue(parameterInfo, out var value) != true)
        {
            value = new CommandUsageDescriptor(parameterInfo);
            UsageDescriptorByMemberInfo.Add(parameterInfo, value);
        }

        return value;
    }

    public static CommandMethodDescriptorCollection GetMethodDescriptors(Type type)
    {
        if (MethodDescriptorsByMemberInfo.TryGetValue(type, out var value) != true)
        {
            value = CreateMethodDescriptors(type);
            MethodDescriptorsByMemberInfo.Add(type, value);
        }

        return value;
    }

    public static CommandMemberDescriptorCollection GetMemberDescriptors(object obj)
    {
        if (obj is ICustomCommandDescriptor customCommandDescriptor)
        {
            return customCommandDescriptor.GetMembers();
        }
        else if (obj is Type type)
        {
            return GetMemberDescriptors(type);
        }
        else
        {
            return GetMemberDescriptors(obj.GetType());
        }
    }

    public static CommandMemberDescriptorCollection GetMemberDescriptors(Type type)
    {
        if (MemberDescriptorsByMemberInfo.TryGetValue(type, out var value) != true)
        {
            value = CreateMemberDescriptors(type);
            MemberDescriptorsByMemberInfo.Add(type, value);
        }

        return value;
    }

    public static CommandMemberDescriptorCollection GetMemberDescriptors(
        Type type, string methodName)
    {
        var bindingFlags = BindingFlags.Static
            | BindingFlags.Instance
            | BindingFlags.Public
            | BindingFlags.NonPublic;
        var methodInfo = type.GetMethod(methodName, bindingFlags);
        if (methodInfo is null)
        {
            var message = $"Type '{type}' does not have method '{methodName}'.";
            throw new ArgumentException(message, nameof(methodName));
        }

        return GetMemberDescriptorsByMethodInfo(methodInfo);
    }

    public static CommandMemberDescriptorCollection GetMemberDescriptors(
        object obj, string methodName) => GetMemberDescriptors(obj.GetType(), methodName);

    internal static CommandMemberDescriptorCollection GetMemberDescriptorsByMethodInfo(
        MethodInfo methodInfo)
    {
        if (MemberDescriptorsByMemberInfo.TryGetValue(methodInfo, out var value) != true)
        {
            value = CreateMemberDescriptors(methodInfo);
            MemberDescriptorsByMemberInfo.Add(methodInfo, value);
        }

        return value;
    }

    private static CommandMethodDescriptor[] GetStaticMethodDescriptors(Type requestType)
    {
        var attributes = GetCustomAttributes<CommandStaticMethodAttribute>(
            requestType, inherit: true);
        if (attributes.Length > 0 && TypeUtility.IsStaticClass(requestType) == true)
        {
            var message = $"""
                Attribute '{nameof(CommandStaticMethodAttribute)}' is not available because type 
                '{requestType}' is a static type.
                """;
            throw new CommandDefinitionException(message, requestType);
        }

        var methodDescriptorCollectionList = new List<CommandMethodDescriptorCollection>(
            attributes.Length);
        for (var i = 0; i < attributes.Length; i++)
        {
            var attribute = attributes[i];
            var staticType = attribute.GetStaticType(requestType);
            var staticMethodDescriptors = GetMethodDescriptors(staticType);
            methodDescriptorCollectionList.Add(staticMethodDescriptors);
        }

        var capacity = methodDescriptorCollectionList.Sum(item => item.Count);
        var methodDescriptorList = new List<CommandMethodDescriptor>(capacity);
        for (var i = 0; i < attributes.Length; i++)
        {
            var attribute = attributes[i];
            var methodDescriptorCollection = methodDescriptorCollectionList[i];
            var methodDescriptors = methodDescriptorCollection.Filter(
                requestType, attribute.MethodNames);
            methodDescriptorList.AddRange(methodDescriptors);
        }

        return [.. methodDescriptorList];
    }

    private static CommandMemberDescriptor[] GetStaticMemberDescriptors(Type requestType)
    {
        var attributes = GetCustomAttributes<CommandStaticPropertyAttribute>(
            requestType, inherit: true);
        if (attributes.Length > 0 && TypeUtility.IsStaticClass(requestType) == true)
        {
            var message = $"""
                Attribute '{nameof(CommandStaticPropertyAttribute)}' is not available because 
                type '{requestType}' is a static type.
                """;
            throw new CommandDefinitionException(message, requestType);
        }

        var memberDescriptorCollectionList = new List<CommandMemberDescriptorCollection>(
            attributes.Length);
        for (var i = 0; i < attributes.Length; i++)
        {
            var attribute = attributes[i];
            var staticType = attribute.GetStaticType(requestType);
            var staticMemberDescriptors = GetMemberDescriptors(staticType);
            memberDescriptorCollectionList.Add(staticMemberDescriptors);
        }

        var capacity = memberDescriptorCollectionList.Sum(item => item.Count);
        var memberDescriptorList = new List<CommandMemberDescriptor>(capacity);
        for (var i = 0; i < attributes.Length; i++)
        {
            var attribute = attributes[i];
            var memberDescriptorCollection = memberDescriptorCollectionList[i];
            var memberDescriptors = memberDescriptorCollection.Filter(
                requestType, attribute.PropertyNames);
            memberDescriptorList.AddRange(memberDescriptors);
        }

        return [.. memberDescriptorList];
    }

    private static CommandMemberDescriptor[] GetMethodMemberDescriptors(MethodInfo methodInfo)
    {
        var requestType = methodInfo.DeclaringType!;
        var attributes = GetCustomAttributes<CommandMethodPropertyAttribute>(
            methodInfo, inherit: true);
        var memberDescriptorCollectionList = new List<CommandMemberDescriptorCollection>(
            attributes.Length);
        for (var i = 0; i < attributes.Length; i++)
        {
            var memberDescriptorCollection = GetMemberDescriptors(requestType);
            memberDescriptorCollectionList.Add(memberDescriptorCollection);
        }

        var capacity = memberDescriptorCollectionList.Sum(item => item.Count);
        var memberDescriptorList = new List<CommandMemberDescriptor>(capacity);
        for (var i = 0; i < attributes.Length; i++)
        {
            var attribute = attributes[i];
            var memberDescriptorCollection = memberDescriptorCollectionList[i];
            var memberDescriptors = memberDescriptorCollection.Filter(
                requestType, attribute.PropertyNames);
            memberDescriptorList.AddRange(memberDescriptors);
        }

        return [.. memberDescriptorList];
    }

    private static CommandMemberDescriptor[] GetMethodStaticMemberDescriptors(MethodInfo methodInfo)
    {
        var requestType = methodInfo.DeclaringType!;
        var attributes = GetCustomAttributes<CommandMethodStaticPropertyAttribute>(
            methodInfo, inherit: true);
        var memberDescriptorCollectionList = new List<CommandMemberDescriptorCollection>(
            attributes.Length);
        for (var i = 0; i < attributes.Length; i++)
        {
            var attribute = attributes[i];
            var staticType = attribute.GetStaticType(requestType);
            var staticMemberDescriptors = GetMemberDescriptors(staticType);
            memberDescriptorCollectionList.Add(staticMemberDescriptors);
        }

        var capacity = memberDescriptorCollectionList.Sum(item => item.Count);
        var memberDescriptorList = new List<CommandMemberDescriptor>(capacity);
        for (var i = 0; i < attributes.Length; i++)
        {
            var attribute = attributes[i];
            var memberDescriptorCollection = memberDescriptorCollectionList[i];
            var memberDescriptors = memberDescriptorCollection.Filter(
                requestType, attribute.PropertyNames);
            memberDescriptorList.AddRange(memberDescriptors);
        }

        return [.. memberDescriptorList];
    }

    private static CommandMemberDescriptorCollection CreateMemberDescriptors(MethodInfo methodInfo)
    {
        if (methodInfo.DeclaringType is null)
        {
            var message = $"""
                Property '{nameof(MethodInfo.DeclaringType)}' of '{nameof(methodInfo)}' 
                cannot be null.
                """;
            throw new ArgumentException(message, nameof(methodInfo));
        }

        VerifyCommandMethod(methodInfo);

        var memberDescriptorList = new List<CommandMemberDescriptor>();
        if (GetBrowsable(methodInfo.DeclaringType) == true && GetBrowsable(methodInfo) == true)
        {
            var parameterInfos = methodInfo.GetParameters();
            var methodMemberDescriptors = GetMethodMemberDescriptors(methodInfo);
            var methodStaticMemberDescriptors = GetMethodStaticMemberDescriptors(methodInfo);
            var capacity = parameterInfos.Length
                + methodMemberDescriptors.Length + methodStaticMemberDescriptors.Length;
            var query = from parameterInfo in parameterInfos
                        where IsCommandParameter(parameterInfo) == true
                        select parameterInfo;
            var items = query.ToArray();
            memberDescriptorList.Capacity = capacity;
            foreach (var item in items)
            {
                memberDescriptorList.Add(CreateParameterDescriptor(item));
            }

            static CommandMemberDescriptor CreateParameterDescriptor(ParameterInfo parameterInfo)
            {
                if (parameterInfo.GetCustomAttribute<ParamArrayAttribute>() is not null)
                {
                    return new CommandParameterArrayDescriptor(parameterInfo);
                }

                return new CommandParameterDescriptor(parameterInfo);
            }

            memberDescriptorList.AddRange(methodMemberDescriptors);
            memberDescriptorList.AddRange(methodStaticMemberDescriptors);
        }

        return new(methodInfo.DeclaringType, memberDescriptorList);
    }

    private static CommandMemberDescriptorCollection CreateMemberDescriptors(Type type)
    {
        var memberDescriptorList = new List<CommandMemberDescriptor>();
        if (GetBrowsable(type) == true)
        {
            var bindingFlags = CommandSettings.GetBindingFlags(type);
            var propertyInfos = type.GetProperties(bindingFlags);
            var query = from propertyInfo in propertyInfos
                        where IsCommandProperty(propertyInfo) == true
                        where GetBrowsable(propertyInfo) == true
                        select propertyInfo;
            var items = query.ToArray();
            var staticMemberDescriptors = GetStaticMemberDescriptors(requestType: type);
            var capacity = items.Length + staticMemberDescriptors.Length;
            memberDescriptorList.Capacity = capacity;
            foreach (var item in items)
            {
                memberDescriptorList.Add(new CommandPropertyDescriptor(item));
            }

            memberDescriptorList.AddRange(staticMemberDescriptors);
            System.Diagnostics.Trace.Assert(memberDescriptorList.Count <= capacity);
        }

        return new(type, memberDescriptorList);
    }

    private static CommandMethodDescriptorCollection CreateMethodDescriptors(Type type)
    {
        var methodDescriptorList = new List<CommandMethodDescriptor>();
        if (GetBrowsable(type) == true)
        {
            var bindingFlags = CommandSettings.GetBindingFlags(type);
            var methodInfos = type.GetMethods(bindingFlags);
            var query = from methodInfo in methodInfos
                        where IsCommandMethod(methodInfo) == true
                        where GetBrowsable(methodInfo) == true
                        select methodInfo;
            var items = query.ToArray();
            var staticMethodDescriptors = GetStaticMethodDescriptors(type);
            methodDescriptorList.Capacity = items.Length + staticMethodDescriptors.Length;
            foreach (var item in items)
            {
                methodDescriptorList.Add(new StandardCommandMethodDescriptor(item));
            }

            methodDescriptorList.AddRange(staticMethodDescriptors);
        }

        return new(type, methodDescriptorList);
    }

    private static CommandUsageDescriptorBase CreateUsageDescriptor(
        CommandUsageAttribute usageAttribute, object target)
    {
        var usageDescriptorType = usageAttribute.UsageDescriptorType;
        var args = new object[] { usageAttribute, target };
        return (CommandUsageDescriptorBase)Activator.CreateInstance(usageDescriptorType, args)!;
    }

    private static CommandUsageDescriptorBase? GetUsageDescriptor(
        MemberInfo memberInfo, object target)
    {
        if (GetCustomAttribute<CommandUsageAttribute>(memberInfo) is { } usageAttribute)
        {
            if (UsageDescriptorByMemberInfo.TryGetValue(target, out var value) != true)
            {
                value = CreateUsageDescriptor(usageAttribute, target);
                UsageDescriptorByMemberInfo.Add(target, value);
            }

            return value;
        }
        else if (memberInfo.DeclaringType is not null
            && GetUsageDescriptor(memberInfo.DeclaringType, target) is { } usageDescriptor)
        {
            return usageDescriptor;
        }

        return null;
    }

    private static CommandUsageDescriptorBase? GetUsageDescriptor(
        ParameterInfo parameterInfo, object target)
    {
        if (GetCustomAttribute<CommandUsageAttribute>(parameterInfo) is { } usageAttribute)
        {
            if (UsageDescriptorByMemberInfo.TryGetValue(target, out var value) != true)
            {
                value = CreateUsageDescriptor(usageAttribute, target);
                UsageDescriptorByMemberInfo.Add(target, value);
            }

            return value;
        }

        return null;
    }
}
