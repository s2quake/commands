// <copyright file="ParseContext.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using System.Text;

namespace JSSoft.Commands;

internal sealed class ParseContext(
    CommandMemberDescriptorCollection memberDescriptors, CommandSettings settings, string[] args)
{
    public ParseDescriptorCollection Items { get; } = Parse(memberDescriptors, args);

    public void SetValue(object instance)
    {
        ThrowIfInvalidValue(Items);

        var items = Items;
        var supportInitialize = instance as ISupportInitialize;
        var customCommandDescriptor = instance as ICustomCommandDescriptor;
        foreach (var item in items)
        {
            var memberDescriptor = item.MemberDescriptor;
            if (item.HasValue is false && item.IsOptionSet is false)
            {
                continue;
            }

            memberDescriptor.VerifyTrigger(items);
        }

        supportInitialize?.BeginInit();
        foreach (var item in items)
        {
            var memberDescriptor = item.MemberDescriptor;
            var obj = customCommandDescriptor?.GetMemberOwner(memberDescriptor) ?? instance;
            memberDescriptor.SetValueInternal(obj, item.InitValue);
        }

        supportInitialize?.EndInit();
        foreach (var item in items)
        {
            var memberDescriptor = item.MemberDescriptor;
            var obj = customCommandDescriptor?.GetMemberOwner(memberDescriptor) ?? instance;
            var value = item.Value;
            if (value is not DBNull)
            {
                memberDescriptor.SetValueInternal(obj, value);
            }
        }

        foreach (var item in items)
        {
            var memberDescriptor = item.MemberDescriptor;
            var obj = customCommandDescriptor?.GetMemberOwner(memberDescriptor) ?? instance;
            var value = memberDescriptor.GetValueInternal(obj);
            var valueValidator = settings.ValueValidator;
            item.ValidateValue(valueValidator, obj, value);
        }
    }

    private static ParseDescriptorCollection Parse(
        CommandMemberDescriptorCollection memberDescriptors, string[] args)
    {
        var parseDescriptors = new ParseDescriptorCollection(memberDescriptors);
        var implicitParseDescriptors = parseDescriptors.CreateQueue();
        var variablesDescriptor = memberDescriptors.VariablesDescriptor;
        var argQueue = CreateQueue(args);
        var variableList = new List<string>(argQueue.Count);

        while (argQueue.Count is not 0)
        {
            var arg = argQueue.Dequeue();
            if (memberDescriptors.FindByOptionName(arg) is { } memberDescriptor)
            {
                if (memberDescriptor.IsSwitch is true)
                {
                    var parseDescriptor = parseDescriptors[memberDescriptor];
                    parseDescriptor.SetSwitchValue(true);
                    parseDescriptor.IsOptionSet = true;
                }
                else
                {
                    if (argQueue.TryPeek(out var nextArg) is true
                        && CommandUtility.IsOption(nextArg) is false
                        && nextArg != "--")
                    {
                        var textValue = argQueue.Dequeue();
                        var parseDescriptor = parseDescriptors[memberDescriptor];
                        parseDescriptor.SetValue(textValue);
                    }

                    parseDescriptors[memberDescriptor].IsOptionSet = true;
                }
            }
            else if (arg is "--")
            {
                variableList.AddRange([.. argQueue]);
                argQueue.Clear();
            }
            else if (CommandUtility.IsMultipleSwitch(arg))
            {
                for (var i = 1; i < arg.Length; i++)
                {
                    var s = arg[i];
                    var item = $"-{s}";
                    if (memberDescriptors.FindByOptionName(item) is not { } memberDescriptor1)
                    {
                        throw new InvalidOperationException($"Unknown switch: '{s}'.");
                    }

                    if (memberDescriptor1.MemberType != typeof(bool))
                    {
                        throw new InvalidOperationException($"Unknown switch: '{s}'.");
                    }

                    parseDescriptors[memberDescriptor1].IsOptionSet = true;
                }
            }
            else if (CommandUtility.IsOption(arg) is true)
            {
                variableList.Add(arg);
            }
            else
            {
                if (implicitParseDescriptors.TryDequeue(out var parseDescriptor) is true)
                {
                    parseDescriptor.SetValue(arg);
                }
                else
                {
                    variableList.Add(arg);
                }
            }
        }

        if (variablesDescriptor is not null && variableList.Count is not 0)
        {
            var variablesParseDescriptor = parseDescriptors[variablesDescriptor];
            variablesParseDescriptor.SetVariablesValue(variableList);
            variableList.Clear();
        }

        if (variableList.Count is not 0)
        {
            var sb = new StringBuilder();
            sb.AppendLine("There are unhandled arguments.");
            foreach (var item in variableList)
            {
                sb.AppendLine($"    {item}");
            }

            throw new CommandLineException(sb.ToString());
        }

        return parseDescriptors;
    }

    private static Queue<string> CreateQueue(string[] args)
    {
        var queue = new Queue<string>(args.Length);
        foreach (var arg in args)
        {
            queue.Enqueue(arg);
        }

        return queue;
    }

    private static void ThrowIfInvalidValue(ParseDescriptorCollection parseDescriptors)
    {
        foreach (var parseDescriptor in parseDescriptors)
        {
            parseDescriptor.ThrowIfValueMissing();
        }
    }
}
