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

public sealed class CommandCompletionContext
{
    private CommandCompletionContext(ICommand command, CommandMemberDescriptor member, string[] args, string find, IReadOnlyDictionary<string, object?> properties)
    {
        Command = command;
        MemberDescriptor = member;
        Arguments = args;
        Find = find;
        Properties = properties;
    }

    internal static object? Create(ICommand command, CommandMemberDescriptorCollection memberDescriptors, string[] args, string find)
    {
        var parseContext = new ParseContext(memberDescriptors, args);
        var properties = new Dictionary<string, object?>();
        var parseDescriptorByMemberDescriptor = parseContext.Items.ToDictionary(item => item.MemberDescriptor);

        foreach (var item in parseDescriptorByMemberDescriptor.ToArray())
        {
            var memberDescriptor = item.Key;
            var parseDescriptor = item.Value;
            if (parseDescriptor.HasValue == true)
            {
                properties.Add(memberDescriptor.MemberName, parseDescriptor.Value);
                if (memberDescriptor.CommandType != CommandType.Variables)
                    parseDescriptorByMemberDescriptor.Remove(memberDescriptor);
            }
        }

        if (find.StartsWith(CommandUtility.Delimiter) == true)
        {
            var argList = new List<string>();
            var optionName = find.Substring(CommandUtility.Delimiter.Length);
            foreach (var item in parseDescriptorByMemberDescriptor)
            {
                var memberDescriptor = item.Key;
                if (memberDescriptor.IsExplicit == false)
                    continue;
            }
            return argList.OrderBy(item => item).ToArray();
        }
        else if (find.StartsWith(CommandUtility.ShortDelimiter) == true)
        {
            var argList = new List<string>();
            foreach (var item in parseDescriptorByMemberDescriptor)
            {
                var memberDescriptor = item.Key;
                if (memberDescriptor.IsExplicit == false)
                    continue;
            }
            return argList.OrderBy(item => item).ToArray();
        }
        else if (parseDescriptorByMemberDescriptor.Count != 0)
        {
            var memberDescriptor = parseDescriptorByMemberDescriptor.Keys.First();
            return new CommandCompletionContext(command, memberDescriptor, [.. args], find, properties);
        }
        return null;
    }

    public ICommand Command { get; }

    public CommandMemberDescriptor MemberDescriptor { get; }

    public string Find { get; }

    public string[] Arguments { get; }

    public IReadOnlyDictionary<string, object?> Properties { get; }

    public string MemberName => MemberDescriptor.MemberName;
}
