// <copyright file="CommandParsingException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public class CommandParsingException(
    CommandParser parser, CommandParsingError error, string[] args, Exception? innerException)
    : Exception(innerException?.Message, innerException), ICommandUsage
{
    private readonly CommandUsageDescriptorBase _usageDescriptor
        = CommandDescriptor.GetUsageDescriptor(parser.Instance.GetType());

    public CommandParsingException(CommandParser parser, CommandParsingError error, string[] args)
        : this(parser, error, args, innerException: null)
    {
    }

    public CommandParsingError Error { get; } = error;

    public string[] Arguments { get; } = args;

    public CommandParser Parser { get; } = parser;

    public CommandMemberDescriptorCollection MemberDescriptors
        => CommandDescriptor.GetMemberDescriptors(Parser.Instance);

    string ICommandUsage.ExecutionName => Parser.ExecutionName;

    string ICommandUsage.Summary => _usageDescriptor.Summary;

    string ICommandUsage.Description => _usageDescriptor.Description;

    string ICommandUsage.Example => _usageDescriptor.Example;
}
