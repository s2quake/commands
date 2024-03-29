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
using System.Threading.Tasks;

namespace JSSoft.Commands;

public abstract class CommandMemberDescriptor
{
    protected CommandMemberDescriptor(CommandPropertyBaseAttribute attribute, string memberName)
    {
        Attribute = attribute;
        MemberName = memberName;
        Name = attribute.GetName(defaultName: memberName);
        ShortName = attribute.ShortName;
        IsRequired = attribute.IsRequired;
        IsExplicit = attribute.IsExplicit;
        IsSwitch = attribute.IsSwitch;
        IsVariables = attribute.IsVariables;
        DefaultValue = attribute.DefaultValue;
        InitValue = attribute.InitValue;
        CommandType = attribute.CommandType;
        DisplayName = GenerateDisplayName(this);
    }

    public override string ToString() => $"{MemberName} [{DisplayName}]";

    public string Name { get; }

    public char ShortName { get; }

    public virtual string DisplayName { get; }

    public virtual object? InitValue { get; }

    public virtual object? DefaultValue { get; }

    public virtual bool IsRequired { get; }

    public virtual bool IsExplicit { get; }

    public virtual bool IsSwitch { get; }

    public virtual bool IsVariables { get; }

    public abstract bool IsNullable { get; }

    public abstract Type MemberType { get; }

    public string MemberName { get; }

    public CommandType CommandType { get; }

    public abstract CommandUsageDescriptorBase UsageDescriptor { get; }

    protected abstract void SetValue(object instance, object? value);

    protected abstract object? GetValue(object instance);

    protected virtual void OnValidateTrigger(ParseDescriptorCollection parseDescriptors)
    {
    }

    protected virtual string[]? GetCompletion(object instance, string find)
    {
        return null;
    }

    protected string[] GetCompletion(object instance, string find, CommandMemberCompletionAttribute attribute)
    {
        var methodName = attribute.MethodName;
        var type = attribute.StaticType ?? instance.GetType();
        var obj = attribute.StaticType != null ? null : instance;
        var flag = attribute.StaticType != null ? BindingFlags.Static : BindingFlags.Instance;
        var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | flag, null, [], null);
        if (method == null)
            throw new ArgumentException($"Cannot found method '{methodName}'", nameof(attribute));
        try
        {
            var arrayType = MemberType.MakeArrayType();
            var value = method.Invoke(obj, null);
            if (value?.GetType() == arrayType && value is Array @array)
            {
                var itemList = new List<string>(@array.Length);
                foreach (var item in @array)
                {
                    itemList.Add($"{item}");
                }
                return [.. itemList];
            }
            else if (value is string[] items)
            {
                return items;
            }
            else if (value is Task<string[]> task)
            {
                if (task.Wait(CommandSettings.AsyncTimeout) == false)
                    return [];
                return task.Result;
            }
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            Trace.TraceError($"{e}");
            return [];
        }
    }

    protected CommandPropertyBaseAttribute Attribute { get; }

    internal void SetValueInternal(object instance, object? value) => SetValue(instance, value);

    internal object? GetValueInternal(object instance) => GetValue(instance);

    internal void ValidateTrigger(ParseDescriptorCollection parseDescriptors) => OnValidateTrigger(parseDescriptors);

    internal string[]? GetCompletionInternal(object instance, string find)
    {
        if (GetCompletion(instance, find) is { } items)
        {
            var query = from item in items
                        where item.StartsWith(find)
                        select item;
            return query.ToArray();
        }
        return null;
    }

    private static string GenerateDisplayName(CommandMemberDescriptor memberDescriptor)
    {
        var itemList = new List<string>(2)
        {
            GetNamePattern(),
            GetShortNamePattern(),
        };
        if (memberDescriptor.Attribute.AllowName == true)
        {
            itemList.Reverse();
        }
        return string.Join(" | ", itemList.Where(item => item != string.Empty));

        string GetNamePattern()
        {
            if (memberDescriptor.Name == string.Empty)
                return string.Empty;
            if (memberDescriptor.IsExplicit == true)
                return CommandUtility.Delimiter + memberDescriptor.Name;
            return memberDescriptor.Name;
        }

        string GetShortNamePattern()
        {
            if (memberDescriptor.ShortName == char.MinValue)
                return string.Empty;
            if (memberDescriptor.IsExplicit == true)
                return CommandUtility.ShortDelimiter + memberDescriptor.ShortName;
            return $"{memberDescriptor.ShortName}";
        }
    }
}
