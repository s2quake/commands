// <copyright file="CommandUsageUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Runtime.CompilerServices;

namespace JSSoft.Commands;

public static class CommandUsageUtility
{
    public static void Print(TextWriter writer, CommandParsingException e)
    {
        var action = GetAction(e.Error);
        action.Invoke(writer, e);
    }

    public static void Print(TextWriter writer, CommandInvocationException e)
    {
        var action = GetAction(e.Error);
        action.Invoke(writer, e);
    }

    private static Action<TextWriter, CommandParsingException> GetAction(CommandParsingError error)
        => error switch
        {
            CommandParsingError.Empty => PrintSummary,
            CommandParsingError.Help => PrintHelp,
            CommandParsingError.Version => PrintVersion,
            _ => throw new SwitchExpressionException(),
        };

    private static Action<TextWriter, CommandInvocationException> GetAction(
        CommandInvocationError error)
        => error switch
        {
            CommandInvocationError.Empty => PrintSummary,
            CommandInvocationError.Help => PrintHelp,
            CommandInvocationError.Version => PrintVersion,
            _ => throw new SwitchExpressionException(),
        };

    private static void PrintSummary(TextWriter writer, CommandParsingException e)
    {
        var parser = e.Parser;
        var executionName = parser.ExecutionName;
        var settings = parser.Settings;
        PrintSummary(writer, executionName, settings);
    }

    private static void PrintSummary(TextWriter writer, CommandInvocationException e)
    {
        var invoker = e.Invoker;
        var executionName = invoker.ExecutionName;
        var settings = invoker.Settings;
        PrintSummary(writer, executionName, settings);
    }

    private static void PrintSummary(
        TextWriter writer, string executionName, CommandSettings settings)
    {
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

    private static void PrintHelp(TextWriter writer, CommandInvocationException e)
    {
        var settings = e.Invoker.Settings;
        var invocationHelp = CommandInvocationHelp.Create(e);
        var methodDescriptors = e.MethodDescriptors;
        var usagePrinter = new CommandInvocationUsagePrinter(e, settings)
        {
            IsDetail = invocationHelp.IsDetail,
        };
        if (invocationHelp.Command == string.Empty)
        {
            usagePrinter.Print(writer, [.. methodDescriptors]);
        }
        else if (methodDescriptors.FindByName(invocationHelp.Command) is { } methodDescriptor)
        {
            usagePrinter.Print(writer, methodDescriptor!);
        }
        else
        {
            throw new CommandLineException("weqrwqrwe");
        }
    }

    private static void PrintVersion(TextWriter writer, CommandParsingException e)
    {
        var settings = CommandParsingVersion.Create(e);
        var text = settings.GetVersionString();
        writer.WriteLine(text);
    }

    private static void PrintVersion(TextWriter writer, CommandInvocationException e)
    {
        var settings = CommandInvocationVersion.Create(e);
        var text = settings.IsQuiet != true
            ? settings.GetDetailedString()
            : settings.GetQuietString();
        writer.WriteLine(text);
    }
}
