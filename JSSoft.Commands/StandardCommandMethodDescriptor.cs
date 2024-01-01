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

sealed class StandardCommandMethodDescriptor : CommandMethodDescriptor
{
    private readonly PropertyInfo? _validationPropertyInfo;
    private readonly MethodInfo? _completionMethodInfo;
    private readonly bool _isStatic;

    public StandardCommandMethodDescriptor(MethodInfo methodInfo)
        : base(methodInfo)
    {
        ThrowUtility.ThrowIfDeclaringTypeNull(methodInfo);

        MethodName = methodInfo.Name;
        IsAsync = CommandMethodUtility.IsAsync(methodInfo);
        Name = CommandMethodUtility.GetName(methodInfo);
        Aliases = CommandMethodUtility.GetAliases(methodInfo);
        DisplayName = CommandMethodUtility.GetDisplayName(methodInfo);
        Members = CommandDescriptor.GetMemberDescriptorsByMethodInfo(methodInfo);
        Category = AttributeUtility.GetCategory(methodInfo);
        _validationPropertyInfo = CommandMethodUtility.GetValidationPropertyInfo(methodInfo);
        _completionMethodInfo = CommandMethodUtility.GetCompletionMethodInfo(methodInfo);
        _isStatic = TypeUtility.IsStaticClass(methodInfo.DeclaringType!);
    }

    public override string MethodName { get; }

    public override string Name { get; }

    public override string[] Aliases { get; }

    public override string DisplayName { get; }

    public override string Category { get; }

    public override bool IsAsync { get; }

    public override CommandMemberDescriptorCollection Members { get; }

    protected override object? OnInvoke(object instance, object?[] parameters)
    {
        if (MethodInfo.DeclaringType!.IsAbstract && MethodInfo.DeclaringType.IsSealed == true)
        {
            return MethodInfo.Invoke(null, parameters);
        }
        else
        {
            return MethodInfo.Invoke(instance, parameters);
        }
    }

    protected override bool OnCanExecute(object instance)
    {
        if (_validationPropertyInfo?.GetValue(instance) is bool @bool)
        {
            return @bool;
        }
        return base.OnCanExecute(instance);
    }

    protected override string[] GetCompletion(object instance, object?[] parameters)
    {
        if (_completionMethodInfo != null)
        {
            return InvokeCompletionMethod(instance, parameters);
        }
        return base.GetCompletion(instance, parameters);
    }

    private string[] InvokeCompletionMethod(object instance, object?[] parameters)
    {
        try
        {
            var value = _completionMethodInfo!.Invoke(instance, parameters);
            if (value is string[] items)
            {
                return items;
            }
            else if (value is Task<string[]> task)
            {
                if (task.Wait(CommandSettings.AsyncTimeout) == false)
                    return [];
                return task.Result;
            }
            throw new UnreachableException();
        }
        catch (Exception e)
        {
            Trace.TraceError($"{e}");
            return [];
        }
    }
}
