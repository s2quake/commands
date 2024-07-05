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
