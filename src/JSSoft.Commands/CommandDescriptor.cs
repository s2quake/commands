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

public static class CommandDescriptor
{
    private static readonly Dictionary<MemberInfo, CommandMemberDescriptorCollection> memberDescriptorsByMemberInfo = [];
    private static readonly Dictionary<MemberInfo, CommandMethodDescriptorCollection> methodDescriptorsByMemberInfo = [];
    private static readonly Dictionary<object, CommandUsageDescriptorBase> usageDescriptorByMemberInfo = [];

    public static CommandUsageDescriptorBase GetUsageDescriptor(MemberInfo memberInfo)
    {
        if (GetUsageDescriptor(memberInfo, memberInfo) is { } usageDescriptor1)
        {
            return usageDescriptor1;
        }
        else if (memberInfo.DeclaringType != null && GetUsageDescriptor(memberInfo.DeclaringType, memberInfo) is { } usageDescriptor2)
        {
            return usageDescriptor2;
        }
        if (usageDescriptorByMemberInfo.ContainsKey(memberInfo) == false)
        {
            usageDescriptorByMemberInfo.Add(memberInfo, new CommandUsageDescriptor(memberInfo));
        }
        return usageDescriptorByMemberInfo[memberInfo];
    }

    public static CommandUsageDescriptorBase GetUsageDescriptor(ParameterInfo parameterInfo)
    {
        if (GetUsageDescriptor(parameterInfo, parameterInfo) is { } usageDescriptor1)
        {
            return usageDescriptor1;
        }
        else if (parameterInfo.Member != null && GetUsageDescriptor(parameterInfo.Member, parameterInfo) is { } usageDescriptor2)
        {
            return usageDescriptor2;
        }
        if (usageDescriptorByMemberInfo.ContainsKey(parameterInfo) == false)
        {
            usageDescriptorByMemberInfo.Add(parameterInfo, new CommandUsageDescriptor(parameterInfo));
        }
        return usageDescriptorByMemberInfo[parameterInfo];
    }

    public static CommandMethodDescriptorCollection GetMethodDescriptors(Type type)
    {
        if (methodDescriptorsByMemberInfo.ContainsKey(type) == false)
        {
            methodDescriptorsByMemberInfo.Add(type, CreateMethodDescriptors(type));
        }
        return methodDescriptorsByMemberInfo[type];
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
        if (memberDescriptorsByMemberInfo.ContainsKey(type) == false)
        {
            var memberDescriptors = CreateMemberDescriptors(type);
            memberDescriptorsByMemberInfo.Add(type, memberDescriptors);
        }
        return memberDescriptorsByMemberInfo[type];
    }

    public static CommandMemberDescriptorCollection GetMemberDescriptors(Type type, string methodName)
    {
        var bindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var methodInfo = type.GetMethod(methodName, bindingFlags) ?? throw new ArgumentException($"Type '{type}' does not have method '{methodName}'.", nameof(methodName));
        return GetMemberDescriptorsByMethodInfo(methodInfo);
    }

    public static CommandMemberDescriptorCollection GetMemberDescriptors(object obj, string methodName) => GetMemberDescriptors(obj.GetType(), methodName);

    internal static CommandMemberDescriptorCollection GetMemberDescriptorsByMethodInfo(MethodInfo methodInfo)
    {
        if (memberDescriptorsByMemberInfo.ContainsKey(methodInfo) == false)
        {
            var memberDescriptors = CreateMemberDescriptors(methodInfo);
            memberDescriptorsByMemberInfo.Add(methodInfo, memberDescriptors);
        }
        return memberDescriptorsByMemberInfo[methodInfo];
    }

    private static CommandMethodDescriptor[] GetStaticMethodDescriptors(Type requestType)
    {
        var attributes = AttributeUtility.GetCustomAttributes<CommandStaticMethodAttribute>(requestType, inherit: true);
        if (attributes.Length > 0 && TypeUtility.IsStaticClass(requestType) == true)
            throw new CommandDefinitionException($"Attribute '{nameof(CommandStaticMethodAttribute)}' is not available because type '{requestType}' is a static type.", requestType);

        var methodDescriptorCollectionList = new List<CommandMethodDescriptorCollection>(attributes.Length);
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
            var methodDescriptors = methodDescriptorCollection.Filter(requestType, attribute.MethodNames);
            methodDescriptorList.AddRange(methodDescriptors);
        }
        return [.. methodDescriptorList];
    }

    private static CommandMemberDescriptor[] GetStaticMemberDescriptors(Type requestType)
    {
        var attributes = AttributeUtility.GetCustomAttributes<CommandStaticPropertyAttribute>(requestType, inherit: true);
        if (attributes.Length > 0 && TypeUtility.IsStaticClass(requestType) == true)
            throw new CommandDefinitionException($"Attribute '{nameof(CommandStaticPropertyAttribute)}' is not available because type '{requestType}' is a static type.", requestType);

        var memberDescriptorCollectionList = new List<CommandMemberDescriptorCollection>(attributes.Length);
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
            var memberDescriptors = memberDescriptorCollection.Filter(requestType, attribute.PropertyNames);
            memberDescriptorList.AddRange(memberDescriptors);
        }
        return [.. memberDescriptorList];
    }

    private static CommandMemberDescriptor[] GetMethodMemberDescriptors(MethodInfo methodInfo)
    {
        var requestType = methodInfo.DeclaringType!;
        var attributes = AttributeUtility.GetCustomAttributes<CommandMethodPropertyAttribute>(methodInfo, inherit: true);
        var memberDescriptorCollectionList = new List<CommandMemberDescriptorCollection>(attributes.Length);
        for (var i = 0; i < attributes.Length; i++)
        {
            var attribute = attributes[i];
            var memberDescriptorCollection = GetMemberDescriptors(requestType);
            memberDescriptorCollectionList.Add(memberDescriptorCollection);
        }
        var capacity = memberDescriptorCollectionList.Sum(item => item.Count);
        var memberDescriptorList = new List<CommandMemberDescriptor>(capacity);
        for (var i = 0; i < attributes.Length; i++)
        {
            var attribute = attributes[i];
            var memberDescriptorCollection = memberDescriptorCollectionList[i];
            var memberDescriptors = memberDescriptorCollection.Filter(requestType, attribute.PropertyNames);
            memberDescriptorList.AddRange(memberDescriptors);
        }
        return [.. memberDescriptorList];
    }

    private static CommandMemberDescriptor[] GetMethodStaticMemberDescriptors(MethodInfo methodInfo)
    {
        var requestType = methodInfo.DeclaringType!;
        var attributes = AttributeUtility.GetCustomAttributes<CommandMethodStaticPropertyAttribute>(methodInfo, inherit: true);
        var memberDescriptorCollectionList = new List<CommandMemberDescriptorCollection>(attributes.Length);
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
            var memberDescriptors = memberDescriptorCollection.Filter(requestType, attribute.PropertyNames);
            memberDescriptorList.AddRange(memberDescriptors);
        }
        return [.. memberDescriptorList];
    }

    private static CommandMemberDescriptorCollection CreateMemberDescriptors(MethodInfo methodInfo)
    {
        if (methodInfo.DeclaringType == null)
            throw new ArgumentException($"Property '{nameof(MethodInfo.DeclaringType)}' of '{nameof(methodInfo)}' cannot be null.", nameof(methodInfo));
        CommandMethodUtility.VerifyCommandMethod(methodInfo);

        var memberDescriptorList = new List<CommandMemberDescriptor>();
        if (CommandAttributeUtility.GetBrowsable(methodInfo.DeclaringType) == true && CommandAttributeUtility.GetBrowsable(methodInfo) == true)
        {
            var parameterInfos = methodInfo.GetParameters();
            var methodMemberDescriptors = GetMethodMemberDescriptors(methodInfo);
            var methodStaticMemberDescriptors = GetMethodStaticMemberDescriptors(methodInfo);
            var capacity = parameterInfos.Length + methodMemberDescriptors.Length + methodStaticMemberDescriptors.Length;
            var query = from item in parameterInfos
                        where CommandMethodUtility.IsCommandParameter(item) == true
                        select item;
            var items = query.ToArray();
            memberDescriptorList.Capacity = capacity;
            foreach (var item in items)
            {
                memberDescriptorList.Add(CreateParameterDescriptor(item));
            }

            static CommandMemberDescriptor CreateParameterDescriptor(ParameterInfo parameterInfo)
            {
                if (parameterInfo.GetCustomAttribute<ParamArrayAttribute>() != null)
                    return new CommandParameterArrayDescriptor(parameterInfo);
                return new CommandParameterDescriptor(parameterInfo);
            }
            memberDescriptorList.AddRange(methodMemberDescriptors);
            memberDescriptorList.AddRange(methodStaticMemberDescriptors);
        }
        return new(methodInfo.DeclaringType, memberDescriptorList);
    }

    private static CommandMethodDescriptorCollection CreateMethodDescriptors(Type type)
    {
        var methodDescriptorList = new List<CommandMethodDescriptor>();
        if (CommandAttributeUtility.GetBrowsable(type) == true)
        {
            var bindingFlags = CommandSettings.GetBindingFlags(type);
            var methods = type.GetMethods(bindingFlags);
            var query = from item in methods
                        where CommandAttributeUtility.IsCommandMethod(item) == true
                        where CommandAttributeUtility.GetBrowsable(item) == true
                        select item;
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

    private static CommandMemberDescriptorCollection CreateMemberDescriptors(Type type)
    {
        var memberDescriptorList = new List<CommandMemberDescriptor>();
        if (CommandAttributeUtility.GetBrowsable(type) == true)
        {
            var bindingFlags = CommandSettings.GetBindingFlags(type);
            var properties = type.GetProperties(bindingFlags);
            var query = from item in properties
                        where CommandAttributeUtility.IsCommandProperty(item) == true
                        where CommandAttributeUtility.GetBrowsable(item) == true
                        select item;
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

    private static CommandUsageDescriptorBase CreateUsageDescriptor(CommandUsageAttribute usageAttribute, MemberInfo memberInfo, object target)
    {
        var usageDescriptorType = usageAttribute.UsageDescriptorType;
        var args = new object[] { usageAttribute, target };
        return (CommandUsageDescriptorBase)Activator.CreateInstance(usageDescriptorType, args)!;
    }

    private static CommandUsageDescriptorBase CreateUsageDescriptor(CommandUsageAttribute usageAttribute, ParameterInfo parameterInfo, object target)
    {
        var usageDescriptorType = usageAttribute.UsageDescriptorType;
        var args = new object[] { usageAttribute, target };
        return (CommandUsageDescriptorBase)Activator.CreateInstance(usageDescriptorType, args)!;
    }

    private static CommandUsageDescriptorBase? GetUsageDescriptor(MemberInfo memberInfo, object target)
    {
        if (AttributeUtility.GetCustomAttribute<CommandUsageAttribute>(memberInfo) is { } usageAttribute)
        {
            if (usageDescriptorByMemberInfo.ContainsKey(target) == false)
            {
                usageDescriptorByMemberInfo.Add(target, CreateUsageDescriptor(usageAttribute, memberInfo, target));
            }
            return usageDescriptorByMemberInfo[target];
        }
        else if (memberInfo.DeclaringType != null && GetUsageDescriptor(memberInfo.DeclaringType, target) is { } usageDescriptor)
        {
            return usageDescriptor;
        }
        return null;
    }

    private static CommandUsageDescriptorBase? GetUsageDescriptor(ParameterInfo parameterInfo, object target)
    {
        if (AttributeUtility.GetCustomAttribute<CommandUsageAttribute>(parameterInfo) is { } usageAttribute)
        {
            if (usageDescriptorByMemberInfo.ContainsKey(target) == false)
            {
                usageDescriptorByMemberInfo.Add(target, CreateUsageDescriptor(usageAttribute, parameterInfo, target));
            }
            return usageDescriptorByMemberInfo[target];
        }
        return null;
    }
}
