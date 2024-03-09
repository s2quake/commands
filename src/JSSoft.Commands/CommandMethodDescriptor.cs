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
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands;

public abstract class CommandMethodDescriptor
{
    protected CommandMethodDescriptor(MethodInfo methodInfo)
    {
        MethodInfo = methodInfo;
        UsageDescriptor = CommandDescriptor.GetUsageDescriptor(methodInfo);
    }

    public abstract string Name { get; }

    public abstract string[] Aliases { get; }

    public abstract string DisplayName { get; }

    public abstract string MethodName { get; }

    public abstract bool IsAsync { get; }

    public abstract CommandMemberDescriptorCollection Members { get; }

    public abstract string Category { get; }

    public MethodInfo MethodInfo { get; }

    public CommandUsageDescriptorBase UsageDescriptor { get; }

    protected abstract object? OnInvoke(object instance, object?[] parameters);

    protected virtual bool OnCanExecute(object instance)
    {
        return true;
    }

    protected virtual string[] GetCompletion(object instance, object?[] parameters)
    {
        return [];
    }

    internal bool CanExecute(object instance)
    {
        return OnCanExecute(instance);
    }

    internal object? Invoke(object instance, string[] args, CommandMemberDescriptorCollection memberDescriptors)
    {
        var parseContext = new ParseContext(memberDescriptors, args);
        var parameters = MethodInfo.GetParameters();
        var valueList = new List<object?>(parameters.Length);
        parseContext.SetValue(instance);
        foreach (var item in parameters)
        {
            var memberDescriptor = memberDescriptors[item.Name!];
            var value = memberDescriptor.GetValueInternal(instance);
            valueList.Add(value);
        }
        return OnInvoke(instance, [.. valueList]);
    }

    internal Task InvokeAsync(object instance, string[] args, CommandMemberDescriptorCollection memberDescriptors, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        var parseContext = new ParseContext(memberDescriptors, args);
        var parameters = MethodInfo.GetParameters();
        var valueList = new List<object?>(parameters.Length);
        parseContext.SetValue(instance);
        foreach (var item in parameters)
        {
            if (item.ParameterType == typeof(CancellationToken))
            {
                valueList.Add(cancellationToken);
            }
            else if (CommandMethodUtility.IsProgressParameter(item) == true)
            {
                valueList.Add(new CommandProgress(progress));
            }
            else
            {
                var memberDescriptor = memberDescriptors[item.Name!];
                var value = memberDescriptor.GetValueInternal(instance);
                valueList.Add(value);
            }
        }
        if (OnInvoke(instance, [.. valueList]) is Task task)
        {
            return task;
        }
        throw new UnreachableException();
    }

    internal object? Invoke(object instance, CommandMemberDescriptorCollection memberDescriptors)
    {
        var parameters = MethodInfo.GetParameters();
        var valueList = new List<object?>(parameters.Length);
        foreach (var item in parameters)
        {
            var memberDescriptor = memberDescriptors[item.Name!];
            var value = memberDescriptor.GetValueInternal(instance);
            valueList.Add(value);
        }
        return OnInvoke(instance, [.. valueList]);
    }

    internal Task InvokeAsync(object instance, CommandMemberDescriptorCollection memberDescriptors, CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        var parameters = MethodInfo.GetParameters();
        var valueList = new List<object?>(parameters.Length);
        foreach (var item in parameters)
        {
            if (item.ParameterType == typeof(CancellationToken))
            {
                valueList.Add(cancellationToken);
            }
            else if (CommandMethodUtility.IsProgressParameter(item) == true)
            {
                valueList.Add(new CommandProgress(progress));
            }
            else
            {
                var memberDescriptor = memberDescriptors[item.Name!];
                var value = memberDescriptor.GetValueInternal(instance);
                valueList.Add(value);
            }
        }
        if (OnInvoke(instance, [.. valueList]) is Task task)
        {
            return task;
        }
        throw new UnreachableException();
    }

    internal string[] GetCompletionInternal(object instance, CommandMemberDescriptor memberDescriptor, string find)
    {
        if (memberDescriptor.GetCompletionInternal(instance, find) is string[] items)
            return items;
        return GetCompletion(instance, [memberDescriptor, find]);
    }
}
