// <copyright file="CommandInvocationVersion.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Text;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

[ResourceUsage(typeof(VersionCommandBase))]
internal sealed class CommandInvocationVersion
{
    [CommandPropertySwitch("quiet", 'q')]
    public bool IsQuiet { get; set; }

    public string ExecutionName { get; private set; } = string.Empty;

    public string Version { get; private set; } = string.Empty;

    public string Copyright { get; private set; } = string.Empty;

    public static CommandInvocationVersion Create(CommandInvocationException e)
    {
        var settings = e.Invoker.Settings;
        var name = e.Arguments[0];
        var args = e.Arguments.Where(item => settings.IsVersionArg(item) is false).ToArray();
        var obj = new CommandInvocationVersion();
        var parser = new VersionCommandParser(name, obj);
        parser.Parse(args);
        obj.ExecutionName = e.Invoker.ExecutionName;
        obj.Version = e.Invoker.Version;
        obj.Copyright = e.Invoker.Copyright;
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
        => IsQuiet is true ? GetQuietString() : GetDetailedString();

    private sealed class VersionCommandParser(string commandName, object instance)
        : CommandParser(commandName, instance)
    {
        protected override void OnVerify(string[] args)
        {
            // do nothing
        }
    }
}
