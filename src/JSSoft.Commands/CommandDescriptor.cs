// <copyright file="CommandDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

// Reflection should not be used to increase accessibility of classes, methods, or fields
#pragma warning disable S3011

using System.Diagnostics;
using JSSoft.Commands.Extensions;
using static JSSoft.Commands.AttributeUtility;

namespace JSSoft.Commands;

public static class CommandDescriptor
{
    private static readonly Dictionary<MemberInfo, CommandMemberDescriptorCollection>
        MemberDescriptorsByMemberInfo = [];

    private static readonly Dictionary<MemberInfo, CommandMethodDescriptorCollection>
        MethodDescriptorsByMemberInfo = [];

    private static readonly Dictionary<CommandMemberInfo, CommandUsage>
        UsageByMemberInfo = [];

    public static CommandUsage GetUsage(CommandMemberInfo memberInfo)
    {
        if (GetAttribute<CommandUsageAttribute>(memberInfo.DeclaringType) is { } attribute)
        {
            return attribute.GetUsage(memberInfo);
        }

        if (UsageByMemberInfo.TryGetValue(memberInfo, out var value) is false)
        {
            value = memberInfo.GetDefaultUsage();
            UsageByMemberInfo.Add(memberInfo, value);
        }

        return value;
    }

    public static CommandMethodDescriptorCollection GetMethodDescriptors(Type type)
    {
        if (MethodDescriptorsByMemberInfo.TryGetValue(type, out var value) is false)
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
            return customCommandDescriptor.Members;
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
        if (MemberDescriptorsByMemberInfo.TryGetValue(type, out var value) is false)
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
        if (MemberDescriptorsByMemberInfo.TryGetValue(methodInfo, out var value) is false)
        {
            value = CreateMemberDescriptors(methodInfo);
            MemberDescriptorsByMemberInfo.Add(methodInfo, value);
        }

        return value;
    }

    private static CommandMethodDescriptor[] GetStaticMethodDescriptors(Type requestType)
    {
        var attributes = GetAttributes<CommandStaticMethodAttribute>(
            requestType, inherit: true);
        if (attributes.Length > 0 && requestType.IsStaticClass() is true)
        {
            var message = $"Attribute '{nameof(CommandStaticMethodAttribute)}' is not available " +
                          $"because type '{requestType}' is a static type.";
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
        var attributes = GetAttributes<CommandStaticPropertyAttribute>(
            requestType, inherit: true);
        if (attributes.Length > 0 && requestType.IsStaticClass() is true)
        {
            var message = $"Attribute '{nameof(CommandStaticPropertyAttribute)}' is not " +
                          $"available because type '{requestType}' is a static type.";
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
        var attributes = GetAttributes<CommandMethodPropertyAttribute>(
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
                methodInfo, attribute.PropertyNames);
            memberDescriptorList.AddRange(memberDescriptors);
        }

        return [.. memberDescriptorList];
    }

    private static CommandMemberDescriptor[] GetMethodStaticMemberDescriptors(MethodInfo methodInfo)
    {
        var attributes = GetAttributes<CommandMethodStaticPropertyAttribute>(
            methodInfo, inherit: true);
        var memberDescriptorCollectionList = new List<CommandMemberDescriptorCollection>(
            attributes.Length);
        for (var i = 0; i < attributes.Length; i++)
        {
            var attribute = attributes[i];
            var staticType = attribute.GetStaticType(methodInfo);
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
                methodInfo, attribute.PropertyNames);
            memberDescriptorList.AddRange(memberDescriptors);
        }

        return [.. memberDescriptorList];
    }

    private static CommandMemberDescriptorCollection CreateMemberDescriptors(MethodInfo methodInfo)
    {
        methodInfo.VerifyCommandMethod();

        var memberInfo = new CommandMemberInfo(methodInfo);
        var memberDescriptorList = new List<CommandMemberDescriptor>();
        if (methodInfo.IsBrowsable() is true)
        {
            var parameterInfos = methodInfo.GetParameters();
            var methodMemberDescriptors = GetMethodMemberDescriptors(methodInfo);
            var methodStaticMemberDescriptors = GetMethodStaticMemberDescriptors(methodInfo);
            var capacity = parameterInfos.Length
                + methodMemberDescriptors.Length + methodStaticMemberDescriptors.Length;
            var query = from parameterInfo in parameterInfos
                        where parameterInfo.IsCommandParameter() is true
                        select parameterInfo;
            var items = query.ToArray();
            memberDescriptorList.Capacity = capacity;
            foreach (var item in items)
            {
                if (CommandUtility.IsSupportedType(item.ParameterType) is true)
                {
                    memberDescriptorList.Add(CreateParameterDescriptor(item));
                }
                else if (IsDefined<CommandParameterAttribute>(item) is true)
                {
                    memberDescriptorList.AddRange(GetMemberDescriptors(item.ParameterType));
                }
                else
                {
                    throw new UnreachableException("This should not be reached.");
                }
            }

            memberDescriptorList.AddRange(methodMemberDescriptors);
            memberDescriptorList.AddRange(methodStaticMemberDescriptors);
        }

        return new(memberInfo, memberDescriptorList);
    }

    private static CommandMemberDescriptorCollection CreateMemberDescriptors(Type type)
    {
        var memberDescriptorList = new List<CommandMemberDescriptor>();
        if (type.IsBrowsable() is true)
        {
            var bindingFlags = CommandSettings.GetBindingFlags(type);
            var propertyInfos = type.GetProperties(bindingFlags);
            var query = from propertyInfo in propertyInfos
                        where propertyInfo.IsCommandProperty() is true
                        where propertyInfo.IsBrowsable() is true
                        select propertyInfo;
            var items = query.ToArray();
            var staticMemberDescriptors = GetStaticMemberDescriptors(requestType: type);
            var capacity = items.Length + staticMemberDescriptors.Length;
            memberDescriptorList.Capacity = capacity;
            foreach (var item in items)
            {
                var propertyAttribute = GetAttribute<CommandPropertyBaseAttribute>(item)!;
                memberDescriptorList.Add(new CommandPropertyDescriptor(item, propertyAttribute));
            }

            memberDescriptorList.AddRange(staticMemberDescriptors);
            Trace.Assert(memberDescriptorList.Count <= capacity);
        }

        return new(type, memberDescriptorList);
    }

    private static CommandMethodDescriptorCollection CreateMethodDescriptors(Type type)
    {
        var methodDescriptorList = new List<CommandMethodDescriptor>();
        if (type.IsBrowsable() is true)
        {
            var bindingFlags = CommandSettings.GetBindingFlags(type);
            var methodInfos = type.GetMethods(bindingFlags);
            var query = from methodInfo in methodInfos
                        where methodInfo.IsCommandMethod() is true
                        where methodInfo.IsBrowsable() is true
                        select methodInfo;
            var items = query.ToArray();
            var staticMethodDescriptors = GetStaticMethodDescriptors(type);
            methodDescriptorList.Capacity = items.Length + staticMethodDescriptors.Length;
            foreach (var item in items)
            {
                methodDescriptorList.Add(new CommandMethodDescriptor(item));
            }

            methodDescriptorList.AddRange(staticMethodDescriptors);
        }

        return new(type, methodDescriptorList);
    }

    private static CommandMemberDescriptor CreateParameterDescriptor(ParameterInfo parameterInfo)
    {
        if (IsDefined<ParamArrayAttribute>(parameterInfo) is true)
        {
            var attribute1 = new CommandParameterArrayAttribute();
            return new CommandParameterDescriptor(parameterInfo, attribute1);
        }

        if (GetAttribute<CommandParameterBaseAttribute>(parameterInfo) is { } attribute2)
        {
            return new CommandParameterDescriptor(parameterInfo, attribute2);
        }

        var attribute3 = new CommandParameterAttribute();
        return new CommandParameterDescriptor(parameterInfo, attribute3);
    }
}
