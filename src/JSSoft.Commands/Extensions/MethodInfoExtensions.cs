// <copyright file="MethodInfoExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using JSSoft.Commands.Exceptions;
using static JSSoft.Commands.AttributeUtility;

namespace JSSoft.Commands.Extensions;

public static class MethodInfoExtensions
{
    public static bool IsCommandMethod(this MethodInfo @this)
        => IsDefined<CommandMethodAttribute>(@this);

    public static bool IsAsync(this MethodInfo @this)
        => @this.ReturnType.IsAssignableFrom(typeof(Task));

    public static string GetName(this MethodInfo @this)
        => GetValue<CommandMethodAttribute>(@this, item => item.Name, GetDefaultName);

    public static string GetPureName(this MethodInfo @this)
    {
        var isAsync = IsAsync(@this);
        var methodName = @this.Name;
        if (isAsync == true && methodName.EndsWith("Async") == true)
        {
            methodName = methodName.Substring(0, methodName.Length - "Async".Length);
        }

        return methodName;
    }

    public static string[] GetAliases(this MethodInfo @this)
        => GetValue<CommandMethodAttribute, string[]>(@this, item => item.Aliases, []);

    public static string GetDisplayName(this MethodInfo @this)
    {
        if (TryGetDisplayName(@this, out var displayName) == true)
        {
            return displayName;
        }

        var name = @this.GetName();
        var aliases = GetAliases(@this);
        return string.Join(", ", value: [name, .. aliases]);
    }

    public static PropertyInfo? GetValidationPropertyInfo(this MethodInfo @this)
    {
        if (@this.DeclaringType is null)
        {
            var message = $"Property '{nameof(MethodInfo.DeclaringType)}' of " +
                          $"'{nameof(@this)}' cannot be null.";
            throw new ArgumentException(message, nameof(@this));
        }

        if (@this.GetCustomAttribute<CommandMethodValidationAttribute>() is { } attribute)
        {
            var propertyName = attribute.PropertyName;
            return attribute.GetPropertyInfo(@this, propertyName);
        }
        else
        {
            var instanceType = @this.DeclaringType;
            var propertyName = $"Can{GetPureName(@this)}";
            var bindingFlags = CommandSettings.GetBindingFlags(instanceType);
            return instanceType.GetProperty(propertyName, bindingFlags);
        }
    }

    public static MethodInfo? GetCompletionMethodInfo(this MethodInfo @this)
    {
        if (@this.DeclaringType is null)
        {
            var message = $"Property '{nameof(MethodInfo.DeclaringType)}' of " +
                          $"'{nameof(@this)}' cannot be null.";
            throw new ArgumentException(message, nameof(@this));
        }

        if (@this.GetCustomAttribute<CommandMethodCompletionAttribute>() is { } attribute)
        {
            var methodName = attribute.MethodName;
            var completionMethodInfo = attribute.GetMethodInfo(@this, methodName);
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
            var instanceType = @this.DeclaringType;
            var bindingFlags = CommandSettings.GetBindingFlags(instanceType);
#pragma warning disable S1199 // Nested code blocks should not be used
            {
                var asyncName = $"Complete{@this.GetPureName()}Async";
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
                var normalName = $"Complete{@this.GetPureName()}";
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

    internal static void VerifyCommandMethod(this MethodInfo @this)
    {
        if (@this.DeclaringType is null)
        {
            throw new CommandDeclaringTypeNullException(@this);
        }

        if (@this.IsCommandMethod() != true)
        {
            throw new CommandMemberMissingAttributeException(
                methodInfo: @this,
                attributeType: typeof(CommandMethodAttribute));
        }

        if (typeof(Task).IsAssignableFrom(@this.ReturnType) != true
            && @this.ReturnType != typeof(void))
        {
            var message = $"Method '{@this}' must have a return type of '{typeof(void)}'.";
            throw new CommandDefinitionException(message, @this);
        }

        if (typeof(Task).IsAssignableFrom(@this.ReturnType) == true
            && typeof(Task) != @this.ReturnType)
        {
            var message = $"Method '{@this}' must have a return type of '{typeof(Task)}'.";
            throw new CommandDefinitionException(message, @this);
        }

        if (@this.ReturnType == typeof(Task))
        {
            var @params = @this.GetParameters();
            var paramsCancellationToken
                = @params.SingleOrDefault(item => item.IsCancellationTokenParameter());
            var paramsProgress = @params.SingleOrDefault(item => item.IsProgressParameter());

            if (paramsProgress is not null)
            {
                var indexProgress = IndexOf(@params, paramsProgress);
                if (indexProgress != @params.Length - 1)
                {
                    var message = $"Parameter '{paramsProgress.Name}' " +
                                  $"of the method '{@this}' must be defined last.";
                    throw new CommandDefinitionException(message, paramsProgress);
                }
            }

            if (paramsProgress is not null && paramsCancellationToken is not null)
            {
                var indexProgress = IndexOf(@params, paramsProgress);
                var indexCancellationToken = IndexOf(@params, paramsCancellationToken);
                if (indexCancellationToken >= 0 && indexProgress != @params.Length - 1)
                {
                    var message = $"Parameter '{paramsCancellationToken.Name}' " +
                                  $"of the method '{@this}' must be defined before " +
                                  $"the parameter '{paramsProgress.Name}'.";
                    throw new CommandDefinitionException(message, @this);
                }
            }

            if (paramsProgress is null && paramsCancellationToken is not null)
            {
                var indexCancellationToken = IndexOf(@params, paramsCancellationToken);
                if (indexCancellationToken != @params.Length - 1)
                {
                    var message = $"Parameter '{paramsCancellationToken.Name}' " +
                                  $"of the method '{@this}' must be defined last.";
                    throw new CommandDefinitionException(message, @this);
                }
            }
        }

        var candidateParamInfos = @this.GetParameters()
            .Where(item => item.IsCancellationTokenParameter() != true)
            .Where(item => item.IsProgressParameter() != true)
            .ToArray();
        var availableParamInfos = candidateParamInfos
            .Where(item => item.IsCommandParameter() == true)
            .ToArray();
        var hasParamArray = Array.Exists(availableParamInfos, IsDefined<ParamArrayAttribute>);
        var commandParamsArray = availableParamInfos.Where(IsDefined<CommandParameterArrayAttribute>)
            .ToArray();

        if (Array.Exists(commandParamsArray, item => item.ParameterType.IsArray != true))
        {
            var message = $"Method '{@this}' must have an array type parameter.";
            throw new CommandDefinitionException(message, @this);
        }

        if (hasParamArray == true && commandParamsArray.Length > 0)
        {
            var message = $"Method '{@this}' cannot have both 'params' and " +
                          $"'{nameof(CommandParameterArrayAttribute)}' attributes.";
            throw new CommandDefinitionException(message, @this);
        }

        if (commandParamsArray.Length > 1)
        {
            var message = $"Method '{@this}' can only have one " +
                          $"'{nameof(CommandParameterArrayAttribute)}' attribute.";
            throw new CommandDefinitionException(message, @this);
        }

        foreach (var candidateParamInfo in candidateParamInfos)
        {
            if (availableParamInfos.Contains(candidateParamInfo) != true)
            {
                throw new CommandParameterNotSupportedTypeException(candidateParamInfo);
            }
        }
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

    private static string GetDefaultName(MemberInfo memberInfo)
    {
        if (memberInfo is MethodInfo methodInfo)
        {
            return CommandUtility.ToSpinalCase(methodInfo.GetPureName());
        }

        throw new UnreachableException();
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
}
