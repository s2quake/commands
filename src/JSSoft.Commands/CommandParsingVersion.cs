// <copyright file="CommandParsingVersion.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Text;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

[ResourceUsage(typeof(VersionCommandBase))]
internal sealed class CommandParsingVersion
{
    [CommandPropertySwitch("quiet", 'q')]
    public bool IsQuiet { get; set; }

    public string ExecutionName { get; private set; } = string.Empty;

    public string Version { get; private set; } = string.Empty;

    public string Copyright { get; private set; } = string.Empty;

    public static CommandParsingVersion Create(CommandParsingException e)
    {
        var settings = e.Parser.Settings;
        var args = e.Arguments.Where(item => settings.IsVersionArg(item) != true).ToArray();
        var obj = new CommandParsingVersion();
        var parser = new VersionCommandParser($"{e.Parser.ExecutionName} {e.Arguments[0]}", obj);
        parser.Parse(args);
        obj.ExecutionName = e.Parser.ExecutionName;
        obj.Version = e.Parser.Version;
        obj.Copyright = e.Parser.Copyright;
        return obj;
    }

    public string GetDetailedString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{ExecutionName} {Version}");
        sb.AppendIf(Copyright, item => item != string.Empty);
        return sb.ToString();
    }

    public string GetQuietString() => Version;

    public string GetVersionString()
        => IsQuiet == true ? GetQuietString() : GetDetailedString();

    private sealed class VersionCommandParser(string commandName, object instance)
        : CommandParser(commandName, instance)
    {
        protected override void OnVerify(string[] args)
        {
            // do nothing
        }
    }
}
