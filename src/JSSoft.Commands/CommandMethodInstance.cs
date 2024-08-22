// <copyright file="CommandMethodInstance.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using System.Threading;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

internal class CommandMethodInstance : ICustomCommandDescriptor, ISupportInitialize
{
    private readonly ParameterInfo[] _parameterInfos;
    private readonly string[] _methodParameterNames;
    private readonly Dictionary<Type, object> _valueByType = [];
    private readonly CommandMethodDescriptor _methodDescriptor;
    private readonly object _instance;

    public CommandMethodInstance(CommandMethodDescriptor methodDescriptor)
        : this(methodDescriptor, instance: null!)
    {
    }

    public CommandMethodInstance(
        CommandMethodDescriptor methodDescriptor,
        object instance)
    {
        var methodInfo = methodDescriptor.MethodInfo;
        var parameterInfos = methodInfo.GetParameters();
        var methodParameterNames = methodInfo.GetMethodParameterNames();
        _parameterInfos = parameterInfos;
        _methodParameterNames = methodParameterNames;
        _methodDescriptor = methodDescriptor;
        _instance = instance;
    }

    CommandMemberDescriptorCollection ICustomCommandDescriptor.Members => _methodDescriptor.Members;

    public object?[] GetParameters()
    {
        var valueList = new List<object?>(_parameterInfos.Length);
        foreach (var parameterInfo in _parameterInfos)
        {
            if (CommandUtility.IsSupportedType(parameterInfo.ParameterType) != true
                && _methodParameterNames.Contains(parameterInfo.Name) == true)
            {
                valueList.Add(_valueByType[parameterInfo.ParameterType]);
            }
            else
            {
                var memberDescriptor = _methodDescriptor.Members[parameterInfo.Name!];
                var value = memberDescriptor.GetValueInternal(_instance);
                valueList.Add(value);
            }
        }

        return [.. valueList];
    }

    public object?[] GetParameters(
        CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        var valueList = new List<object?>(_parameterInfos.Length);
        foreach (var parameterInfo in _parameterInfos)
        {
            if (_methodParameterNames.Contains(parameterInfo.Name) == true)
            {
                valueList.Add(_valueByType[parameterInfo.ParameterType]);
            }
            else if (parameterInfo.ParameterType == typeof(CancellationToken))
            {
                valueList.Add(cancellationToken);
            }
            else if (parameterInfo.IsProgressParameter() == true)
            {
                valueList.Add(new CommandProgress(progress));
            }
            else
            {
                var memberDescriptor = _methodDescriptor.Members[parameterInfo.Name!];
                var value = memberDescriptor.GetValueInternal(_instance);
                valueList.Add(value);
            }
        }

        return [.. valueList];
    }

    object ICustomCommandDescriptor.GetMemberOwner(CommandMemberDescriptor memberDescriptor)
    {
        if (memberDescriptor.Owner.Type == CommandMemberType.Parameter)
        {
            return _instance;
        }
        else if (memberDescriptor.Owner.Type == CommandMemberType.Property)
        {
            var declaringType = memberDescriptor.Owner.DeclaringType;
            if (_valueByType.TryGetValue(declaringType, out var value) == true)
            {
                return value;
            }

            return _instance;
        }
        else
        {
            throw new NotSupportedException("not supported member owner");
        }
    }

    void ISupportInitialize.BeginInit()
    {
        for (var i = 0; i < _parameterInfos.Length; i++)
        {
            var parameterInfo = _parameterInfos[i];
            var parameterType = parameterInfo.ParameterType;
            if (CommandUtility.IsSupportedType(parameterType) != true
                && _methodParameterNames.Contains(parameterInfo.Name) == true)
            {
                var value = Activator.CreateInstance(parameterInfo.ParameterType)!;
                _valueByType[parameterType] = value;
            }
        }
    }

    void ISupportInitialize.EndInit()
    {
    }
}
