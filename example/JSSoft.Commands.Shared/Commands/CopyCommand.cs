// <copyright file="CopyCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;

namespace JSSoft.Commands.Applications.Commands;

[Export(typeof(ICommand))]
[ResourceUsage]
[Category("IO")]
internal sealed class CopyCommand : CommandBase
{
    public CopyCommand()
        : base("copy", ["cp"])
    {
    }

    [CommandPropertyRequired]
    public string SourcePath { get; set; } = string.Empty;

    [CommandPropertyRequired]
    public string TargetPath { get; set; } = string.Empty;

    [CommandPropertySwitch('o', useName: true)]
    public bool OverWrite { get; set; }

    protected override void OnExecute()
    {
        File.Copy(SourcePath, TargetPath, OverWrite);
    }
}
