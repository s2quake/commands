// <copyright file="VersionCommandBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Text;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

[ResourceUsage]
public abstract class VersionCommandBase : CommandBase
{
    [CommandPropertySwitch("quiet", 'q')]
    public bool IsQuiet { get; set; }

    protected override void OnExecute()
    {
        var settings = CommandContext.Settings;
        using var writer = new CommandTextWriter(Out, settings);
        var text = IsQuiet == true ? GetQuietString() : GetDetailedString();
        writer.WriteLine(text);
    }

    protected string GetDetailedString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{CommandContext.Name} {CommandContext.Version}");
        sb.AppendIf(CommandContext.Copyright, item => item != string.Empty);
        return sb.ToString();
    }

    protected string GetQuietString() => CommandContext.Version;
}
