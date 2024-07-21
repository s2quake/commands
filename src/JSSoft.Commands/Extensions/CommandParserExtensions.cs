// <copyright file="CommandParserExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Extensions;

public static class CommandParserExtensions
{
    public static bool TryParseCommandLine(this CommandParser @this, string commandLine)
    {
        var (commandName, commandArguments) = CommandUtility.SplitCommandLine(commandLine);
        @this.ThrowIfNotVerifyCommandName(commandName);
        return @this.TryParse(commandArguments);
    }

    public static bool TryParse(this CommandParser @this, string[] args)
    {
        try
        {
            @this.Parse(args);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool TryParse(this CommandParser @this, string argumentLine)
    {
        var args = CommandUtility.Split(argumentLine);
        return @this.TryParse(args);
    }

    public static void ParseCommandLine(this CommandParser @this, string commandLine)
    {
        var (commandName, commandArguments) = CommandUtility.SplitCommandLine(commandLine);
        @this.ThrowIfNotVerifyCommandName(commandName);
        @this.Parse(commandArguments);
    }

    public static void Parse(this CommandParser @this, string argumentLine)
    {
        var args = CommandUtility.Split(argumentLine);
        @this.Parse(args);
    }
}
