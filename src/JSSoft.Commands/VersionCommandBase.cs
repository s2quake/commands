// <copyright file="VersionCommandBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using System.Text;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

[ResourceUsage]
[Category]
public abstract class VersionCommandBase : CommandBase
{
    protected VersionCommandBase()
        : base("version")
    {
    }

    [CommandPropertySwitch("quiet", 'q')]
    public bool IsQuiet { get; set; }

    protected override void OnExecute()
    {
        var settings = Context.Settings;
        using var writer = new CommandTextWriter(Out, settings);
        var text = IsQuiet is true ? GetQuietString() : GetDetailedString();
        writer.WriteLine(text);
    }

    protected string GetDetailedString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{Context.Name} {Context.Version}");
        sb.AppendIf(Context.Copyright, item => item != string.Empty);
        return sb.ToString();
    }

    protected string GetQuietString() => Context.Version;
}
