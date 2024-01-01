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

using System.IO;
using System.Runtime.CompilerServices;

namespace JSSoft.Commands;

public static class CommandUsageUtility
{
    public static void Print(TextWriter writer, CommandParsingException e)
    {
        var action = GetAction(e.Error);
        action.Invoke(writer, e);

        static Action<TextWriter, CommandParsingException> GetAction(CommandParsingError error) => error switch
        {
            CommandParsingError.Empty => PrintSummary,
            CommandParsingError.Help => PrintHelp,
            CommandParsingError.Version => PrintVersion,
            _ => throw new SwitchExpressionException(),
        };
    }

    public static void Print(TextWriter writer, CommandInvocationException e)
    {
        var action = GetAction(e.Error);
        action.Invoke(writer, e);

        static Action<TextWriter, CommandInvocationException> GetAction(CommandInvocationError error) => error switch
        {
            CommandInvocationError.Empty => PrintSummary,
            CommandInvocationError.Help => PrintHelp,
            CommandInvocationError.Version => PrintVersion,
            _ => throw new SwitchExpressionException(),
        };
    }

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

    private static void PrintSummary(TextWriter writer, string executionName, CommandSettings settings)
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
                itemList.Add($"{CommandUtility.Delimiter}{settings.HelpName}");
            if (settings.HelpShortName != char.MinValue)
                itemList.Add($"{CommandUtility.ShortDelimiter}{settings.HelpShortName}");
            return string.Join(" | ", itemList);
        }

        static string GetVersionNames(CommandSettings settings)
        {
            var itemList = new List<string>(2);
            if (settings.VersionName != string.Empty)
                itemList.Add($"{CommandUtility.Delimiter}{settings.VersionName}");
            if (settings.VersionShortName != char.MinValue)
                itemList.Add($"{CommandUtility.ShortDelimiter}{settings.VersionShortName}");
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
        var text = settings.IsQuiet == false ? settings.GetDetailedString() : settings.GetQuietString();
        writer.WriteLine(text);
    }

    private static void PrintVersion(TextWriter writer, CommandInvocationException e)
    {
        var settings = CommandInvocationVersion.Create(e);
        var text = settings.IsQuiet == false ? settings.GetDetailedString() : settings.GetQuietString();
        writer.WriteLine(text);
    }
}
