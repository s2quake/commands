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
        string find,
        IReadOnlyDictionary<string, object?> properties)
    {
        Command = command;
        MemberDescriptor = memberDescriptor;
        Find = find;
        Properties = properties;
    }

    public ICommand Command { get; }

    public CommandMemberDescriptor MemberDescriptor { get; }

    public string Find { get; }

    public IReadOnlyDictionary<string, object?> Properties { get; }

    public string MemberName => MemberDescriptor.MemberName;

    internal static object? Create(
        ICommand command,
        ParseContext parseContext,
        string find)
    {
        var properties = new Dictionary<string, object?>();
        var parseDescriptorByMemberDescriptor
            = parseContext.Items.ToDictionary(item => item.MemberDescriptor);

        foreach (var item in parseDescriptorByMemberDescriptor.ToArray())
        {
            var memberDescriptor = item.Key;
            var parseDescriptor = item.Value;
            if (parseDescriptor.HasValue is true)
            {
                properties.Add(memberDescriptor.MemberName, parseDescriptor.Value);
                if (memberDescriptor.IsVariables is false)
                {
                    parseDescriptorByMemberDescriptor.Remove(memberDescriptor);
                }
            }
        }

        if (find.StartsWith(CommandUtility.Delimiter) is false
            && find.StartsWith(CommandUtility.ShortDelimiter) is false
            && parseDescriptorByMemberDescriptor.Count != 0)
        {
            var memberDescriptor = parseDescriptorByMemberDescriptor.Keys.First();
            return new CommandCompletionContext(
                command, memberDescriptor, find, properties);
        }

        return null;
    }
}
