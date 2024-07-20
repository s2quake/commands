// <copyright file="CommandInvocationHelp.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[ResourceUsage(typeof(HelpCommandBase))]
internal sealed class CommandInvocationHelp
{
    [CommandPropertyRequired(DefaultValue = "")]
    public string Command { get; set; } = string.Empty;

    [CommandPropertySwitch("detail")]
    public bool IsDetail { get; set; }

    public static CommandInvocationHelp Create(CommandInvocationException e)
    {
        var settings = e.Invoker.Settings;
        var name = e.Arguments[0];
        var args = e.Arguments.Where(item => settings.IsHelpArg(item) != true).ToArray();
        var obj = new CommandInvocationHelp();
        var parser = new HelpCommandParser(name, obj);
        parser.Parse(args);
        return obj;
    }

    private sealed class HelpCommandParser(string commandName, object instance)
        : CommandParser(commandName, instance)
    {
        protected override void OnValidate(string[] args)
        {
            // do nothing
        }
    }
}
