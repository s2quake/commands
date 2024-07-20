// <copyright file="CommandLineException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public sealed class CommandLineException : SystemException
{
    public CommandLineException(string message)
        : base(message)
    {
    }

    public CommandLineException(string message, CommandMemberDescriptor memberDescriptor)
        : base(message)
    {
        Descriptor = memberDescriptor;
    }

    public CommandLineException(
        CommandLineError error, string message, CommandMemberDescriptor memberDescriptor)
        : base(message)
    {
        Error = error;
        Descriptor = memberDescriptor;
    }

    public CommandLineException(
        CommandLineError error,
        string message,
        CommandMemberDescriptor memberDescriptor,
        Exception? innerException)
        : base(message, innerException)
    {
        Error = error;
        Descriptor = memberDescriptor;
    }

    public CommandLineException(
        string message, CommandMemberDescriptor memberDescriptor, Exception innerException)
        : base(message, innerException)
    {
        Descriptor = memberDescriptor;
    }

    public CommandLineException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public CommandMemberDescriptor? Descriptor { get; }

    public CommandLineError Error { get; }

    public static void ThrowIfValueMissing(CommandMemberDescriptor memberDescriptor)
    {
        var error = CommandLineError.ValueMissing;
        var message = $"Option '{memberDescriptor.DisplayName}' value is missing.";
        throw new CommandLineException(error, message, memberDescriptor);
    }

    public static void ThrowIfInvalidValue(CommandMemberDescriptor memberDescriptor, string value)
    {
        var error = CommandLineError.InvalidValue;
        var message = $"""
            Value '{value}' cannot be used for option '{memberDescriptor.DisplayName}'.
            """;
        throw new CommandLineException(error, message, memberDescriptor);
    }
}
