// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands;

static class CommandMethodUtility
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
    {
        return parameterInfo.ParameterType == typeof(CancellationToken);
    }

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
    {
        return methodInfo.ReturnType.IsAssignableFrom(typeof(Task));
    }

    public static string GetName(MethodInfo methodInfo)
    {
        if (AttributeUtility.GetCustomAttribute<CommandMethodAttribute>(methodInfo) is { } commandMethodAttribute &&
            commandMethodAttribute.Name != string.Empty)
        {
            return commandMethodAttribute.Name;
        }
        var methodName = GetPureName(methodInfo);
        return CommandUtility.ToSpinalCase(methodName);
    }

    public static string[] GetAliases(MethodInfo methodInfo)
    {
        if (AttributeUtility.GetCustomAttribute<CommandMethodAttribute>(methodInfo) is { } commandMethodAttribute)
        {
            return commandMethodAttribute.Aliases;
        }
        return [];
    }

    public static string GetDisplayName(MethodInfo methodInfo)
    {
        if (AttributeUtility.TryGetDisplayName(methodInfo, out var displayName) == true)
        {
            return displayName;
        }
        var name = GetName(methodInfo);
        var aliases = GetAliases(methodInfo);
        return string.Join(", ", [name, .. aliases]);
    }

    public static string GetPureName(MethodInfo methodInfo)
    {
        var isAsync = IsAsync(methodInfo);
        var methodName = methodInfo.Name;
        if (isAsync == true && methodName.EndsWith("Async") == true)
            methodName = methodName.Substring(0, methodName.Length - "Async".Length);
        return methodName;
    }

    public static PropertyInfo? GetValidationPropertyInfo(MethodInfo methodInfo)
    {
        if (methodInfo.DeclaringType == null)
            throw new ArgumentException($"Property '{nameof(MethodInfo.DeclaringType)}' of '{nameof(methodInfo)}' cannot be null.", nameof(methodInfo));

        if (methodInfo.GetCustomAttribute<CommandMethodValidationAttribute>() is CommandMethodValidationAttribute attribute)
        {
            var instanceType = attribute.StaticType ?? methodInfo.DeclaringType;
            var propertyName = attribute.PropertyName;
            var bindingFlags1 = attribute.StaticType != null ? BindingFlags.Static : BindingFlags.Instance;
            var bindingFlags2 = BindingFlags.Public | BindingFlags.NonPublic | bindingFlags1;
            var validationPropertyInfo = instanceType.GetProperty(propertyName, bindingFlags2);
            if (validationPropertyInfo is null)
            {
                Trace.TraceWarning($"Type '{instanceType}' does not have property '{propertyName}'.");
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
        if (methodInfo.DeclaringType == null)
            throw new ArgumentException($"Property '{nameof(MethodInfo.DeclaringType)}' of '{nameof(methodInfo)}' cannot be null.", nameof(methodInfo));

        if (methodInfo.GetCustomAttribute<CommandMethodCompletionAttribute>() is CommandMethodCompletionAttribute attribute)
        {
            var instanceType = attribute.StaticType ?? methodInfo.DeclaringType;
            var methodName = attribute.MethodName;
            var bindingFlags1 = attribute.StaticType != null ? BindingFlags.Static : BindingFlags.Instance;
            var bindingFlags2 = BindingFlags.Public | BindingFlags.NonPublic | bindingFlags1;
            var completionMethodInfo = instanceType.GetMethod(methodName, bindingFlags2);
            if (completionMethodInfo != null)
            {
                if (completionMethodInfo.ReturnType.IsSubclassOf(typeof(Task)) == true)
                {
                    if (IsCompletionAsyncMethod(completionMethodInfo) == true)
                        return completionMethodInfo;
                    var preferredMethodName = GenerateMethodInfoName(methodName, typeof(Task<string[]>), parameterTypes: [typeof(CommandMemberDescriptor), typeof(string), typeof(CancellationToken)]);
                    Trace.TraceWarning($"Method must have the following format: {preferredMethodName}");
                }
                else
                {
                    if (IsCompletionMethod(completionMethodInfo) == true)
                        return completionMethodInfo;
                    var preferredMethodName = GenerateMethodInfoName(methodName, typeof(string[]), parameterTypes: [typeof(CommandMemberDescriptor), typeof(string)]);
                    Trace.TraceWarning($"Method must have the following format: {preferredMethodName}");
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
            var isAsync = IsAsync(methodInfo);
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            {
                var asyncName = $"Complete{GetPureName(methodInfo)}Async";
                var asyncMethod = instanceType.GetMethod(asyncName, bindingFlags);
                if (asyncMethod != null)
                {
                    if (IsCompletionAsyncMethod(asyncMethod) == true)
                        return asyncMethod;
                    var preferredMethodName = GenerateMethodInfoName(asyncName, typeof(Task<string[]>), parameterTypes: [typeof(CommandMemberDescriptor), typeof(string), typeof(CancellationToken)]);
                    Trace.TraceWarning($"Method must have the following format: {preferredMethodName}");
                }
            }
            {
                var normalName = $"Complete{GetPureName(methodInfo)}";
                var normalMethod = instanceType.GetMethod(normalName, bindingFlags);
                if (normalMethod != null)
                {
                    if (IsCompletionMethod(normalMethod) == true)
                        return normalMethod;
                    var preferredMethodName = GenerateMethodInfoName(normalName, typeof(string[]), parameterTypes: [typeof(CommandMemberDescriptor), typeof(string)]);
                    Trace.TraceWarning($"Method must have the following format: {preferredMethodName}");
                }
            }
        }
        return null;
    }

    internal static void VerifyCommandMethod(MethodInfo methodInfo)
    {
        if (CommandAttributeUtility.IsCommandMethod(methodInfo) == false)
            throw new CommandDefinitionException($"MethodInfo '{methodInfo}' does not have attribute '{nameof(CommandMethodAttribute)}'.", methodInfo.DeclaringType!);
        if (typeof(Task).IsAssignableFrom(methodInfo.ReturnType) == false && methodInfo.ReturnType != typeof(void))
            throw new CommandDefinitionException($"Return type of a Method '{methodInfo}' must be void.", methodInfo.DeclaringType!);
        if (typeof(Task).IsAssignableFrom(methodInfo.ReturnType) == true && typeof(Task) != methodInfo.ReturnType)
            throw new CommandDefinitionException($"Return type of a Method '{methodInfo}' must be {typeof(Task)}.", methodInfo.DeclaringType!);
        VerifyCommandAsyncMethodWithParameter(methodInfo);
    }

    private static void VerifyCommandAsyncMethodWithParameter(MethodInfo methodInfo)
    {
        if (methodInfo.ReturnType == typeof(Task))
        {
            var parameters = methodInfo.GetParameters();
            var parameterCancellationToken = parameters.SingleOrDefault(IsCancellationTokenParameter);
            var parameterProgress = parameters.SingleOrDefault(IsProgressParameter);
            var indexCancellationToken = IndexOf(parameters, parameterCancellationToken);
            var indexProgress = IndexOf(parameters, parameterProgress);
            if (indexProgress >= 0)
            {
                if (indexProgress != parameters.Length - 1)
                    throw new CommandDefinitionException($"Parameter '{parameterProgress!.Name}' must be defined last.", methodInfo.DeclaringType!);
                if (indexCancellationToken >= 0 && indexProgress != parameters.Length - 1)
                    throw new CommandDefinitionException($"Parameter '{parameterCancellationToken!.Name}' must be defined before the parameter '{parameterProgress!.Name}'.", methodInfo.DeclaringType!);
            }
            if (indexProgress == -1 && indexCancellationToken >= 0)
            {
                if (indexCancellationToken != parameters.Length - 1)
                    throw new CommandDefinitionException($"Parameter '{parameterCancellationToken!.Name}' must be defined last.", methodInfo.DeclaringType!);
            }
        }

        static int IndexOf(ParameterInfo[] parameters, ParameterInfo? parameterInfo)
        {
            for (var i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] == parameterInfo)
                    return i;
            }
            return -1;
        }
    }

    private static bool IsCompletionMethod(MethodInfo methodInfo)
    {
        if (methodInfo.ReturnType == typeof(string[]))
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 2 && parameters[0].ParameterType == typeof(CommandMemberDescriptor) && parameters[1].ParameterType == typeof(string))
                return true;
        }
        return false;
    }

    private static bool IsCompletionAsyncMethod(MethodInfo methodInfo)
    {
        if (methodInfo.ReturnType == typeof(Task<string[]>))
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 3 && parameters[0].ParameterType == typeof(CommandMemberDescriptor) && parameters[1].ParameterType == typeof(string) && parameters[2].ParameterType == typeof(CancellationToken))
                return true;
        }
        return false;
    }

    private static string GenerateMethodInfoName(string methodName, Type returnType, params Type[] parameterTypes)
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
