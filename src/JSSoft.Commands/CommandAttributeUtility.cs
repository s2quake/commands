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

using System.ComponentModel;

namespace JSSoft.Commands;

public static class CommandAttributeUtility
{
    public static bool IsCommandProperty(PropertyInfo propertyInfo)
    {
        if (AttributeUtility.GetCustomAttribute<CommandPropertyBaseAttribute>(propertyInfo, inherit: true) is { } == true)
        {
            return true;
        }
        return false;
    }

    public static bool IsCommandMethod(MethodInfo methodInfo)
    {
        if (AttributeUtility.GetCustomAttribute<CommandMethodAttribute>(methodInfo) is { } == true)
        {
            return true;
        }
        return false;
    }

    public static bool GetBrowsable(MemberInfo memberInfo)
    {
        if (AttributeUtility.GetBrowsable(memberInfo) == false)
            return false;
        if (CommandSettings.IsConsoleMode == false && AttributeUtility.GetCustomAttribute<ConsoleModeOnlyAttribute>(memberInfo) is not { })
            return false;
        return true;
    }

    public static string GetSummary(MemberInfo memberInfo)
    {
        if (AttributeUtility.GetCustomAttribute<CommandSummaryAttribute>(memberInfo) is { } commandSummaryAttribute)
        {
            return commandSummaryAttribute.Summary;
        }
        return string.Empty;
    }

    public static string GetSummary(ParameterInfo parameterInfo)
    {
        if (AttributeUtility.GetCustomAttribute<CommandSummaryAttribute>(parameterInfo) is { } commandSummaryAttribute)
        {
            return commandSummaryAttribute.Summary;
        }
        return string.Empty;
    }

    public static string GetDescription(MemberInfo memberInfo)
    {
        if (AttributeUtility.GetCustomAttribute<DescriptionAttribute>(memberInfo) is { } descriptionAttribute)
        {
            return descriptionAttribute.Description;
        }
        return string.Empty;
    }

    public static string GetDescription(ParameterInfo parameterInfo)
    {
        if (AttributeUtility.GetCustomAttribute<DescriptionAttribute>(parameterInfo) is { } descriptionAttribute)
        {
            return descriptionAttribute.Description;
        }
        return string.Empty;
    }

    public static string GetExample(MemberInfo memberInfo)
    {
        if (AttributeUtility.GetCustomAttribute<CommandExampleAttribute>(memberInfo) is { } commandExampleAttribute)
        {
            return commandExampleAttribute.Example;
        }
        return string.Empty;
    }

    public static string GetExample(ParameterInfo parameterInfo)
    {
        if (AttributeUtility.GetCustomAttribute<CommandExampleAttribute>(parameterInfo) is { } commandExampleAttribute)
        {
            return commandExampleAttribute.Example;
        }
        return string.Empty;
    }
}
