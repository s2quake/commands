// <copyright file="ParameterInfoExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;
using static JSSoft.Commands.AttributeUtility;

namespace JSSoft.Commands.Extensions;

public static class ParameterInfoExtensions
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

    public static bool IsCommandParameter(this ParameterInfo @this)
    {
        if (IsCancellationTokenParameter(@this) is true)
        {
            return false;
        }

        if (IsProgressParameter(@this) is true)
        {
            return false;
        }

        if (CommandUtility.IsSupportedType(@this.ParameterType) is false)
        {
            return IsDefined<CommandParameterAttribute>(@this);
        }

        return true;
    }

    public static bool IsCancellationTokenParameter(this ParameterInfo @this)
        => @this.ParameterType == typeof(CancellationToken);

    public static bool IsProgressParameter(this ParameterInfo @this)
    {
        var parameterType = @this.ParameterType;
        if (typeof(IProgress<ProgressInfo>).IsAssignableFrom(parameterType) is true)
        {
            return true;
        }
        else if (parameterType.Namespace == nameof(System) && parameterType.Name == "IProgress`1")
        {
            var argumentType = parameterType.GetGenericArguments()[0];
            return ProgressGenericArgumentTypes.Contains(argumentType) is true;
        }

        return false;
    }
}
