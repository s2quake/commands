// <copyright file="CommandParsingException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Runtime.CompilerServices;

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

    public void Print(TextWriter writer)
    {
        var action = GetAction(Error);
        action.Invoke(writer, this);
    }

    private static Action<TextWriter, CommandParsingException> GetAction(CommandParsingError error)
        => error switch
        {
            CommandParsingError.Empty => PrintSummary,
            CommandParsingError.Help => PrintHelp,
            CommandParsingError.Version => PrintVersion,
            _ => throw new SwitchExpressionException(),
        };

    private static void PrintSummary(
        TextWriter writer, CommandParsingException e)
    {
        var parser = e.Parser;
        var executionName = parser.ExecutionName;
        var settings = parser.Settings;
        var helpNames = GetHelpNames(settings);
        var versionNames = GetVersionNames(settings);
        if (helpNames != string.Empty)
        {
            writer.WriteLine($"Type '{executionName} {helpNames}' for usage.");
        }

        if (versionNames != string.Empty)
        {
            writer.WriteLine($"Type '{executionName} {versionNames}' for version.");
        }

        static string GetHelpNames(CommandSettings settings)
        {
            var itemList = new List<string>(2);
            if (settings.HelpName != string.Empty)
            {
                itemList.Add($"{CommandUtility.Delimiter}{settings.HelpName}");
            }

            if (settings.HelpShortName != char.MinValue)
            {
                itemList.Add($"{CommandUtility.ShortDelimiter}{settings.HelpShortName}");
            }

            return string.Join(" | ", itemList);
        }

        static string GetVersionNames(CommandSettings settings)
        {
            var itemList = new List<string>(2);
            if (settings.VersionName != string.Empty)
            {
                itemList.Add($"{CommandUtility.Delimiter}{settings.VersionName}");
            }

            if (settings.VersionShortName != char.MinValue)
            {
                itemList.Add($"{CommandUtility.ShortDelimiter}{settings.VersionShortName}");
            }

            return string.Join(" | ", itemList);
        }
    }

    private static void PrintHelp(TextWriter writer, CommandParsingException e)
    {
        var settings = e.Parser.Settings;
        var parsingHelp = CommandParsingHelp.Create(e);
        var memberDescriptors = e.MemberDescriptors;
        var usagePrinter = new CommandParsingUsagePrinter(e, settings)
        {
            IsDetail = parsingHelp.IsDetail,
        };
        usagePrinter.Print(writer, memberDescriptors);
    }

    private static void PrintVersion(TextWriter writer, CommandParsingException e)
    {
        var settings = CommandParsingVersion.Create(e);
        var text = settings.GetVersionString();
        writer.WriteLine(text);
    }
}
