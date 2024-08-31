// <copyright file="CommandInvocationException.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Runtime.CompilerServices;

namespace JSSoft.Commands;

public class CommandInvocationException(
    CommandInvoker invoker, CommandInvocationError error, string[] args, Exception? innerException)
    : Exception(innerException?.Message, innerException), ICommandUsage
{
    private readonly CommandUsageDescriptorBase _usageDescriptor
        = CommandDescriptor.GetUsageDescriptor(invoker.InstanceType);

    public CommandInvocationException(
        CommandInvoker invoker, CommandInvocationError error, string[] args)
        : this(invoker, error, args, innerException: null)
    {
    }

    public CommandInvocationError Error { get; } = error;

    public string[] Arguments { get; } = args;

    public CommandInvoker Invoker { get; } = invoker;

    public CommandMethodDescriptorCollection MethodDescriptors
        => CommandDescriptor.GetMethodDescriptors(Invoker.InstanceType);

    string ICommandUsage.ExecutionName => Invoker.ExecutionName;

    string ICommandUsage.Summary => _usageDescriptor.Summary;

    string ICommandUsage.Description => _usageDescriptor.Description;

    string ICommandUsage.Example => _usageDescriptor.Example;

    public void Print(TextWriter writer)
    {
        var action = GetAction(Error);
        action.Invoke(writer, this);
    }

    private static Action<TextWriter, CommandInvocationException> GetAction(
        CommandInvocationError error)
        => error switch
        {
            CommandInvocationError.Empty => PrintSummary,
            CommandInvocationError.Help => PrintHelp,
            CommandInvocationError.Version => PrintVersion,
            _ => throw new SwitchExpressionException(),
        };

    private static void PrintSummary(TextWriter writer, CommandInvocationException e)
    {
        var invoker = e.Invoker;
        var executionName = invoker.ExecutionName;
        var settings = invoker.Settings;
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

    private static void PrintVersion(TextWriter writer, CommandInvocationException e)
    {
        var settings = CommandInvocationVersion.Create(e);
        var text = settings.IsQuiet != true
            ? settings.GetDetailedString()
            : settings.GetQuietString();
        writer.WriteLine(text);
    }
}
