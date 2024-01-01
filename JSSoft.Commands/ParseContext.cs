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
using System.Text;

namespace JSSoft.Commands;

sealed class ParseContext(CommandMemberDescriptorCollection memberDescriptors, string[] args)
{
    public void SetValue(object instance)
    {
        ThrowIfInvalidValue(Items);

        var items = Items;
        var supportInitialize = instance as ISupportInitialize;
        var customCommandDescriptor = instance as ICustomCommandDescriptor;
        foreach (var item in items)
        {
            var memberDescriptor = item.MemberDescriptor;
            if (item.HasValue == false && item.IsOptionSet == false)
                continue;
            memberDescriptor.ValidateTrigger(items);
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
            if (item.Value is not DBNull)
            {
                memberDescriptor.SetValueInternal(obj, item.Value);
            }
        }
    }

    public ParseDescriptorCollection Items { get; } = Parse(memberDescriptors, args);

    private static ParseDescriptorCollection Parse(CommandMemberDescriptorCollection memberDescriptors, string[] args)
    {
        var parseDescriptors = new ParseDescriptorCollection(memberDescriptors);
        var implicitParseDescriptors = parseDescriptors.CreateQueue();
        var variablesMemberDescriptor = memberDescriptors.SingleOrDefault(item => item is { CommandType: CommandType.Variables });
        var variableList = new List<string>();
        var argQueue = CreateQueue(args);

        while (argQueue.Count != 0)
        {
            var arg = argQueue.Dequeue();
            if (memberDescriptors.FindByOptionName(arg) is { } memberDescriptor)
            {
                if (memberDescriptor.IsSwitch == true)
                {
                    var parseDescriptor = parseDescriptors[memberDescriptor];
                    parseDescriptor.SetSwitchValue(true);
                    parseDescriptor.IsOptionSet = true;
                }
                else
                {
                    if (argQueue.TryPeek(out var nextArg) == true && CommandUtility.IsOption(nextArg) == false && nextArg != "--")
                    {
                        var textValue = argQueue.Dequeue();
                        var parseDescriptor = parseDescriptors[memberDescriptor];
                        parseDescriptor.SetValue(textValue);
                    }
                    parseDescriptors[memberDescriptor].IsOptionSet = true;
                }
            }
            else if (arg == "--")
            {
                variableList.AddRange(argQueue.ToArray());
                argQueue.Clear();
            }
            else if (CommandUtility.IsMultipleSwitch(arg))
            {
                for (var i = 1; i < arg.Length; i++)
                {
                    var s = arg[i];
                    var item = $"-{s}";
                    if (memberDescriptors.FindByOptionName(item) is not CommandMemberDescriptor memberDescriptor1)
                        throw new InvalidOperationException($"unknown switch: '{s}'");
                    if (memberDescriptor1.MemberType != typeof(bool))
                        throw new InvalidOperationException($"unknown switch: '{s}'");
                    parseDescriptors[memberDescriptor1].IsOptionSet = true;
                }
            }
            else if (CommandUtility.IsOption(arg) == true)
            {
                variableList.Add(arg);
            }
            else
            {
                if (implicitParseDescriptors.TryDequeue(out var parseDescriptor) == true)
                {
                    parseDescriptor.SetValue(arg);
                }
                else
                {
                    variableList.Add(arg);
                }
            }
        }

        if (variablesMemberDescriptor != null && variableList.Count != 0)
        {
            var variablesParseDescriptor = parseDescriptors[variablesMemberDescriptor];
            variablesParseDescriptor.SetVariablesValue(variableList);
            variableList.Clear();
        }
        if (variableList.Count != 0)
        {
            var sb = new StringBuilder();
            sb.AppendLine("There are unprocessed arguments.");
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
        foreach (var item in args)
        {
            queue.Enqueue(item);
        }
        return queue;
    }

    private static void ThrowIfInvalidValue(ParseDescriptorCollection parseDescriptors)
    {
        foreach (var item in parseDescriptors)
        {
            var memberDescriptor = item.MemberDescriptor;
            if (item.HasValue == true)
                continue;
            if (item.IsOptionSet == true && item.HasValue == false && memberDescriptor.DefaultValue is DBNull)
                CommandLineException.ThrowIfValueMissing(memberDescriptor);
            if (memberDescriptor.IsRequired == true && item.HasValue == false && memberDescriptor.DefaultValue is DBNull)
                CommandLineException.ThrowIfValueMissing(memberDescriptor);
        }
    }
}
