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

public sealed class CommandParameterArrayDescriptor : CommandMemberDescriptor
{
    private object? _value;

    internal CommandParameterArrayDescriptor(ParameterInfo parameterInfo)
        : base(new CommandPropertyArrayAttribute(), parameterInfo.Name!)
    {
        ThrowUtility.ThrowIfParameterInfoNameNull(parameterInfo);

        _value = parameterInfo.DefaultValue;
        InitValue = Array.CreateInstance(parameterInfo.ParameterType.GetElementType()!, 0);
        MemberType = parameterInfo.ParameterType;
        UsageDescriptor = CommandDescriptor.GetUsageDescriptor(parameterInfo);
        IsNullable = CommandUtility.IsNullable(parameterInfo);
    }

    public override object InitValue { get; }

    public override Type MemberType { get; }

    public override bool IsExplicit { get; }

    public override bool IsRequired { get; }

    public override bool IsNullable { get; }

    public override CommandUsageDescriptorBase UsageDescriptor { get; }

    protected override void SetValue(object instance, object? value)
    {
        _value = value;
    }

    protected override object? GetValue(object instance)
    {
        return _value;
    }
}
