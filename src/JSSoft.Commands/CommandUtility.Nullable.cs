// <copyright file="CommandUtility.Nullable.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections.ObjectModel;

namespace JSSoft.Commands;

// https://stackoverflow.com/questions/58453972/how-to-use-net-reflection-to-check-for-nullable-reference-type
public static partial class CommandUtility
{
    private const string NullableAttribute = "System.Runtime.CompilerServices.NullableAttribute";
    private const string NullableContextAttribute
         = "System.Runtime.CompilerServices.NullableContextAttribute";

    internal static bool IsNullable(PropertyInfo propertyInfo)
        => IsNullableHelper(
            propertyInfo.PropertyType, propertyInfo.DeclaringType, propertyInfo.CustomAttributes);

    internal static bool IsNullable(FieldInfo fieldInfo)
        => IsNullableHelper(
            fieldInfo.FieldType, fieldInfo.DeclaringType, fieldInfo.CustomAttributes);

    internal static bool IsNullable(ParameterInfo parameterInfo)
        => IsNullableHelper(
            parameterInfo.ParameterType, parameterInfo.Member, parameterInfo.CustomAttributes);

    private static bool IsNullableHelper(
        Type memberType,
        MemberInfo? declaringType,
        IEnumerable<CustomAttributeData> customAttributes)
    {
        if (memberType.IsValueType)
        {
            return Nullable.GetUnderlyingType(memberType) is not null;
        }

        var nullable = customAttributes
            .FirstOrDefault(x => x.AttributeType.FullName == NullableAttribute);
        if (nullable is not null && nullable.ConstructorArguments.Count == 1)
        {
            var argument = nullable.ConstructorArguments[0];
            if (argument.ArgumentType == typeof(byte[]))
            {
                var args = (ReadOnlyCollection<CustomAttributeTypedArgument>)argument.Value!;
                if (args.Count > 0 && args[0].ArgumentType == typeof(byte))
                {
                    return (byte)args[0].Value! == 2;
                }
            }
            else if (argument.ArgumentType == typeof(byte))
            {
                return (byte)argument.Value! == 2;
            }
        }

        for (var type = declaringType; type is not null; type = type.DeclaringType)
        {
            var context = type.CustomAttributes
                .FirstOrDefault(x => x.AttributeType.FullName == NullableContextAttribute);
            if (context is not null &&
                context.ConstructorArguments.Count == 1 &&
                context.ConstructorArguments[0].ArgumentType == typeof(byte))
            {
                return (byte)context.ConstructorArguments[0].Value! == 2;
            }
        }

        // Couldn't find a suitable attribute
        return false;
    }
}
