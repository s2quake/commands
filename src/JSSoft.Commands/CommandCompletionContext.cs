// <copyright file="CommandCompletionContext.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public sealed class CommandCompletionContext
{
    private CommandCompletionContext(
        ICommand command,
        CommandMemberDescriptor memberDescriptor,
        string[] args,
        string find,
        IReadOnlyDictionary<string, object?> properties)
    {
        Command = command;
        MemberDescriptor = memberDescriptor;
        Arguments = args;
        Find = find;
        Properties = properties;
    }

    public ICommand Command { get; }

    public CommandMemberDescriptor MemberDescriptor { get; }

    public string Find { get; }

    public string[] Arguments { get; }

    public IReadOnlyDictionary<string, object?> Properties { get; }

    public string MemberName => MemberDescriptor.MemberName;

    internal static object? Create(
        ICommand command,
        CommandMemberDescriptorCollection memberDescriptors,
        string[] args,
        string find)
    {
        var parseContext = new ParseContext(memberDescriptors, args);
        var properties = new Dictionary<string, object?>();
        var parseDescriptorByMemberDescriptor
            = parseContext.Items.ToDictionary(item => item.MemberDescriptor);

        foreach (var item in parseDescriptorByMemberDescriptor.ToArray())
        {
            var memberDescriptor = item.Key;
            var parseDescriptor = item.Value;
            if (parseDescriptor.HasValue == true)
            {
                properties.Add(memberDescriptor.MemberName, parseDescriptor.Value);
                if (memberDescriptor.IsVariables != true)
                {
                    parseDescriptorByMemberDescriptor.Remove(memberDescriptor);
                }
            }
        }

        if (find.StartsWith(CommandUtility.Delimiter) == true
            || find.StartsWith(CommandUtility.ShortDelimiter) == true)
        {
            var argList = new List<string>();
            foreach (var memberDescriptor in parseDescriptorByMemberDescriptor.Keys)
            {
                if (memberDescriptor.IsExplicit != true)
                {
                    continue;
                }
            }

            return argList.OrderBy(item => item).ToArray();
        }
        else if (parseDescriptorByMemberDescriptor.Count != 0)
        {
            var memberDescriptor = parseDescriptorByMemberDescriptor.Keys.First();
            return new CommandCompletionContext(
                command, memberDescriptor, [.. args], find, properties);
        }

        return null;
    }
}
