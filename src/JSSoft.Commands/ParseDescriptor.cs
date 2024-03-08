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

public sealed class ParseDescriptor(CommandMemberDescriptor memberDescriptor)
{
    private object? _value = DBNull.Value;

    public CommandMemberDescriptor MemberDescriptor { get; } = memberDescriptor;

    public bool IsRequired => MemberDescriptor.IsRequired;

    public bool IsExplicit => MemberDescriptor.IsExplicit;

    public bool IsValueSet { get; private set; }

    public bool IsOptionSet { get; internal set; }

    public bool HasValue => _value is not DBNull;

    public object? Value
    {
        get
        {
            if (_value is DBNull)
            {
                if (MemberDescriptor.IsExplicit == true && IsOptionSet == true && MemberDescriptor.DefaultValue is not DBNull)
                    return MemberDescriptor.DefaultValue;
                if (MemberDescriptor.IsExplicit == false && MemberDescriptor.DefaultValue is not DBNull)
                    return MemberDescriptor.DefaultValue;
                if (MemberDescriptor.InitValue is not DBNull)
                    return MemberDescriptor.InitValue;
            }
            return _value;
        }
    }

    public string? TextValue { get; private set; }

    public object? InitValue
    {
        get
        {
            if (MemberDescriptor.InitValue is not DBNull)
                return MemberDescriptor.InitValue;
            if (MemberDescriptor.IsNullable == true)
                return null;
            if (MemberDescriptor.MemberType.IsArray == true)
                return Array.CreateInstance(MemberDescriptor.MemberType.GetElementType()!, 0);
            if (MemberDescriptor.MemberType == typeof(string))
                return string.Empty;
            if (MemberDescriptor.MemberType.IsValueType == true)
                return Activator.CreateInstance(MemberDescriptor.MemberType);
            return null;
        }
    }

    public object? ActualValue => IsValueSet == true ? Value : InitValue;

    internal void SetValue(string textValue)
    {
        _value = ParseUtility.Parse(MemberDescriptor, textValue);
        TextValue = textValue;
        IsValueSet = true;
    }

    internal void SetVariablesValue(IReadOnlyList<string> args)
    {
        var textVariables = args.Select(item => CommandUtility.TryWrapDoubleQuotes(item, out var s) == true ? s : item);
        _value = ParseUtility.ParseArray(MemberDescriptor, [.. args]);
        TextValue = string.Join(" ", textVariables);
        IsValueSet = true;
    }

    internal void SetSwitchValue(bool value)
    {
        _value = value;
        IsValueSet = true;
    }
}
