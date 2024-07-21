// <copyright file="CommandDefinitionException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public class CommandDefinitionException : SystemException
{
    public CommandDefinitionException(string message)
        : base(message)
    {
    }

    public CommandDefinitionException(string message, Type type)
        : base(message)
    {
        Source = $"{type.AssemblyQualifiedName}";
    }

    public CommandDefinitionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public CommandDefinitionException(string message, Type type, Exception innerException)
        : base(message, innerException)
    {
        Source = $"{type.AssemblyQualifiedName}";
    }

    public static void ThrowIfParameterUnsupportedType(ParameterInfo parameterInfo)
    {
        if (CommandUtility.IsSupportedType(parameterInfo.ParameterType) != true)
        {
            var message = $"""
                ParameterType '{nameof(ParameterInfo.ParameterType)}' is an unsupported type.
                """;
            throw new CommandDefinitionException(message)
            {
                Source = parameterInfo.Member.DeclaringType!.AssemblyQualifiedName,
            };
        }
    }

    public static void ThrowIfPropertyNotReadWrite(PropertyInfo propertyInfo)
    {
        if (propertyInfo.CanRead != true)
        {
            var message = $"""
                Property '{nameof(PropertyInfo.CanRead)}' of '{nameof(PropertyInfo)}' 
                must be '{true}'.
                """;
            throw new CommandDefinitionException(message)
            {
                Source = propertyInfo.DeclaringType!.AssemblyQualifiedName,
            };
        }

        if (propertyInfo.CanWrite != true)
        {
            var message = $"""
                Property '{nameof(PropertyInfo.CanWrite)}' of '{nameof(PropertyInfo)}' 
                must be '{true}'.
                """;
            throw new CommandDefinitionException(message)
            {
                Source = propertyInfo.DeclaringType!.AssemblyQualifiedName,
            };
        }
    }

    public static void ThrowIfPropertyUnsupportedType(PropertyInfo propertyInfo)
    {
        if (CommandUtility.IsSupportedType(propertyInfo.PropertyType) != true)
        {
            var message = $"""
                PropertyType '{propertyInfo.PropertyType}' of '{propertyInfo.DeclaringType}' is 
                an unsupported type.
                """;
            throw new CommandDefinitionException(message)
            {
                Source = propertyInfo.DeclaringType!.AssemblyQualifiedName,
            };
        }
    }

    public static void ThrowIfPropertyNotRightTypeForSwitch(
        bool isSwitch, PropertyInfo propertyInfo)
    {
        if (isSwitch == true && propertyInfo.PropertyType != typeof(bool))
        {
            var message = $"""
                Attribute '{nameof(CommandPropertySwitchAttribute)}' is not available for 
                property '{propertyInfo}'.
                """;
            throw new CommandDefinitionException(message, propertyInfo.DeclaringType!);
        }
    }

    public static void ThrowIfPropertyNotRightTypeForVariables(
        bool isVariables, PropertyInfo propertyInfo)
    {
        if (isVariables == true && propertyInfo.PropertyType.IsArray != true)
        {
            var message = $"""
                Attribute '{nameof(CommandPropertyArrayAttribute)}' is not available for 
                property '{propertyInfo}'.
                """;
            throw new CommandDefinitionException(message, propertyInfo.DeclaringType!);
        }
    }
}
