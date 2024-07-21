// <copyright file="CleanCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.Composition;

namespace JSSoft.Commands.Applications.Commands;

[Export(typeof(ICommand))]
[ResourceUsage]
[Category("Git-like")]
internal sealed class CleanCommand : CommandBase
{
    [CommandPropertySwitch('d')]
    public bool IsDirectory { get; set; }

    [CommandPropertySwitch('f')]
    public bool IsUntrackedFiles { get; set; }

    [CommandPropertySwitch('n')]
    public bool IsDryRun { get; set; }

    [CommandPropertySwitch('x')]
    public bool IsIgnoreFiles { get; set; }

    [CommandProperty('e')]
    public string Pattern { get; set; } = string.Empty;

    protected override void OnExecute()
    {
        Out.WriteLine($"{nameof(IsDirectory)}: {IsDirectory}");
        Out.WriteLine($"{nameof(IsUntrackedFiles)}: {IsUntrackedFiles}");
        Out.WriteLine($"{nameof(IsDryRun)}: {IsDryRun}");
        Out.WriteLine($"{nameof(IsIgnoreFiles)}: {IsIgnoreFiles}");
    }
}
