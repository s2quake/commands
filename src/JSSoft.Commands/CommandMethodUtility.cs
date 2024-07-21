// <copyright file="CommandMethodUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

// Reflection should not be used to increase accessibility of classes, methods, or fields
#pragma warning disable S3011

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static JSSoft.Commands.AttributeUtility;

namespace JSSoft.Commands;

internal static class CommandMethodUtility
{
    private static readonly Type[] ProgressGenericArgumentTypes =
    [
        typeof(sbyte),
        typeof(byte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
        typeof(float),
        typeof(double),
        typeof(decimal),
    ];

    public static bool IsCommandParameter(ParameterInfo parameterInfo)
    {
        if (IsCancellationTokenParameter(parameterInfo) == true)
        {
            return false;
        }

        if (IsProgressParameter(parameterInfo) == true)
        {
            return false;
        }

        return true;
    }

    public static bool IsCancellationTokenParameter(ParameterInfo parameterInfo)
        => parameterInfo.ParameterType == typeof(CancellationToken);

    public static bool IsProgressParameter(ParameterInfo parameterInfo)
    {
        var parameterType = parameterInfo.ParameterType;
        if (typeof(IProgress<ProgressInfo>).IsAssignableFrom(parameterType) == true)
        {
            return true;
        }
        else if (parameterType.Namespace == nameof(System) && parameterType.Name == "IProgress`1")
        {
            var argumentType = parameterType.GetGenericArguments()[0];
            return ProgressGenericArgumentTypes.Contains(argumentType) == true;
        }

        return false;
    }

    public static bool IsAsync(MethodInfo methodInfo)
        => methodInfo.ReturnType.IsAssignableFrom(typeof(Task));

    public static string GetName(MethodInfo methodInfo)
        => GetValue<CommandMethodAttribute>(methodInfo, item => item.Name, GetDefaultName);

    public static string[] GetAliases(MethodInfo methodInfo)
        => GetValue<CommandMethodAttribute, string[]>(methodInfo, item => item.Aliases, []);

    public static string GetDisplayName(MethodInfo methodInfo)
    {
        if (TryGetDisplayName(methodInfo, out var displayName) == true)
        {
            return displayName;
        }

        var name = GetName(methodInfo);
        var aliases = GetAliases(methodInfo);
        return string.Join(", ", value: [name, .. aliases]);
    }

    public static string GetPureName(MethodInfo methodInfo)
    {
        var isAsync = IsAsync(methodInfo);
        var methodName = methodInfo.Name;
        if (isAsync == true && methodName.EndsWith("Async") == true)
        {
            methodName = methodName.Substring(0, methodName.Length - "Async".Length);
        }

        return methodName;
    }

    public static PropertyInfo? GetValidationPropertyInfo(MethodInfo methodInfo)
    {
        if (methodInfo.DeclaringType is null)
        {
            var message = $"""
                Property '{nameof(MethodInfo.DeclaringType)}' of '{nameof(methodInfo)}' 
                cannot be null.
                """;
            throw new ArgumentException(message, nameof(methodInfo));
        }

        if (methodInfo.GetCustomAttribute<CommandMethodValidationAttribute>() is { } attribute)
        {
            var instanceType = attribute.StaticType ?? methodInfo.DeclaringType;
            var propertyName = attribute.PropertyName;
            var bindingFlags1 = attribute.StaticType is not null
                ? BindingFlags.Static : BindingFlags.Instance;
            var bindingFlags2 = BindingFlags.Public | BindingFlags.NonPublic | bindingFlags1;
            var validationPropertyInfo = instanceType.GetProperty(propertyName, bindingFlags2);
            if (validationPropertyInfo is null)
            {
                var message = $"Type '{instanceType}' does not have property '{propertyName}'.";
                Trace.TraceWarning(message);
            }

            return validationPropertyInfo;
        }
        else
        {
            var instanceType = methodInfo.DeclaringType;
            var propertyName = $"Can{GetPureName(methodInfo)}";
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            return instanceType.GetProperty(propertyName, bindingFlags);
        }
    }

    public static MethodInfo? GetCompletionMethodInfo(MethodInfo methodInfo)
    {
        if (methodInfo.DeclaringType is null)
        {
            var message = $"""
                Property '{nameof(MethodInfo.DeclaringType)}' of '{nameof(methodInfo)}' 
                cannot be null.
                """;
            throw new ArgumentException(message, nameof(methodInfo));
        }

        if (methodInfo.GetCustomAttribute<CommandMethodCompletionAttribute>() is { } attribute)
        {
            var instanceType = attribute.StaticType ?? methodInfo.DeclaringType;
            var methodName = attribute.MethodName;
            var bindingFlags1 = attribute.StaticType is not null
                ? BindingFlags.Static : BindingFlags.Instance;
            var bindingFlags2 = BindingFlags.Public | BindingFlags.NonPublic | bindingFlags1;
            var completionMethodInfo = instanceType.GetMethod(methodName, bindingFlags2);
            if (completionMethodInfo is not null)
            {
                if (completionMethodInfo.ReturnType.IsSubclassOf(typeof(Task)) == true)
                {
                    if (IsCompletionAsyncMethod(completionMethodInfo) == true)
                    {
                        return completionMethodInfo;
                    }

                    var parameterTypes = new Type[]
                    {
                        typeof(CommandMemberDescriptor),
                        typeof(string),
                        typeof(CancellationToken),
                    };
                    var preferredMethodName = GenerateMethodInfoName(
                        methodName, typeof(Task<string[]>), parameterTypes);
                    var message = $"Method must have the following format: {preferredMethodName}.";
                    Trace.TraceWarning(message);
                }
                else
                {
                    if (IsCompletionMethod(completionMethodInfo) == true)
                    {
                        return completionMethodInfo;
                    }

                    var parameterTypes = new Type[]
                    {
                        typeof(CommandMemberDescriptor),
                        typeof(string),
                    };
                    var preferredMethodName = GenerateMethodInfoName(
                        methodName, typeof(string[]), parameterTypes);
                    var message = $"Method must have the following format: {preferredMethodName}.";
                    Trace.TraceWarning(message);
                }
            }
            else
            {
                Trace.TraceWarning($"Type '{instanceType}' does not have method '{methodName}'.");
            }
        }
        else
        {
            var instanceType = methodInfo.DeclaringType;
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
#pragma warning disable S1199 // Nested code blocks should not be used
            {
                var asyncName = $"Complete{GetPureName(methodInfo)}Async";
                var asyncMethod = instanceType.GetMethod(asyncName, bindingFlags);
                if (asyncMethod is not null)
                {
                    if (IsCompletionAsyncMethod(asyncMethod) == true)
                    {
                        return asyncMethod;
                    }

                    var parameterTypes = new Type[]
                    {
                        typeof(CommandMemberDescriptor),
                        typeof(string),
                        typeof(CancellationToken),
                    };
                    var preferredMethodName = GenerateMethodInfoName(
                        asyncName, typeof(Task<string[]>), parameterTypes);
                    var message = $"Method must have the following format: {preferredMethodName}.";
                    Trace.TraceWarning(message);
                }
            }

            {
                var normalName = $"Complete{GetPureName(methodInfo)}";
                var normalMethod = instanceType.GetMethod(normalName, bindingFlags);
                if (normalMethod is not null)
                {
                    if (IsCompletionMethod(normalMethod) == true)
                    {
                        return normalMethod;
                    }

                    var parameterTypes = new Type[]
                    {
                        typeof(CommandMemberDescriptor),
                        typeof(string),
                    };
                    var preferredMethodName = GenerateMethodInfoName(
                        normalName, typeof(string[]), parameterTypes);
                    var message = $"Method must have the following format: {preferredMethodName}.";
                    Trace.TraceWarning(message);
                }
            }
#pragma warning restore S1199 // Nested code blocks should not be used
        }

        return null;
    }

    internal static void VerifyCommandMethod(MethodInfo methodInfo)
    {
        if (CommandAttributeUtility.IsCommandMethod(methodInfo) != true)
        {
            var message = $"""
                MethodInfo '{methodInfo}' does not have attribute 
                '{nameof(CommandMethodAttribute)}'.
                """;
            throw new CommandDefinitionException(message, methodInfo.DeclaringType!);
        }

        if (typeof(Task).IsAssignableFrom(methodInfo.ReturnType) != true
            && methodInfo.ReturnType != typeof(void))
        {
            var message = $"Return type of a Method '{methodInfo}' must be void.";
            throw new CommandDefinitionException(message, methodInfo.DeclaringType!);
        }

        if (typeof(Task).IsAssignableFrom(methodInfo.ReturnType) == true
            && typeof(Task) != methodInfo.ReturnType)
        {
            var message = $"Return type of a Method '{methodInfo}' must be {typeof(Task)}.";
            throw new CommandDefinitionException(message, methodInfo.DeclaringType!);
        }

        VerifyCommandAsyncMethodWithParameter(methodInfo);
    }

    private static void VerifyCommandAsyncMethodWithParameter(MethodInfo methodInfo)
    {
        if (methodInfo.ReturnType == typeof(Task))
        {
            var @params = methodInfo.GetParameters();
            var paramsCancellationToken = @params.SingleOrDefault(IsCancellationTokenParameter);
            var paramsProgress = @params.SingleOrDefault(IsProgressParameter);
            var indexCancellationToken = IndexOf(@params, paramsCancellationToken);
            var indexProgress = IndexOf(@params, paramsProgress);
            if (indexProgress >= 0)
            {
                if (indexProgress != @params.Length - 1)
                {
                    var message = $"""
                        Parameter '{paramsProgress!.Name}' must be defined last.
                        """;
                    throw new CommandDefinitionException(message, methodInfo.DeclaringType!);
                }

                if (indexCancellationToken >= 0 && indexProgress != @params.Length - 1)
                {
                    var message = $"""
                        Parameter '{paramsCancellationToken!.Name}' must be defined before 
                        the parameter '{paramsProgress!.Name}'.
                        """;
                    throw new CommandDefinitionException(message, methodInfo.DeclaringType!);
                }
            }

            if (indexProgress == -1 && indexCancellationToken >= 0
                && indexCancellationToken != @params.Length - 1)
            {
                var message = $"""
                    Parameter '{paramsCancellationToken!.Name}' must be defined last.
                    """;
                throw new CommandDefinitionException(message, methodInfo.DeclaringType!);
            }
        }
    }

    private static bool IsCompletionMethod(MethodInfo methodInfo)
    {
        if (methodInfo.ReturnType != typeof(string[]))
        {
            return false;
        }

        var parameters = methodInfo.GetParameters();
        if (parameters.Length != 2)
        {
            return false;
        }

        if (parameters[0].ParameterType != typeof(CommandMemberDescriptor))
        {
            return false;
        }

        if (parameters[1].ParameterType != typeof(string))
        {
            return false;
        }

        return true;
    }

    private static bool IsCompletionAsyncMethod(MethodInfo methodInfo)
    {
        if (methodInfo.ReturnType != typeof(Task<string[]>))
        {
            return false;
        }

        var parameters = methodInfo.GetParameters();
        if (parameters.Length != 3)
        {
            return false;
        }

        if (parameters[0].ParameterType != typeof(CommandMemberDescriptor))
        {
            return false;
        }

        if (parameters[1].ParameterType != typeof(string))
        {
            return false;
        }

        if (parameters[2].ParameterType != typeof(CancellationToken))
        {
            return false;
        }

        return true;
    }

    private static string GenerateMethodInfoName(
        string methodName, Type returnType, params Type[] parameterTypes)
    {
        var sb = new System.Text.StringBuilder();
        sb.Append(returnType);
        sb.Append(' ');
        sb.Append(methodName);
        sb.Append('(');
        sb.Append(string.Join(", ", parameterTypes.Select(item => $"{item}")));
        sb.Append(')');
        return sb.ToString();
    }

    private static string GetDefaultName(MemberInfo memberInfo)
    {
        if (memberInfo is MethodInfo methodInfo)
        {
            return CommandUtility.ToSpinalCase(GetPureName(methodInfo));
        }

        throw new UnreachableException();
    }

    private static int IndexOf(ParameterInfo[] parameters, ParameterInfo? parameterInfo)
    {
        for (var i = 0; i < parameters.Length; i++)
        {
            if (parameters[i] == parameterInfo)
            {
                return i;
            }
        }

        return -1;
    }
}
