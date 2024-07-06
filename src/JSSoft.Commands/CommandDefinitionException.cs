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
        if (CommandUtility.IsSupportedType(parameterInfo.ParameterType) == false)
        {
            throw new CommandDefinitionException($"ParameterType '{nameof(ParameterInfo.ParameterType)}' is an unsupported type.")
            {
                Source = parameterInfo.Member.DeclaringType!.AssemblyQualifiedName
            };
        }
    }

    public static void ThrowIfPropertyNotReadWrite(PropertyInfo propertyInfo)
    {
        if (propertyInfo.CanRead == false)
        {
            throw new CommandDefinitionException($"Property '{nameof(PropertyInfo.CanRead)}' of '{nameof(PropertyInfo)}' must be '{true}'.")
            {
                Source = propertyInfo.DeclaringType!.AssemblyQualifiedName
            };
        }
        if (propertyInfo.CanWrite == false)
        {
            throw new CommandDefinitionException($"Property '{nameof(PropertyInfo.CanWrite)}' of '{nameof(PropertyInfo)}' must be '{true}'.")
            {
                Source = propertyInfo.DeclaringType!.AssemblyQualifiedName
            };
        }
    }

    public static void ThrowIfPropertyUnsupportedType(PropertyInfo propertyInfo)
    {
        if (CommandUtility.IsSupportedType(propertyInfo.PropertyType) == false)
        {
            throw new CommandDefinitionException($"PropertyType '{propertyInfo.PropertyType}' of '{propertyInfo.DeclaringType}' is an unsupported type.")
            {
                Source = propertyInfo.DeclaringType!.AssemblyQualifiedName
            };
        }
    }

    public static void ThrowIfPropertyNotRightTypeForSwitch(CommandType commandType, PropertyInfo propertyInfo)
    {
        if (commandType == CommandType.Switch && propertyInfo.PropertyType != typeof(bool))
            throw new CommandDefinitionException($"Attribute '{nameof(CommandPropertySwitchAttribute)}' is not available for property '{propertyInfo}'.", propertyInfo.DeclaringType!);
    }

    public static void ThrowIfPropertyNotRightTypeForVariables(CommandType commandType, PropertyInfo propertyInfo)
    {
        if (commandType == CommandType.Variables && propertyInfo.PropertyType.IsArray == false)
            throw new CommandDefinitionException($"Attribute '{nameof(CommandPropertyArrayAttribute)}' is not available for property '{propertyInfo}'.", propertyInfo.DeclaringType!);
    }
}
