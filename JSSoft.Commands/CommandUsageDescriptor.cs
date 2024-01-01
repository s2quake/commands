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

using System.Diagnostics;

namespace JSSoft.Commands;

sealed class CommandUsageDescriptor : CommandUsageDescriptorBase
{
    public CommandUsageDescriptor(object target)
        : base(new CommandUsageAttribute(), target)
    {
        if (target is Type type)
        {
            Summary = CommandAttributeUtility.GetSummary(type);
            Description = CommandAttributeUtility.GetDescription(type);
            Example = CommandAttributeUtility.GetExample(type);
        }
        else if (target is MemberInfo memberInfo)
        {
            Summary = CommandAttributeUtility.GetSummary(memberInfo);
            Description = CommandAttributeUtility.GetDescription(memberInfo);
            Example = CommandAttributeUtility.GetExample(memberInfo);
        }
        else if (target is MethodInfo methodInfo)
        {
            Summary = CommandAttributeUtility.GetSummary(methodInfo);
            Description = CommandAttributeUtility.GetDescription(methodInfo);
            Example = CommandAttributeUtility.GetExample(methodInfo);
        }
        else if (target is ParameterInfo parameterInfo)
        {
            Summary = CommandAttributeUtility.GetSummary(parameterInfo);
            Description = CommandAttributeUtility.GetDescription(parameterInfo);
            Example = CommandAttributeUtility.GetExample(parameterInfo);
        }
        else
        {
            throw new UnreachableException();
        }
    }

    public override string Summary { get; }

    public override string Description { get; }

    public override string Example { get; }
}
